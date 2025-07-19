﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace OnlineEducationPlatform.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        DbSet<Class> Classes { get; set; }
        DbSet<Subject> Subjects { get; set; }
        DbSet<ClassSubject> ClassSubjects { get; set; }
        DbSet<Assignment> Assignments { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<Exam> Exams { get; set; }
        DbSet<Enrollment> Enrollments { get; set; }
        DbSet<ExamSubmission> ExamSubmissions { get; set; }
        DbSet<AssignmentSubmission> AssignmentSubmission { get; set; }
        DbSet<Book> Books { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ClassSubject>().HasKey(cs => new { cs.ClassId, cs.SubjectId });
            modelBuilder.Entity<AssignmentSubmission>().HasKey(asb => new { asb.AssignmentId, asb.StudentId });
            // Class Relationships
            modelBuilder.Entity<Class>(entity =>
            {
                // Class -> Teacher (Many-to-One)
                entity.HasOne(c => c.Teacher)
                      .WithMany(t => t.Classes)
                      .HasForeignKey(c => c.TeacherId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Class -> Exams (One-to-Many)
                entity.HasMany(c => c.Exams)
                      .WithOne(e => e.Class)
                      .HasForeignKey(e => e.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Class -> ClassSubject (One-to-Many)
                entity.HasMany(c => c.Subjects)
                      .WithOne(e => e.Class)
                      .HasForeignKey(e => e.ClassId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Assignments)
                      .WithOne(a => a.Class)
                      .HasForeignKey(a => a.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                // Class -> ClassSubject (One-to-Many)
                entity.HasMany(c => c.Classes)
                      .WithOne(e => e.Subject)
                      .HasForeignKey(e => e.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Class -> Assignment (One-to-Many)
                entity.HasMany(c => c.Assignments)
                      .WithOne(a => a.Subject)
                      .HasForeignKey(a => a.SubjectId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Exam Relationships
            modelBuilder.Entity<Exam>(entity =>
            {
                // Exam -> Questions (One-to-Many)
                entity.HasMany(e => e.Questions)
                      .WithOne(q => q.Exam)
                      .HasForeignKey(q => q.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Exam -> Submissions (One-to-Many)
                entity.HasMany(e => e.Submissions)
                      .WithOne(s => s.Exam)
                      .HasForeignKey(s => s.ExamId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Question Configuration
            modelBuilder.Entity<Question>(entity =>
            {
                // JSON Column Configuration
                entity.Property(q => q.Options)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions()));
            });

            // Enrollment (Join Table)
            modelBuilder.Entity<Enrollment>(entity =>
            {
                // Composite PK
                entity.HasKey(e => new { e.ClassId, e.StudentId });

                // Enrollment -> Class (Many-to-One)
                entity.HasOne(e => e.Class)
                      .WithMany(c => c.Enrollments)
                      .HasForeignKey(e => e.ClassId);

                // Enrollment -> Student (Many-to-One)
                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Enrollments)
                      .HasForeignKey(e => e.StudentId);
            });

            // Exam Submission
            modelBuilder.Entity<ExamSubmission>(entity =>
            {
                entity.HasKey(s => new { s.ExamId, s.StudentId });
                // JSON Answers Storage
                entity.Property(s => s.Answers)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => JsonSerializer.Deserialize<Dictionary<int, string>>(v, new JsonSerializerOptions()));

                // Submission -> Student (Many-to-One)
                entity.HasOne(s => s.Student)
                      .WithMany(s => s.ExamSubmissions)
                      .HasForeignKey(s => s.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            base.OnModelCreating(modelBuilder);

        }
    }
}
