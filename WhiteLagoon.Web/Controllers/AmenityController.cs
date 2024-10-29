using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    [Authorize(Roles =SD.Role_Admin)]
    public class AmenityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AmenityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var amenities = _unitOfWork.Amenity.GetAll(includeProperties: "Villa");
            return View(amenities);
        }
        public IActionResult Create()
        {
            AmenityVm amenityVm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()

                }),
            };
            return View(amenityVm);
        }

        [HttpPost]
        public IActionResult Create(AmenityVm obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Add(obj.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been created successfully";
                return RedirectToAction(nameof(Index));
            }

            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            });

            return View(obj);
        }

        public IActionResult Update(int amenityId)
        {
            AmenityVm amenityVm = new()
            {

                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()

                }),
                Amenity = _unitOfWork.Amenity.Get(X => X.Id == amenityId)
            };
            if (amenityVm.Amenity == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(amenityVm);
        }

        [HttpPost]
        public IActionResult Update(AmenityVm amenityVm)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Update(amenityVm.Amenity);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been updated successfully";
                return RedirectToAction(nameof(Index));
            }
            amenityVm.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()

            });
            return View(amenityVm);
        }

        public IActionResult Delete(int amenityId)
        {
            AmenityVm amenityVm = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem {

                    Text = x.Name,
                    Value = x.Id.ToString()

                }),
                Amenity = _unitOfWork.Amenity.Get(x => x.Id == amenityId)
            };

            if(amenityVm.Amenity == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(amenityVm);
        }

        [HttpPost]
        public IActionResult Delete(AmenityVm amenityVm)
        {
            Amenity? objFromDb = _unitOfWork.Amenity
                .Get(x => x.Id == amenityVm.Amenity.Id);
            if (ModelState.IsValid)
            {
                _unitOfWork.Amenity.Remove(objFromDb);
                _unitOfWork.Save();
                TempData["success"] = "The amenity has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The amenity could not be deleted";
            return View();

            
        }

    }
}
