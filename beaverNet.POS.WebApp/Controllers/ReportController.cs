using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using beaverNet.POS.WebApp.Data;
using beaverNet.POS.WebApp.Models.POS;
using Microsoft.AspNetCore.Authorization;

namespace beaverNet.POS.WebApp.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendor
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Purchase()
        {
            return View();
        }

        public IActionResult ProductSales()
        {
            return View();
        }

        public IActionResult TimeSales()
        {
            return View();
        }

        public IActionResult CustomerSales()
        {
            return View();
        }

    }
}
