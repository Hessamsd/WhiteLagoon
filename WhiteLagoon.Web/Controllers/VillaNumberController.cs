using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {

        private ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaVumbers = _db.VillaNumbers.ToList();
            return View(villaVumbers);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> list = _db.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

           
            ViewBag.VillaList = list;

            return View();
        }

        [HttpPost]
        public IActionResult Create(VillaNumber command)
        {
            //ModelState.Remove("Villa");
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Add(command);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }

}

