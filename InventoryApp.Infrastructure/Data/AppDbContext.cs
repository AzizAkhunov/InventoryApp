using InventoryApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<InventoryAccess> InventoryAccesses => Set<InventoryAccess>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<InventoryTag> InventoryTags => Set<InventoryTag>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<DiscussionPost> DiscussionPosts => Set<DiscussionPost>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<InventoryIdElement> InventoryIdElements => Set<InventoryIdElement>();
        public DbSet<ItemLike> ItemLikes => Set<ItemLike>();
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //USER
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)//Unique index on Email
                .IsUnique();

            //INVENTORY
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Owner)
                .WithMany(u => u.OwnedInventories)
                .HasForeignKey(i => i.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Inventory>()
                .Property(i => i.Version)
                .IsConcurrencyToken();

            //временно для проверки
            modelBuilder.Entity<User>().HasData(
                new User
                    {
                        Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                        Email = "admin@test.com",
                        UserName = "admin",
                        IsAdmin = true,
                        IsBlocked = false,
                        CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            //ITEM
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Inventory)
                .WithMany(u => u.Items)
                .HasForeignKey(i => i.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.CreatedBy)
                .WithMany()
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasIndex(i => new { i.InventoryId, i.CustomId })
                .IsUnique();

            modelBuilder.Entity<Item>()
                .Property(i => i.Version)
                .IsConcurrencyToken();


            //INVENTORY ACCESS
            modelBuilder.Entity<InventoryAccess>()
            .HasOne(a => a.Inventory)
            .WithMany(i => i.AccessList)
            .HasForeignKey(a => a.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryAccess>()
                .HasOne(a => a.User)
                .WithMany(u => u.AccessibleInventories)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryAccess>()
                .HasKey(x => new { x.InventoryId, x.UserId });


            //TAG (unique name)
            modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

            //INVENTORY TAG

            modelBuilder.Entity<InventoryTag>()
            .HasKey(it => new { it.InventoryId, it.TagId });

            modelBuilder.Entity<InventoryTag>()
                .HasOne(it => it.Inventory)
                .WithMany(i => i.InventoryTags)
                .HasForeignKey(it => it.InventoryId);

            modelBuilder.Entity<InventoryTag>()
                .HasOne(it => it.Tag)
                .WithMany(t => t.InventoryTags)
                .HasForeignKey(it => it.TagId);

            // CATEGORY

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Inventories)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Equipment" },
                new Category { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Furniture" },
                new Category { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Books" },
                new Category { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Other" }
            );

            //COMMENT

            modelBuilder.Entity<Comment>()
           .HasOne(c => c.Inventory)
           .WithMany(i => i.Comments)
           .HasForeignKey(c => c.InventoryId)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            //LIKE

            modelBuilder.Entity<ItemLike>()
    .HasKey(l => new { l.ItemId, l.UserId });

            modelBuilder.Entity<ItemLike>()
                .HasOne(l => l.Item)
                .WithMany(i => i.Likes)
                .HasForeignKey(l => l.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemLike>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemLike>()
                .HasIndex(l => new { l.ItemId, l.UserId })
                .IsUnique();

            //InventoryID ELEMENTS
            modelBuilder.Entity<InventoryIdElement>()
                .HasOne(e => e.Inventory)
                .WithMany(i => i.IdElements)
                .HasForeignKey(e => e.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryIdElement>()
                .HasIndex(e => new { e.InventoryId, e.Order })
                .IsUnique();


            //DiscussionPost
            modelBuilder.Entity<DiscussionPost>()
                .HasOne(p => p.Inventory)
                .WithMany()
                .HasForeignKey(p => p.InventoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DiscussionPost>()
                .HasOne(p => p.Author)
                .WithMany()
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
