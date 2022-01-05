namespace EPrescribing.Web.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class newRDLCBranch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Advices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AdviceName = c.String(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Brands",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    BrandName = c.String(nullable: false),
                    Location = c.String(),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ChiefComplaints",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Description = c.String(),
                    UpperRight = c.Boolean(nullable: false),
                    UpperLeft = c.Boolean(nullable: false),
                    BottomRight = c.Boolean(nullable: false),
                    BottomLeft = c.Boolean(nullable: false),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.Prescriptions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DoctorId = c.Int(nullable: false),
                    PatientId = c.Int(nullable: false),
                    PaymentId = c.Int(),
                    PresentIllnes = c.String(),
                    MedicalHistory = c.String(),
                    NextAppointmentSchedule = c.String(),
                    TreatmentCharge = c.Double(nullable: false),
                    NextAppointmentDate = c.DateTime(),
                    Status = c.String(),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: false)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .ForeignKey("dbo.Payments", t => t.PaymentId)
                .Index(t => t.DoctorId)
                .Index(t => t.PatientId)
                .Index(t => t.PaymentId);

            CreateTable(
                "dbo.DiagnosticTest",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PrescriptionId = c.Int(nullable: false),
                    DiagnosticId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Diagnostics", t => t.DiagnosticId, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId)
                .Index(t => t.DiagnosticId);

            CreateTable(
                "dbo.Diagnostics",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ParentId = c.Int(),
                    TestName = c.String(nullable: false, maxLength: 500),
                    Note = c.String(maxLength: 1000),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Diagnostics", t => t.ParentId)
                .Index(t => t.ParentId);

            CreateTable(
                "dbo.DifferentialDiagnosis",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DiseaseName = c.String(nullable: false),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.Doctors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AppUserID = c.String(maxLength: 128),
                    Name = c.String(),
                    Image = c.String(),
                    BMDCRegistrationNumber = c.String(),
                    DesignationId = c.Int(),
                    DentalSchoolId = c.Int(),
                    ClinicName = c.String(),
                    ClinicAddress = c.String(),
                    SubscriptionlId = c.Int(),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.AppUserID)
                .ForeignKey("dbo.DentalSchools", t => t.DentalSchoolId)
                .ForeignKey("dbo.Designations", t => t.DesignationId)
                .ForeignKey("dbo.Subscriptions", t => t.SubscriptionlId)
                .Index(t => t.AppUserID)
                .Index(t => t.DesignationId)
                .Index(t => t.DentalSchoolId)
                .Index(t => t.SubscriptionlId);

            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Otp = c.String(),
                    OtpKey = c.String(),
                    RoleName = c.String(),
                    EmailOtp = c.String(),
                    Email = c.String(maxLength: 256),
                    EmailConfirmed = c.Boolean(nullable: false),
                    PasswordHash = c.String(),
                    SecurityStamp = c.String(),
                    PhoneNumber = c.String(),
                    PhoneNumberConfirmed = c.Boolean(nullable: false),
                    TwoFactorEnabled = c.Boolean(nullable: false),
                    LockoutEndDateUtc = c.DateTime(),
                    LockoutEnabled = c.Boolean(nullable: false),
                    AccessFailedCount = c.Int(nullable: false),
                    UserName = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");

            CreateTable(
                "dbo.UserClaims",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(nullable: false, maxLength: 128),
                    ClaimType = c.String(),
                    ClaimValue = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UserLogins",
                c => new
                {
                    LoginProvider = c.String(nullable: false, maxLength: 128),
                    ProviderKey = c.String(nullable: false, maxLength: 128),
                    UserId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);

            CreateTable(
                "dbo.UserRoles",
                c => new
                {
                    UserId = c.String(nullable: false, maxLength: 128),
                    RoleId = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);

            CreateTable(
                "dbo.DentalSchools",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Code = c.String(),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Designations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Code = c.String(),
                    IsSubscriptionFree = c.Boolean(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Subscriptions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Description = c.String(),
                    Cost = c.Single(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MedicineHistories",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MedicineId = c.Int(nullable: false),
                    Dose = c.String(),
                    Duration = c.String(),
                    Note = c.String(),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicines", t => t.MedicineId, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.MedicineId)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.Medicines",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 500),
                    GenericId = c.Int(nullable: false),
                    BrandId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .ForeignKey("dbo.Generics", t => t.GenericId, cascadeDelete: true)
                .Index(t => t.GenericId)
                .Index(t => t.BrandId);

            CreateTable(
                "dbo.Generics",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    GenericName = c.String(nullable: false, maxLength: 500),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.OnExaminations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Status = c.String(),
                    Description = c.String(),
                    UpperRight = c.String(),
                    UpperLeft = c.String(),
                    BottomRight = c.String(),
                    BottomLeft = c.String(),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.Patients",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PatientID = c.String(maxLength: 14),
                    Name = c.String(nullable: false),
                    Gender = c.String(nullable: false),
                    Age = c.String(nullable: false),
                    MobileNo = c.String(nullable: false),
                    TretmentDate = c.DateTime(nullable: false),
                    TotalCharge = c.Double(nullable: false),
                    DoctorId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctors", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);

            CreateTable(
                "dbo.Payments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PatientId = c.Int(nullable: false),
                    PrescriptionId = c.Int(nullable: false),
                    PaidAmount = c.Double(nullable: false),
                    DueAmount = c.Double(nullable: false),
                    PaymentDate = c.DateTime(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Patients", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);

            CreateTable(
                "dbo.PrescribedAdvices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    AdviceName = c.String(nullable: false),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.PrescribedMedicine",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MedicineId = c.Int(nullable: false),
                    DailyDose = c.String(),
                    Dose = c.String(),
                    Notes = c.String(),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicines", t => t.MedicineId, cascadeDelete: true)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.MedicineId)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.TreatmentDones",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PathologyName = c.String(nullable: false),
                    UpperRight = c.String(),
                    UpperLeft = c.String(),
                    BottomRight = c.String(),
                    BottomLeft = c.String(),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.TreatmentPlans",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TreatmentName = c.String(nullable: false),
                    UpperRight = c.String(),
                    UpperLeft = c.String(),
                    BottomRight = c.String(),
                    BottomLeft = c.String(),
                    PrescriptionId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);

            CreateTable(
                "dbo.Companies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CompanyName = c.String(nullable: false),
                    CompanyCode = c.String(),
                    OperationStartDate = c.DateTime(),
                    Amount = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Departments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    CompanyId = c.Int(nullable: false),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);

            CreateTable(
                "dbo.Diseases",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 500),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LoginUsers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    UserName = c.String(),
                    MobileNo = c.String(),
                    Email = c.String(),
                    Otp = c.String(),
                    OtpKey = c.String(),
                    Password = c.String(),
                    RoleName = c.String(),
                    EmailOtp = c.String(),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Pathologies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 500),
                    Description = c.String(maxLength: 1000),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Roles",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128),
                    Name = c.String(nullable: false, maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");

            CreateTable(
                "dbo.Treatments",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 500),
                    CreatedDate = c.DateTime(nullable: false),
                    UpdatedDate = c.DateTime(),
                    IsActive = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Departments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.TreatmentPlans", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.TreatmentDones", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.PrescribedMedicine", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.PrescribedMedicine", "MedicineId", "dbo.Medicines");
            DropForeignKey("dbo.PrescribedAdvices", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "PaymentId", "dbo.Payments");
            DropForeignKey("dbo.Payments", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Prescriptions", "PatientId", "dbo.Patients");
            DropForeignKey("dbo.Patients", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.OnExaminations", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.MedicineHistories", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.MedicineHistories", "MedicineId", "dbo.Medicines");
            DropForeignKey("dbo.Medicines", "GenericId", "dbo.Generics");
            DropForeignKey("dbo.Medicines", "BrandId", "dbo.Brands");
            DropForeignKey("dbo.Prescriptions", "DoctorId", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "SubscriptionlId", "dbo.Subscriptions");
            DropForeignKey("dbo.Doctors", "DesignationId", "dbo.Designations");
            DropForeignKey("dbo.Doctors", "DentalSchoolId", "dbo.DentalSchools");
            DropForeignKey("dbo.Doctors", "AppUserID", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.DifferentialDiagnosis", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.DiagnosticTest", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.DiagnosticTest", "DiagnosticId", "dbo.Diagnostics");
            DropForeignKey("dbo.Diagnostics", "ParentId", "dbo.Diagnostics");
            DropForeignKey("dbo.ChiefComplaints", "PrescriptionId", "dbo.Prescriptions");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Departments", new[] { "CompanyId" });
            DropIndex("dbo.TreatmentPlans", new[] { "PrescriptionId" });
            DropIndex("dbo.TreatmentDones", new[] { "PrescriptionId" });
            DropIndex("dbo.PrescribedMedicine", new[] { "PrescriptionId" });
            DropIndex("dbo.PrescribedMedicine", new[] { "MedicineId" });
            DropIndex("dbo.PrescribedAdvices", new[] { "PrescriptionId" });
            DropIndex("dbo.Payments", new[] { "PatientId" });
            DropIndex("dbo.Patients", new[] { "DoctorId" });
            DropIndex("dbo.OnExaminations", new[] { "PrescriptionId" });
            DropIndex("dbo.Medicines", new[] { "BrandId" });
            DropIndex("dbo.Medicines", new[] { "GenericId" });
            DropIndex("dbo.MedicineHistories", new[] { "PrescriptionId" });
            DropIndex("dbo.MedicineHistories", new[] { "MedicineId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Doctors", new[] { "SubscriptionlId" });
            DropIndex("dbo.Doctors", new[] { "DentalSchoolId" });
            DropIndex("dbo.Doctors", new[] { "DesignationId" });
            DropIndex("dbo.Doctors", new[] { "AppUserID" });
            DropIndex("dbo.DifferentialDiagnosis", new[] { "PrescriptionId" });
            DropIndex("dbo.Diagnostics", new[] { "ParentId" });
            DropIndex("dbo.DiagnosticTest", new[] { "DiagnosticId" });
            DropIndex("dbo.DiagnosticTest", new[] { "PrescriptionId" });
            DropIndex("dbo.Prescriptions", new[] { "PaymentId" });
            DropIndex("dbo.Prescriptions", new[] { "PatientId" });
            DropIndex("dbo.Prescriptions", new[] { "DoctorId" });
            DropIndex("dbo.ChiefComplaints", new[] { "PrescriptionId" });
            DropTable("dbo.Treatments");
            DropTable("dbo.Roles");
            DropTable("dbo.Pathologies");
            DropTable("dbo.LoginUsers");
            DropTable("dbo.Diseases");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
            DropTable("dbo.TreatmentPlans");
            DropTable("dbo.TreatmentDones");
            DropTable("dbo.PrescribedMedicine");
            DropTable("dbo.PrescribedAdvices");
            DropTable("dbo.Payments");
            DropTable("dbo.Patients");
            DropTable("dbo.OnExaminations");
            DropTable("dbo.Generics");
            DropTable("dbo.Medicines");
            DropTable("dbo.MedicineHistories");
            DropTable("dbo.Subscriptions");
            DropTable("dbo.Designations");
            DropTable("dbo.DentalSchools");
            DropTable("dbo.UserRoles");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Doctors");
            DropTable("dbo.DifferentialDiagnosis");
            DropTable("dbo.Diagnostics");
            DropTable("dbo.DiagnosticTest");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.ChiefComplaints");
            DropTable("dbo.Brands");
            DropTable("dbo.Advices");
        }
    }
}
