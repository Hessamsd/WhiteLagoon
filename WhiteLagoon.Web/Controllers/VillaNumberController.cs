using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

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


            VillaNumberVM vm = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };

            return View(vm);
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

