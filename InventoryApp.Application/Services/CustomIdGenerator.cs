using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Enums;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace InventoryApp.Application.Services
{
    public class CustomIdGenerator : ICustomIdGenerator
    {
        private readonly AppDbContext _context;

        public CustomIdGenerator(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateAsync(Guid inventoryId)
        {
            var elements = await _context.InventoryIdElements
                .Where(e => e.InventoryId == inventoryId)
                .OrderBy(e => e.Order)
                .ToListAsync();

            var sb = new StringBuilder();

            foreach (var element in elements)
            {
                switch (element.Type)
                {
                    case IdElementType.FixedText:
                        if (string.IsNullOrWhiteSpace(element.FixedText))
                            throw new Exception("FixedText element must have value.");
                        sb.Append(element.FixedText);
                        break;
                    case IdElementType.Random20Bit:
                        sb.Append(Random.Shared.Next(0, 1 << 20));
                        break;

                    case IdElementType.Random32Bit:
                        sb.Append(Random.Shared.NextInt64(0, 1L << 32));
                        break;

                    case IdElementType.Random6Digit:
                        sb.Append(Random.Shared.Next(100000, 999999));
                        break;

                    case IdElementType.Random9Digit:
                        sb.Append(Random.Shared.Next(100000000, 999999999));
                        break;

                    case IdElementType.Guid:
                        sb.Append(Guid.NewGuid().ToString("N"));
                        break;

                    case IdElementType.DateTime:
                        sb.Append(DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
                        break;

                    case IdElementType.Sequence:

                        var existingIds = await _context.Items
                            .Where(i => i.InventoryId == inventoryId)
                            .Select(i => i.CustomId)
                            .ToListAsync();

                        int maxSequence = 0;

                        foreach (var id in existingIds)
                        {
                            if (int.TryParse(id, out int number))
                            {
                                if (number > maxSequence)
                                    maxSequence = number;
                            }
                        }

                        var next = maxSequence + 1;

                        if (element.Padding.HasValue)
                            sb.Append(next.ToString().PadLeft(element.Padding.Value, '0'));
                        else
                            sb.Append(next);

                        break;
                }
            }

            return sb.ToString();
        }
    }
}
