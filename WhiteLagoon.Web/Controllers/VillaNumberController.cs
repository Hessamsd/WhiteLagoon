using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM vallidNumberVM = new()
            {
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
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
            bool roomNumberExists = _unitOfWork.VillaNumber.Any(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !roomNumberExists)
            {
                _unitOfWork.VillaNumber.Add(obj.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            if (roomNumberExists)
            {
                TempData["error"] = "The villa number already exists";
            }
            obj.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                VillaNumber = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == villaNumberId)
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
                _unitOfWork.VillaNumber.Update(villaNumberVM.VillaNumber);
                _unitOfWork.Save();
                TempData["success"] = "The Villa number has been updated suuccessfuly";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
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
                VillaList = _unitOfWork.Villa.GetAll().Select(x => new SelectListItem
                {

                    Text = x.Name,
                    Value = x.Id.ToString()

                }),

                VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId)
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
            VillaNumber? objFrom = _unitOfWork.VillaNumber
                .Get(x => x.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);

            if (ModelState.IsValid)
            {
                _unitOfWork.VillaNumber.Remove(objFrom);
                _unitOfWork.Save();
                TempData["success"] = "The villa number has been deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The villa number could not be deleted";

            return View();
        }

    }

}