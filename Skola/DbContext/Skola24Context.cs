using Microsoft.EntityFrameworkCore;
using Skola.API.Models;

namespace Skola.API.Data
{
    public class Skola24Context : DbContext
    {
        public Skola24Context(DbContextOptions<Skola24Context> options) : base(options)
        {
            // Set command timeout globally
            Database.SetCommandTimeout(120); // 120 seconds (adjust as needed)
        }
              
        public DbSet<Student> Students { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<StudentResult> StudentResults { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            try
            {
                modelBuilder.Entity<StudentResult>().HasNoKey();
                modelBuilder.Entity<Student>().ToTable("Students");
                modelBuilder.Entity<School>().ToTable("Schools");

              
                modelBuilder.Entity<Student>()
                    .HasOne(s => s.School)
                    .WithMany(sch => sch.Students)
                    .HasForeignKey(s => s.SchoolAttendingID)
                    .OnDelete(DeleteBehavior.Restrict);


                // Define primary key for Attendance table
                modelBuilder.Entity<Attendance>()
                    .HasKey(a => a.AttendanceID);

                // Define foreign key relationships
                modelBuilder.Entity<Attendance>()
                    .HasOne(a => a.Student)
                    .WithMany()
                    .HasForeignKey(a => a.StudentID)
                    .OnDelete(DeleteBehavior.Restrict); 

                modelBuilder.Entity<Attendance>()
                            .HasOne(a => a.WorkingDays)
                            .WithMany()
                            .HasForeignKey(a => a.WorkingDayID)
                            .OnDelete(DeleteBehavior.Restrict); 

                // Define primary key for WorkingDay table
                modelBuilder.Entity<WorkingDays>()
                            .HasKey(w => w.WorkingDayID);

                // Define foreign key relationship
                modelBuilder.Entity<WorkingDays>()
                            .HasOne(w => w.School)
                            .WithMany()
                            .HasForeignKey(w => w.SchoolID)
                            .OnDelete(DeleteBehavior.Restrict); 

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}
