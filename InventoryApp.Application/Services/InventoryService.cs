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
                .Include(i => i.Category)
                .Select(i => new InventoryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
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
                .Include(i => i.Category)
                .Where(i => i.Id == id)
                .Select(i => new InventoryDto
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    CategoryName = i.Category.Name,
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
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == dto.CategoryName);

            if (category == null)
                throw new Exception("Category not found");

            var inventory = new Inventory
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                CategoryId = category.Id,
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

            if (dto.Tags != null && dto.Tags.Any())
            {
                foreach (var tagName in dto.Tags.Select(t => t.Trim().ToLower()).Distinct())
                {
                    var tag = await _context.Tags
                        .FirstOrDefaultAsync(t => t.Name == tagName);

                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Id = Guid.NewGuid(),
                            Name = tagName
                        };

                        _context.Tags.Add(tag);
                    }

                    _context.InventoryTags.Add(new InventoryTag
                    {
                        InventoryId = inventory.Id,
                        Tag = tag
                    });
                }
            }

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

            var user = await _context.Users.FirstAsync(u => u.Id == userId);

            var isOwner = inventory.OwnerId == userId;
            var isAdmin = user.IsAdmin;

            if (!isOwner && !isAdmin)
                throw new UnauthorizedAccessException();

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == dto.CategoryName);

            if (category == null)
                throw new Exception("Category not found");

            inventory.Title = dto.Title;
            inventory.Description = dto.Description;
            inventory.CategoryId = category.Id;
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

            var user = await _context.Users.FirstAsync(u => u.Id == userId);

            var isOwner = inventory.OwnerId == userId;
            var isAdmin = user.IsAdmin;

            if (!isOwner && !isAdmin)
                throw new UnauthorizedAccessException();

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<InventoryStatisticsDto> GetStatisticsAsync(Guid inventoryId)
        {
            var items = await _context.Items
                .Where(i => i.InventoryId == inventoryId)
                .ToListAsync();

            var result = new InventoryStatisticsDto
            {
                ItemsCount = items.Count
            };

            if (items.Count == 0)
                return result;

            result.NumberStats.Add(new NumberFieldStatsDto
            {
                Field = "Number1",
                Avg = items.Where(i => i.Number1 != null).Average(i => (double?)i.Number1),
                Min = items.Where(i => i.Number1 != null).Min(i => (double?)i.Number1),
                Max = items.Where(i => i.Number1 != null).Max(i => (double?)i.Number1)
            });

            var topText = items
                .Where(i => i.Text1 != null)
                .GroupBy(i => i.Text1)
                .Select(g => new StringFieldStatsDto
                {
                    Field = "Text1",
                    Value = g.Key!,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            result.TopStrings.AddRange(topText);

            return result;
        }
    }
}