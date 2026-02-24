using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class ItemService : IItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ItemDto>> GetByInventoryAsync(Guid inventoryId)
        {
            return await _context.Items
                .Where(i => i.InventoryId == inventoryId)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    InventoryId = i.InventoryId,
                    CustomId = i.CustomId,
                    Version = i.Version,

                    Text1 = i.Text1,
                    Text2 = i.Text2,
                    Text3 = i.Text3,

                    MultiText1 = i.MultiText1,
                    MultiText2 = i.MultiText2,
                    MultiText3 = i.MultiText3,

                    Number1 = i.Number1,
                    Number2 = i.Number2,
                    Number3 = i.Number3,

                    Bool1 = i.Bool1,
                    Bool2 = i.Bool2,
                    Bool3 = i.Bool3,

                    Doc1 = i.Doc1,
                    Doc2 = i.Doc2,
                    Doc3 = i.Doc3
                })
                .ToListAsync();
        }

        public async Task<ItemDto?> GetByIdAsync(Guid id)
        {
            return await _context.Items
                .Where(i => i.Id == id)
                .Select(i => new ItemDto
                {
                    Id = i.Id,
                    InventoryId = i.InventoryId,
                    CustomId = i.CustomId,
                    Version = i.Version,

                    Text1 = i.Text1,
                    Text2 = i.Text2,
                    Text3 = i.Text3,

                    MultiText1 = i.MultiText1,
                    MultiText2 = i.MultiText2,
                    MultiText3 = i.MultiText3,

                    Number1 = i.Number1,
                    Number2 = i.Number2,
                    Number3 = i.Number3,

                    Bool1 = i.Bool1,
                    Bool2 = i.Bool2,
                    Bool3 = i.Bool3,

                    Doc1 = i.Doc1,
                    Doc2 = i.Doc2,
                    Doc3 = i.Doc3
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ItemDto> CreateAsync(Guid userId, ItemDto dto)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                InventoryId = dto.InventoryId,
                CreatedById = userId,
                CustomId = dto.CustomId,
                Version = 1,

                Text1 = dto.Text1,
                Text2 = dto.Text2,
                Text3 = dto.Text3,

                MultiText1 = dto.MultiText1,
                MultiText2 = dto.MultiText2,
                MultiText3 = dto.MultiText3,

                Number1 = dto.Number1,
                Number2 = dto.Number2,
                Number3 = dto.Number3,

                Bool1 = dto.Bool1,
                Bool2 = dto.Bool2,
                Bool3 = dto.Bool3,

                Doc1 = dto.Doc1,
                Doc2 = dto.Doc2,
                Doc3 = dto.Doc3
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            dto.Id = item.Id;
            dto.Version = item.Version;

            return dto;
        }

        public async Task<ItemDto?> UpdateAsync(Guid userId, Guid id, ItemDto dto)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return null;

            item.CustomId = dto.CustomId;
            item.Text1 = dto.Text1;
            item.Number1 = dto.Number1;
            item.Version = dto.Version;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            dto.Version = item.Version;
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return false;

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
