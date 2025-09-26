using Microsoft.EntityFrameworkCore;
using QMS.Core.Models;

namespace QMS.Core.DatabaseContext
{
    public class QMSDbContext : DbContext
    {
        public QMSDbContext(DbContextOptions<QMSDbContext> options) : base(options) { }

        public DbSet<Users> User { get; set; }
        public DbSet<UserRoles> UserRole { get; set; }
        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<VendorResViewModel> Vendor_List { get; set; }
        public DbSet<ProductCodeDetailViewModel> ProductCode { get; set; }
        public DbSet<CertificateMaster> CertificateMaster { get; set; }
        public DbSet<ThirdPartyInspection> ThirdPartyInspections { get; set; }
        public DbSet<InspectionResult> InspectionResults { get; set; }
        public DbSet<CertificationDetail> CertificationDetails { get; set; }
        public DbSet<ThirdPartyCertificateMaster> ThirdPartyCertificateMasters { get; set; }
        public DbSet<ThirdPartyTestReport> ThirdPartyTestReports { get; set; }
        public DbSet<CertificationDetailViewModel> CertificationDetailViewModel { get; set; }
        public DbSet<ThirdPartyTestReportViewModel> ThirdPartyTestReportViewModels { get; set; }
        public DbSet<BisProject_Tracker> BisProject_Tracker { get; set; }
        public DbSet<NatProject_BIS> NatProject_BIS { get; set; }
        public DbSet<Payment_Tracker> PaymentTracker { get; set; }
        public DbSet<Lab_Payment> Lab_Payment { get; set; }
        public DbSet<NPITracker> NPITracker { get; set; }
        public DbSet<VenBISCertificateViewModel> venBISCertificateViewModels { get; set; }
        public DbSet<VenBISCertificate> VenBISCertificates { get; set; }
        public DbSet<CSOTracker> CSOTracker { get; set; }
        public DbSet<PDITracker> PDITracker { get; set; }
        public DbSet<DNTracker> DeviationNote { get; set; }
        public DbSet<ImprovementTracker> ImprovementTracker { get; set; }
        public DbSet<Kaizen_Tracker> Kaizen_Tracker { get; set; }
        public DbSet<SPMReport> SPMReports { get; set; }
        public DbSet<RMTCDetails> RMTCDetails { get; set; }
        public DbSet<ThirdPartyTesting> ThirdPartyTesting { get; set; }
        public DbSet<Purpose_TPT> PurposeTPT { get; set; }
        public DbSet<ProjectInit_TPT> ProjectInitTPT { get; set; }
        public DbSet<TestDet_TPT> TestDetTPT { get; set; }
        public DbSet<FIFOTracker> FIFOTracker { get; set; }



        //// ------- Service -------- ////
        public DbSet<ComplaintDump_Service> COPQComplaintDump { get; set; }
        public DbSet<FailedRecord_Log> FailedRecord_Log { get; set; }
        public DbSet<PendingPo_Service> PODetails { get; set; }
        public DbSet<IndentDump_Service> IndentDump { get; set; }
        public DbSet<Invoice_Service> InvoiceList { get; set; }
        public DbSet<PcChart_Service> PcChart { get; set; }
        public DbSet<Region_Service> Region { get; set; }
        public DbSet<JobWork_Tracking_Service> JobWorkTrac { get; set; }
        public DbSet<RLT_Tracking_Service> RLTTrac { get; set; }
        public DbSet<ContractorDetail_Service> ContractorDetails { get; set; }
        public DbSet<DocumentDetail> Doc_Detail { get; set; }

        //// ------- Service -------- ////

        //// ------- Supply Chain Managment -------- ////
        public DbSet<Open_Po> OpenPo { get; set; }
        public DbSet<Open_Po_Log> OpenPo_Log { get; set; }
        public DbSet<Opne_Po_DeliverySchedule> Opne_Po_Deliveries { get; set; }
        public DbSet<Sales_Order_SCM> Sales_Order { get; set; }
        public DbSet<PC_Calendar_SCM> PC_Calendar { get; set; }



        /////// ------- Supply Chain Managment -------- ////
        public class InspectionResult
        {
            public int InspectionID { get; set; }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mark InspectionResult as keyless
            modelBuilder.Entity<InspectionResult>().HasNoKey();
            modelBuilder.Entity<CertificationDetailViewModel>().HasNoKey(); // ← Add this
            // Seed roles
            modelBuilder.Entity<UserRoles>().HasData(
                new UserRoles { Id = 1, RoleName = "Admin" },
                new UserRoles { Id = 2, RoleName = "Manager" }
            );

            // Seed default admin user
            modelBuilder.Entity<Users>().HasData(
                new Users
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "sysadmin@gmail.com",
                    Username = "sysadmin@gmail.com",
                    Password = "12345",
                    RoleId = 1
                }
            );
        }
    }
}
