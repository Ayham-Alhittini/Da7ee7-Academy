﻿using Da7ee7_Academy.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Student_Course> Students_Courses { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionItem> SectionItems { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<SalePoint> SalePoints { get; set; }
        public DbSet<AppFile> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Student_Course>()
                .HasKey(s => new {s.StudentId, s.CourseId});
        }
    }
}