using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {

        private readonly ApplicationDbContext _db;

        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villaVumbers = _db.VillaNumbers.Include(a => a.Villa).ToList();
            return View(villaVumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM vallidNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()

                })
            };

            return View(vallidNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM obj)
        {
            //ModelState.Remove("Villa");
            bool roomNumberExists = _db.VillaNumbers.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _db.VillaNumbers.Add(obj.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been created successfully";
                return RedirectToAction("Index");
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa number already exists";
            }
            obj.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
            {

                Text = x.Name,
                Value = x.Id.ToString()

            });

            return View(obj);
        }


        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _db.VillaNumbers.FirstOrDefault(u => u.Villa_Number == villaNumberId)
            };
            if (villaNumberVM.VillaNumber == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "The Villa number has been updated suuccessfuly";
                return RedirectToAction("Index");
            }

            villaNumberVM.VillaList = _db.Villas.ToList().Select(x => new SelectListItem
            {

                Text = x.Name,
                Value = x.Id.ToString()

            });

            return View(villaNumberVM);
        }
        public IActionResult Delete(int villaNumberId)
        {

            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _db.Villas.ToList().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()

                }),

                VillaNumber = _db.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
            };

            if (villaNumberVM == null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM villaNumberVM)
        {
            VillaNumber? objFrom = _db.VillaNumbers
                .FirstOrDefault(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Remove(objFrom);
                _db.SaveChanges();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa number could not be deleted";

            return View();
        }

    }

}