using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Domain.Entities;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryApp.Application.Services
{
    public class InventoryExternalService : IInventoryExternalService
    {
        private readonly AppDbContext _context;

        public InventoryExternalService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateTokenAsync(Guid userId, Guid inventoryId)
        {
            var inventory = await _context.Inventories
                .FirstOrDefaultAsync(i => i.Id == inventoryId);

            if (inventory == null)
                throw new Exception("Inventory not found");

            var user = await _context.Users.FirstAsync(u => u.Id == userId);

            if (inventory.OwnerId != userId && !user.IsAdmin)
                throw new UnauthorizedAccessException();

            var token = Guid.NewGuid().ToString("N") + Guid.NewGuid().ToString("N");

            var entity = new InventoryApiToken
            {
                Id = Guid.NewGuid(),
                InventoryId = inventoryId,
                Token = token,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow
            };

            _context.InventoryApiTokens.Add(entity);
            await _context.SaveChangesAsync();

            return token;
        }

        public async Task<InventoryExternalDto?> GetByTokenAsync(string token)
        {
            var tokenEntity = await _context.InventoryApiTokens
                .Include(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.Token == token && x.IsActive);

            if (tokenEntity == null)
                return null;

            var inventory = tokenEntity.Inventory;

            var items = await _context.Items
                .Where(i => i.InventoryId == inventory.Id)
                .ToListAsync();

            var dto = new InventoryExternalDto
            {
                InventoryId = inventory.Id,
                Title = inventory.Title,
                ItemsCount = items.Count
            };

            AddFieldIfEnabled(dto.Fields, inventory.CustomString1Enabled, "text1", inventory.CustomString1Name, "text");
            AddFieldIfEnabled(dto.Fields, inventory.CustomString2Enabled, "text2", inventory.CustomString2Name, "text");
            AddFieldIfEnabled(dto.Fields, inventory.CustomString3Enabled, "text3", inventory.CustomString3Name, "text");

            AddFieldIfEnabled(dto.Fields, inventory.CustomNumber1Enabled, "number1", inventory.CustomNumber1Name, "number");
            AddFieldIfEnabled(dto.Fields, inventory.CustomNumber2Enabled, "number2", inventory.CustomNumber2Name, "number");
            AddFieldIfEnabled(dto.Fields, inventory.CustomNumber3Enabled, "number3", inventory.CustomNumber3Name, "number");

            AddFieldIfEnabled(dto.Fields, inventory.CustomBool1Enabled, "bool1", inventory.CustomBool1Name, "boolean");
            AddFieldIfEnabled(dto.Fields, inventory.CustomBool2Enabled, "bool2", inventory.CustomBool2Name, "boolean");
            AddFieldIfEnabled(dto.Fields, inventory.CustomBool3Enabled, "bool3", inventory.CustomBool3Name, "boolean");

            AddNumberStats(dto, items, inventory.CustomNumber1Enabled, "number1", inventory.CustomNumber1Name, x => x.Number1);
            AddNumberStats(dto, items, inventory.CustomNumber2Enabled, "number2", inventory.CustomNumber2Name, x => x.Number2);
            AddNumberStats(dto, items, inventory.CustomNumber3Enabled, "number3", inventory.CustomNumber3Name, x => x.Number3);

            AddTextStats(dto, items, inventory.CustomString1Enabled, "text1", inventory.CustomString1Name, x => x.Text1);
            AddTextStats(dto, items, inventory.CustomString2Enabled, "text2", inventory.CustomString2Name, x => x.Text2);
            AddTextStats(dto, items, inventory.CustomString3Enabled, "text3", inventory.CustomString3Name, x => x.Text3);

            AddBoolStats(dto, items, inventory.CustomBool1Enabled, "bool1", inventory.CustomBool1Name, x => x.Bool1);
            AddBoolStats(dto, items, inventory.CustomBool2Enabled, "bool2", inventory.CustomBool2Name, x => x.Bool2);
            AddBoolStats(dto, items, inventory.CustomBool3Enabled, "bool3", inventory.CustomBool3Name, x => x.Bool3);

            return dto;
        }

        private static void AddFieldIfEnabled(List<ExternalFieldDto> fields, bool enabled, string key, string? title, string type)
        {
            if (!enabled) return;

            fields.Add(new ExternalFieldDto
            {
                Key = key,
                Title = string.IsNullOrWhiteSpace(title) ? key : title,
                Type = type
            });
        }

        private static void AddNumberStats(
            InventoryExternalDto dto,
            List<Item> items,
            bool enabled,
            string key,
            string? title,
            Func<Item, decimal?> selector)
        {
            if (!enabled) return;

            var values = items
                .Select(selector)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToList();

            if (values.Count == 0) return;

            dto.NumberStats.Add(new ExternalNumberStatDto
            {
                FieldKey = key,
                FieldTitle = string.IsNullOrWhiteSpace(title) ? key : title,
                Min = values.Min(),
                Max = values.Max(),
                Avg = values.Average()
            });
        }

        private static void AddTextStats(
            InventoryExternalDto dto,
            List<Item> items,
            bool enabled,
            string key,
            string? title,
            Func<Item, string?> selector)
        {
            if (!enabled) return;

            var values = items
                .Select(selector)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .GroupBy(x => x!)
                .Select(g => new PopularTextValueDto
                {
                    Value = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            if (values.Count == 0) return;

            dto.TextStats.Add(new ExternalTextStatDto
            {
                FieldKey = key,
                FieldTitle = string.IsNullOrWhiteSpace(title) ? key : title,
                TopValues = values
            });
        }

        private static void AddBoolStats(
            InventoryExternalDto dto,
            List<Item> items,
            bool enabled,
            string key,
            string? title,
            Func<Item, bool?> selector)
        {
            if (!enabled) return;

            var values = items
                .Select(selector)
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .ToList();

            if (values.Count == 0) return;

            dto.BoolStats.Add(new ExternalBoolStatDto
            {
                FieldKey = key,
                FieldTitle = string.IsNullOrWhiteSpace(title) ? key : title,
                TrueCount = values.Count(x => x),
                FalseCount = values.Count(x => !x)
            });
        }
    }
}
