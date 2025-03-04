using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TaskManagementApi.Models
{
    public class TaskManagementDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options) { }

        public DbSet<Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<TaskLabel> TaskLabels { get; set; }
        public DbSet<TaskAttachment> TaskAttachment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 2, Name = "User", NormalizedName = "USER" }
            );

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

            modelBuilder.Entity<TaskAttachment>()
                .ToTable("TaskAttachments")
                .HasKey(t => t.Id);

            modelBuilder.Entity<TaskAttachment>()
                .Property(t => t.UploadedAt)
                .HasDefaultValueSql("GETDATE()");


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

            modelBuilder.Entity<Task>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Task>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

           
            //TaskAttachment
            modelBuilder.Entity<TaskAttachment>()
               .HasOne(t => t.Task)
               .WithMany(u => u.TaskAttachments)
               .HasForeignKey(t => t.TaskId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.Task)
                .WithMany(t => t.TaskComments)
                .HasForeignKey(tc => tc.TaskId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Task thì xóa luôn Comment

            modelBuilder.Entity<TaskComment>()
                .HasOne(tc => tc.User)
                .WithMany(u => u.TaskComments)
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict);

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


        }

    }
}


