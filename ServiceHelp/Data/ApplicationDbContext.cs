using ServiceHelp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceHelp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Prioritet> Prioritet { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<AttachmentIssue> AttachmentIssue { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Issue> Issue { get; set; }
        public DbSet<IssueCategory> IssueCategory { get; set; }
        public DbSet<KnowledgeBase> KnowledgeBase { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IssueCategory>(entity =>
             {
                 entity.HasKey(e => new { e.IdCategory, e.IdIssue })
                     .HasName("PRIMARY");

                 entity.HasOne(d => d.Issue)
                     .WithMany(p => p.IssueCategory)
                     .HasForeignKey(d => d.IdIssue)
                     .OnDelete(DeleteBehavior.ClientCascade);

                 entity.HasOne(d => d.Category)
                     .WithMany(p => p.IssueCategory)
                     .HasForeignKey(d => d.IdCategory)
                     .OnDelete(DeleteBehavior.ClientSetNull);
             });
        }
    }
}
