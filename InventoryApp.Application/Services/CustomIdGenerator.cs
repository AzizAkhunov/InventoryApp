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

            if (!elements.Any())
            {
                var lastId = await _context.Items
                    .Where(i => i.InventoryId == inventoryId)
                    .OrderByDescending(i => i.CustomId)
                    .Select(i => i.CustomId)
                    .FirstOrDefaultAsync();

                int next = 1;

                if (lastId != null && int.TryParse(lastId, out int parsed))
                    next = parsed + 1;

                return next.ToString("D5");
            }

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

                        int sequenceLength = element.Padding ?? 1;

                        int prefixLength = 0;

                        foreach (var prev in elements.Where(e => e.Order < element.Order))
                        {
                            switch (prev.Type)
                            {
                                case IdElementType.FixedText:
                                    prefixLength += prev.FixedText?.Length ?? 0;
                                    break;

                                case IdElementType.Random20Bit:
                                    prefixLength += 7;
                                    break;

                                case IdElementType.Random32Bit:
                                    prefixLength += 10;
                                    break;

                                case IdElementType.Random6Digit:
                                    prefixLength += 6;
                                    break;

                                case IdElementType.Random9Digit:
                                    prefixLength += 9;
                                    break;

                                case IdElementType.Guid:
                                    prefixLength += 32;
                                    break;

                                case IdElementType.DateTime:
                                    prefixLength += 14;
                                    break;

                                case IdElementType.Sequence:
                                    prefixLength += prev.Padding ?? 1;
                                    break;
                            }
                        }

                        var lastId = await _context.Items
                            .Where(i => i.InventoryId == inventoryId)
                            .OrderByDescending(i => i.CustomId)
                            .Select(i => i.CustomId)
                            .FirstOrDefaultAsync();

                        int next = 1;

                        if (lastId != null && lastId.Length >= prefixLength + sequenceLength)
                        {
                            var seqPart = lastId.Substring(prefixLength, sequenceLength);

                            if (int.TryParse(seqPart, out int parsed))
                                next = parsed + 1;
                        }

                        sb.Append(next.ToString().PadLeft(sequenceLength, '0'));

                        break;
                }
            }

            return sb.ToString();
        }
    }
}
