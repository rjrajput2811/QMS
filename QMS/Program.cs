using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QMS.Core.DatabaseContext;
using QMS.Core.Repositories.AHPNoteReposotory;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.CAReportRepository;
using QMS.Core.Repositories.CertificateMasterRepository;
using QMS.Core.Repositories.ChangeNoteImplementationItemRepository;
using QMS.Core.Repositories.ChangeNoteItemsRepository;
using QMS.Core.Repositories.ChangeNoteRepository;
using QMS.Core.Repositories.ContiImproveRespository;
using QMS.Core.Repositories.COPQComplaintDumpRepository;
using QMS.Core.Repositories.CSATCommentRepository;
using QMS.Core.Repositories.CSATSummaryRepository;
using QMS.Core.Repositories.CSOTrackerRepository;
using QMS.Core.Repositories.DashBoardRepository;
using QMS.Core.Repositories.DashBordRepository;
using QMS.Core.Repositories.DNTrackerRepository;
using QMS.Core.Repositories.DocumentConfiRepository;
using QMS.Core.Repositories.ElectricalPerformanceRepo;
using QMS.Core.Repositories.ElectricalProtectionRepo;
using QMS.Core.Repositories.ElectricalProtectionRepository;
using QMS.Core.Repositories.FIFOTrackerRepository;
using QMS.Core.Repositories.ImprTrackerRepository;
using QMS.Core.Repositories.InternalTypeTestRepo;
using QMS.Core.Repositories.KaizenTrackerRepository;
using QMS.Core.Repositories.NPITrackerRepository;
using QMS.Core.Repositories.OpenPoRepository;
using QMS.Core.Repositories.PaymentTrackerRepository;
using QMS.Core.Repositories.PDIAuthSignRepository;
using QMS.Core.Repositories.PDITrackerRepository;
using QMS.Core.Repositories.ProductValidationRepo;
using QMS.Core.Repositories.RCAReportRepository;
using QMS.Core.Repositories.RMTCDetailsRepository;
using QMS.Core.Repositories.SixSigmaIndicesRepo;
using QMS.Core.Repositories.SixSigmaIndicesRepository;
using QMS.Core.Repositories.SPMReportRepository;
using QMS.Core.Repositories.ThirdPartyCertRepository;
using QMS.Core.Repositories.ThirdPartyInspectionRepository;
using QMS.Core.Repositories.ThirdPartyTestRepository;
using QMS.Core.Repositories.UserRolesRepository;
using QMS.Core.Repositories.UsersRepository;
using QMS.Core.Repositories.VendorRepository;
using QMS.Core.Services.ChangeNoteService;
using QMS.Core.Services.SystemLogs;
using System.Data;
using QMS.Core.Repositories.SPMMakeRepository;
using QMS.Core.Repositories.SPMBuyRepository;

var builder = WebApplication.CreateBuilder(args);// Configure database connection.
var connstring = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddDbContext<QMSDbContext>(Options => Options.UseSqlServer(connstring));

builder.Services.AddTransient<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// add custom services
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<IUserRolesRepository, UserRolesRepository>();
builder.Services.AddTransient<ISystemLogService, SystemLogService>();
builder.Services.AddTransient<IDashBoardRepository, DashBoardRepository>();
builder.Services.AddTransient<IVendorRepository, VendorRepository>();
builder.Services.AddTransient<ICertificateMasterRepository, CertificateMasterRepository>();
builder.Services.AddTransient<IThirdPartyInspectionRepository, ThirdPartyInspectionRepository>();
builder.Services.AddTransient<IThirdPartyCertRepository, ThirdPartyCertRepository>();
builder.Services.AddTransient<IBisProjectTracRepository, BisProjectTracRepository>();
builder.Services.AddTransient<INPITrackerRepository, NPITrackerRepository>();
builder.Services.AddTransient<IPDITrackerRepository, PDITrackerRepository>();
builder.Services.AddTransient<ICSOTrackerRepository, CSOTrackerRepository>();
builder.Services.AddTransient<IDNTrackerRepository, DNTrackerRepository>();
builder.Services.AddTransient<IImprTrackerRepository, ImprTrackerRepository>();
builder.Services.AddTransient<IKaizenTrackerRepository, KaizenTrackerRepository>();
builder.Services.AddTransient<ISPMReportRepository, SPMReportRepository>();
builder.Services.AddTransient<IRMTCDetailsRepository, RMTCDetailsRepository>();
builder.Services.AddTransient<IComplaintIndentDumpRepository, ComplaintIndentDumpRepository>();
builder.Services.AddTransient<IJobWorkTracRepository, JobWorkTracRepository>();
builder.Services.AddTransient<IRLTTracRepository, RLTTracRepository>();
builder.Services.AddTransient<IContractorRepository, ContractorRepository>();
builder.Services.AddTransient<IPaymentTracRepository, PaymentTracRepository>();
builder.Services.AddTransient<IOpenPoReposiotry, OpenPoReposiotry>();
builder.Services.AddTransient<IDocumentConfiRepository, DocumentConfiRepository>();
builder.Services.AddTransient<IThirdPartyTestRepository, ThirdPartyTestRepository>();
builder.Services.AddTransient<IFIFOTrackerRepository, FIFOTrackerRepository>();
builder.Services.AddTransient<IPDIAuthSignRepository, PDIAuthSignRepository>();
builder.Services.AddTransient<IContiImproveRespository, ContiImproveRespository>();
builder.Services.AddTransient<ICSATCommentRepository, CSATCommentRepository>();
builder.Services.AddTransient<IPhysicalCheckAndVisualInspectionRepository, PhysicalCheckAndVisualInspectionRepository>();
builder.Services.AddTransient<IElectricalPerformanceRepository, ElectricalPerformanceRepository>();
builder.Services.AddScoped<IElectricalProtectionRepository, ElectricalProtectionRepository>();
builder.Services.AddScoped<IInternalTypeTestRepository, InternalTypeTestRepository>();
builder.Services.AddScoped<IAHPNoteReposotory, AHPNoteReposotory>();
builder.Services.AddTransient<ICSATSummaryRepository, CSATSummaryRepository>();
builder.Services.AddTransient<ICAReportRepository, CAReportRepository>();
builder.Services.AddTransient<IRCAReportRepository, RCAReportRepository>();
builder.Services.AddScoped<ISixSigmaIndicesRepository, SixSigmaIndicesRepository>();
builder.Services.AddTransient<IChangeNoteRepository, ChangeNoteRepository>();
builder.Services.AddTransient<IChangeNoteItemsRepository, ChangeNoteItemsRepository>();
builder.Services.AddTransient<IChangeNoteImplementationItemRepository, ChangeNoteImplementationItemRepository>();
builder.Services.AddTransient<IChangeNoteService, ChangeNoteService>();
builder.Services.AddScoped<ISPMMakeRepository, SPMMakeRepository>();
builder.Services.AddScoped<ISPMBuyRepository, SPMBuyRepository>();


//builder.Services.AddScoped<IDbConnection>(db => new SqlConnection(connstring));

//

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Unspecified;
    options.LoginPath = "/Account/Login";
    options.SlidingExpiration = true;
});
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(
    options =>
    {
        options.LoginPath = new PathString("/Account/Login");
    });
builder.Services.AddAuthorization();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());
});
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100 MB
    // You can set even higher if your files are huge.
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CorsPolicy");
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();


