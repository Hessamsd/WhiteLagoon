using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

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
            if(command.Name == command.Description)
            {
                ModelState.AddModelError("name", "the description cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Villas.Add(command);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
