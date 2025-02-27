using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace TaskManagement.Models
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<TaskLabel> TaskLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Task>()
                .ToTable("Tasks")
                .HasKey(t => t.Id);

            modelBuilder.Entity<Task>()
                .Property(t => t.Title)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .HasColumnType("text")
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Category>()
                .ToTable("Categories")
                .HasKey(c => c.Id);

            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<TaskComment>()
                .ToTable("TaskComments")
                .HasKey(tc => tc.Id);

            modelBuilder.Entity<TaskComment>()
                .Property(tc => tc.Content)
                .IsRequired()
                .HasColumnType("text");

            modelBuilder.Entity<TaskComment>()
                .Property(tc => tc.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Quan hệ 1-N: Một User có nhiều Task
            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);  // Đảm bảo UserId là nullable

            // Quan hệ 1-N: Một Category có nhiều Task
            modelBuilder.Entity<Task>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);  

            // Quan hệ 1-N: Một Task có nhiều Comment
            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Task thì xóa luôn Comment

            // Quan hệ 1-N: Một User có nhiều Comment
            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.TaskComments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quan hệ Many-to-Many: Task - Label
            modelBuilder.Entity<TaskLabel>()
                .HasKey(tl => new { tl.TaskId, tl.LabelId });

            modelBuilder.Entity<TaskLabel>()
                .HasOne(tl => tl.Tasks)
                .WithMany(t => t.TaskLabels)
                .HasForeignKey(tl => tl.TaskId);

            modelBuilder.Entity<TaskLabel>()
                .HasOne(tl => tl.Labels)
                .WithMany(l => l.TaskLabels)
                .HasForeignKey(tl => tl.LabelId);

            //modelBuilder.Entity<User>().HasData(
            //    new User { Id = 1, Username = "admin", Email = "admin@example.com",Role="admin", PasswordHash = "hashed_password_123" },
            //    new User { Id = 2, Username = "john", Email = "john@example.com", Role = "user", PasswordHash = "hashed_password_456" }
            //);

            //modelBuilder.Entity<Category>().HasData(
            //    new Category { Id = 1, Name = "Work", Description = "Tasks related to work" },
            //    new Category { Id = 2, Name = "Personal", Description = "Personal tasks" }
            //);

            //modelBuilder.Entity<TaskManagement.Models.Task>().HasData(
            //    new TaskManagement.Models.Task { Id = 1, Title = "Fix bug #101", Description = "Fix login bug", IsCompleted = false, UserId = 1, CategoryId = 1, CreatedAt = DateTime.Now },
            //    new TaskManagement.Models.Task { Id = 2, Title = "Write blog post", Description = "Write about EF Core Seeding", IsCompleted = true, UserId = 2, CategoryId = 2, CreatedAt = DateTime.Now }
            //);
        }
    }
}
