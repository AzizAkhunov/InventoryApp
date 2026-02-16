using InventoryApp.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<User> Users => Set<User>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<InventoryField> InventoryFields => Set<InventoryField>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<InventoryAccess> InventoryAccesses => Set<InventoryAccess>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<InventoryTag> InventoryTags => Set<InventoryTag>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Like> Likes => Set<Like>();


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

            //INVENTORY FIELD
            modelBuilder.Entity<InventoryField>()
           .HasOne(f => f.Inventory)
           .WithMany(i => i.InventoryFields)
           .HasForeignKey(f => f.InventoryId)
           .OnDelete(DeleteBehavior.Cascade);

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
                .HasIndex(a => new { a.InventoryId, a.UserId })
                .IsUnique();


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

            modelBuilder.Entity<Like>()
            .HasOne(l => l.Item)
            .WithMany(i => i.Likes)
            .HasForeignKey(l => l.ItemId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.ItemId, l.UserId })
                .IsUnique();
        }
    }
}
