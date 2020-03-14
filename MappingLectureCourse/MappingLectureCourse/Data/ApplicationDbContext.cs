using MappingLectureCourse.Models.ContentViewModel;
using MappingLectureCourse.Models.MappingViewModel;
using MappingLectureCourse.Models.UserViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MappingLectureCourse.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("StaffUser");
            builder.Entity<IdentityRole>().ToTable("StaffRole");
            builder.Entity<IdentityUserClaim<string>>().ToTable("StaffUserClaim");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("StaffRoleClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("StaffUserLogin");
            builder.Entity<IdentityUserRole<string>>().ToTable("StaffUserRole");
            builder.Entity<IdentityUserToken<string>>().ToTable("StaffUserToken");
        }

        public DbSet<Department> departments { get; set; }

        public DbSet<Designation> designations { get; set; }

        public DbSet<Level> levels { get; set; }

        public DbSet<Qualification> qualifications { get; set; }

        public DbSet<Semester> semesters { get; set; }

        public DbSet<Session> sessions { get; set; }
        
        public DbSet<Course> courses { get; set; }

        public DbSet<Lecture> lectures { get; set; }

        public DbSet<LectureCourse> lectureCourses { get; set; }

        public DbSet<LectureQualification> lectureQualifications { get; set; }

        public DbSet<LectureResearchArea> lectureResearchAreas { get; set; }

        public DbSet<ResearchArea> researchAreas { get; set; }

        public DbSet<ListLectureCourse> listLectureCourses { get; set; }
    }
}
