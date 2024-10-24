using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {

        private readonly ApplicationDbContext _db;

        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa command)
        {
            if (command.Name == command.Description)
            {
                ModelState.AddModelError("name", "the description cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Villas.Add(command);
                _db.SaveChanges();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa could not be created";

            return View();
        }

        public IActionResult Update(int villaId)
        {

            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            //Villa? obj = _db.Villas.Find(villaId);
            //var villaList = _db.Villas.Where(x => x.Price > 50 && x.Occupancy > 0).ToList();

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            if (ModelState.IsValid && obj.Id > 0)
            {
                _db.Villas.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "The villa has been update successfully";
                return RedirectToAction("Index");

            }
            TempData["error"] = "The Villa could not be update";
            return View();
        }


        public IActionResult Delete(int villaId)
        {

            Villa? obj = _db.Villas.FirstOrDefault(x => x.Id == villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _db.Villas.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb is not null)
            {

                _db.Villas.Remove(objFromDb);
                _db.SaveChanges();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");

            }

            TempData["error"] = "The Villa could not be deleted";
            return View();
        }
    }
}

