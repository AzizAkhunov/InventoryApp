using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<InventoryDto>> GetAllAsync()
        {
            return await _context.Inventories
                .Select(i => new InventoryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CategoryId = i.CategoryId,
                    IsPublic = i.IsPublic,
                    Version = i.Version,

                    CustomString1Enabled = i.CustomString1Enabled,
                    CustomString1Name = i.CustomString1Name,
                    CustomString2Enabled = i.CustomString2Enabled,
                    CustomString2Name = i.CustomString2Name,
                    CustomString3Enabled = i.CustomString3Enabled,
                    CustomString3Name = i.CustomString3Name,

                    CustomNumber1Enabled = i.CustomNumber1Enabled,
                    CustomNumber1Name = i.CustomNumber1Name,
                    CustomNumber2Enabled = i.CustomNumber2Enabled,
                    CustomNumber2Name = i.CustomNumber2Name,
                    CustomNumber3Enabled = i.CustomNumber3Enabled,
                    CustomNumber3Name = i.CustomNumber3Name,

                    CustomBool1Enabled = i.CustomBool1Enabled,
                    CustomBool1Name = i.CustomBool1Name,
                    CustomBool2Enabled = i.CustomBool2Enabled,
                    CustomBool2Name = i.CustomBool2Name,
                    CustomBool3Enabled = i.CustomBool3Enabled,
                    CustomBool3Name = i.CustomBool3Name
                })
                .ToListAsync();
        }

        public async Task<InventoryDto?> GetByIdAsync(Guid id)
        {
            return await _context.Inventories
                .Where(i => i.Id == id)
                .Select(i => new InventoryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CategoryId = i.CategoryId,
                    IsPublic = i.IsPublic,
                    Version = i.Version,

                    CustomString1Enabled = i.CustomString1Enabled,
                    CustomString1Name = i.CustomString1Name,
                    CustomString2Enabled = i.CustomString2Enabled,
                    CustomString2Name = i.CustomString2Name,
                    CustomString3Enabled = i.CustomString3Enabled,
                    CustomString3Name = i.CustomString3Name,

                    CustomNumber1Enabled = i.CustomNumber1Enabled,
                    CustomNumber1Name = i.CustomNumber1Name,
                    CustomNumber2Enabled = i.CustomNumber2Enabled,
                    CustomNumber2Name = i.CustomNumber2Name,
                    CustomNumber3Enabled = i.CustomNumber3Enabled,
                    CustomNumber3Name = i.CustomNumber3Name,

                    CustomBool1Enabled = i.CustomBool1Enabled,
                    CustomBool1Name = i.CustomBool1Name,
                    CustomBool2Enabled = i.CustomBool2Enabled,
                    CustomBool2Name = i.CustomBool2Name,
                    CustomBool3Enabled = i.CustomBool3Enabled,
                    CustomBool3Name = i.CustomBool3Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<InventoryDto> CreateAsync(Guid userId, InventoryDto dto)
        {
            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                OwnerId = userId,
                IsPublic = dto.IsPublic,
                Version = 1,

                CustomString1Enabled = dto.CustomString1Enabled,
                CustomString1Name = dto.CustomString1Name,
                CustomString2Enabled = dto.CustomString2Enabled,
                CustomString2Name = dto.CustomString2Name,
                CustomString3Enabled = dto.CustomString3Enabled,
                CustomString3Name = dto.CustomString3Name,

                CustomNumber1Enabled = dto.CustomNumber1Enabled,
                CustomNumber1Name = dto.CustomNumber1Name,
                CustomNumber2Enabled = dto.CustomNumber2Enabled,
                CustomNumber2Name = dto.CustomNumber2Name,
                CustomNumber3Enabled = dto.CustomNumber3Enabled,
                CustomNumber3Name = dto.CustomNumber3Name,

                CustomBool1Enabled = dto.CustomBool1Enabled,
                CustomBool1Name = dto.CustomBool1Name,
                CustomBool2Enabled = dto.CustomBool2Enabled,
                CustomBool2Name = dto.CustomBool2Name,
                CustomBool3Enabled = dto.CustomBool3Enabled,
                CustomBool3Name = dto.CustomBool3Name
            };

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();

            dto.Id = inventory.Id;
            dto.Version = inventory.Version;

            return dto;
        }

        public async Task<InventoryDto?> UpdateAsync(Guid userId, Guid id, InventoryDto dto)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
                return null;

            if (inventory.OwnerId != userId)
                throw new UnauthorizedAccessException();

            inventory.Title = dto.Title;
            inventory.Description = dto.Description;
            inventory.CategoryId = dto.CategoryId;
            inventory.IsPublic = dto.IsPublic;

            inventory.CustomString1Enabled = dto.CustomString1Enabled;
            inventory.CustomString1Name = dto.CustomString1Name;

            inventory.Version = dto.Version;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            dto.Version = inventory.Version;
            return dto;
        }

        public async Task<bool> DeleteAsync(Guid userId, Guid id)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inventory == null)
                return false;

            if (inventory.OwnerId != userId)
                throw new UnauthorizedAccessException();

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
