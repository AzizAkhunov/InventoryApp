using InventoryApp.Application.DTO;
using InventoryApp.Application.Interfaces;
using InventoryApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Application.Services
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            return await _context.Tags
                .Where(t => t.Name.ToLower().Contains(query.ToLower()))
                .OrderBy(t => t.Name)
                .Select(t => t.Name)
                .Take(20)
                .ToListAsync();
        }

        public async Task<List<string>> AutocompleteAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<string>();

            return await _context.Tags
                .Where(t => t.Name.ToLower().StartsWith(query.ToLower()))
                .OrderBy(t => t.Name)
                .Select(t => t.Name)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<TagCloudDto>> GetCloudAsync()
        {
            return await _context.InventoryTags
                .GroupBy(it => it.Tag.Name)
                .Select(g => new TagCloudDto
                {
                    Tag = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(20)
                .ToListAsync();
        }
    }
}
