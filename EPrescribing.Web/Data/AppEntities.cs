using EPrescribing.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EPrescribing.Web.Data
{
    public class AppEntities : IdentityDbContext<ApplicationUser>
    {
        public AppEntities() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<LoginUser> LoginUsers { get; set; }

        public DbSet<Designation> Designations { get; set; }
        public DbSet<DentalSchool> DentalSchools { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Generic> Generics { get; set; }
        public DbSet<Advice> Advice { get; set; }
        public DbSet<Diagnostic> Diagnostics { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<Pathology> Pathologies { get; set; }
        public DbSet<Treatment> Treatments { get; set; }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<ChiefComplaint> ChiefComplaints { get; set; }
        public DbSet<DiagnosticTest> DiagnosticTests { get; set; }
        public DbSet<DifferentialDiagnosis> DifferentialDiagnosis { get; set; }
        public DbSet<MedicineHistory> MedicineHistories { get; set; }
        public DbSet<OnExamination> OnExaminations { get; set; }
        public DbSet<PrescribedAdvice> PrescribedAdvices { get; set; }
        public DbSet<PrescribedMedicine> PrescribedMedicines { get; set; }
        public DbSet<TreatmentDone> TreatmentDones { get; set; }
        public DbSet<TreatmentPlan> TreatmentPlans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SubscriptionFees> SubscriptionFees { get; set; }
        public DbSet<AboutSection> AboutSections { get; set; }
        public DbSet<TreatmentTemplete> TreatmentTempletes { get; set; }
        public DbSet<WorkProcess> WorkProcesses { get; set; }
        public DbSet<ServiceMainSection> ServiceMainSections { get; set; }
        public DbSet<SingleServiceSection> SingleServiceSections { get; set; }
        public DbSet<TeamMainSection> TeamMainSections { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<ContactSection> ContactSections { get; set; }
        public DbSet<FooterContent> FooterContents { get; set; }


        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            //modelBuilder.Entity<ApplicationRole>().HasKey<string>(r => r.Id).ToTable("Roles");
            //modelBuilder.Entity<ApplicationUser>().HasMany<ApplicationUserRole>((ApplicationUser u) => u.UserRoles);
            //modelBuilder.Entity<ApplicationUserRole>().HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("UserRoles");

        }
        public static AppEntities Create()
        {
            return new AppEntities();
        }
    }
}