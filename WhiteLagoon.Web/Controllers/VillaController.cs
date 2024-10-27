using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                if(command.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(command.Image.FileName);
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

                    using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);

                    command.Image.CopyTo(fileStream);

                    command.ImageUrl = @"\images\VillaImage\" + fileName;

                }
                else
                {
                    command.ImageUrl = "https://placehold.co/600x400";
                }

                _unitOfWork.Villa.Add(command);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be created";

            return View();
        }

        public IActionResult Update(int villaId)
        {

            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId);

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

                if(obj.Image != null)
                {

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);

                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");


                    if (!string.IsNullOrWhiteSpace(obj.ImageUrl))
                    {

                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using var fileStrim = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
                    obj.Image.CopyTo(fileStrim);

                    obj.ImageUrl = @"\images\VillaImage\" + fileName;

                }
                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been update successfully";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa could not be update";
            return View();
        }


        public IActionResult Delete(int villaId)
        {

            Villa? obj = _unitOfWork.Villa.Get(x => x.Id == villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(x => x.Id == obj.Id);

            if (objFromDb is not null)
            {


                if (!string.IsNullOrWhiteSpace(objFromDb.ImageUrl))
                {

                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction(nameof(Index));

            }

            TempData["error"] = "The Villa could not be deleted";
            return View();
        }
    }
}

