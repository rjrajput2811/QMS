using Microsoft.AspNetCore.Mvc;
using QMS.Core.Repositories.BisProjectTracRepository;
using QMS.Core.Repositories.FIFOTrackerRepository;
using QMS.Core.Services.SystemLogs;

namespace QMS.Controllers
{
    public class FIFOTracController : Controller
    {
        private readonly IFIFOTrackerRepository _fIFOTrackerRepository;
        private readonly ISystemLogService _systemLogService;

        public FIFOTracController(IFIFOTrackerRepository fIFOTrackerRepository, ISystemLogService systemLogService)
        {
            _fIFOTrackerRepository = fIFOTrackerRepository;
            _systemLogService = systemLogService;
        }

        public IActionResult FIFOTracker()
        {
            return View();
        }
    }
}
