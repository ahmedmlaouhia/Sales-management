using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWebMVC.Models;
using SalesWebMVC.Services;

namespace SalesWebMVC.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;
        private readonly SellerService _sellerService;

        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create()
        {
            var sellers = await _sellerService.FindAllAsync();
            ViewBag.SellerId = new SelectList(sellers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateOnly,TimeOnly,Amount,Status,SellerId")] SalesRecord salesRecord)
        {
            if (ModelState.IsValid)
            {
                await _salesRecordService.InsertAsync(salesRecord);
                return RedirectToAction(nameof(Index));
            }

            return View(salesRecord);
        }


        public async Task<IActionResult> SimpleSearch(DateOnly? minDate, DateOnly? maxDate)
        {
            if(!minDate.HasValue)
            {
                minDate = new DateOnly(DateTime.Now.Year, 1, 1);
            }

            DateTime dateTime = DateTime.Now;

            if (!maxDate.HasValue)
            {
                maxDate = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }

        public async Task<IActionResult> GroupingSearch(DateOnly? minDate, DateOnly? maxDate)
        {
            if (!minDate.HasValue)
            {
                minDate = new DateOnly(DateTime.Now.Year, 1, 1);
            }

            DateTime dateTime = DateTime.Now;

            if (!maxDate.HasValue)
            {
                maxDate = new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            }

            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }
    }
}
