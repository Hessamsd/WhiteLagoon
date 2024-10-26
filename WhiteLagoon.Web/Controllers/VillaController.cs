﻿using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WhiteLagoon.Web.Controllers
{

    public class VillaController : Controller
    {

        private readonly IVillaRepository _villaRepo;

        public VillaController(IVillaRepository villaRepo)
        {
            _villaRepo = villaRepo;
        }


        public IActionResult Index()
        {
            var villas = _villaRepo.GetAll();
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
                 _villaRepo.Add(command);
                _villaRepo.Save();
                TempData["success"] = "The villa has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "The Villa could not be created";

            return View();
        }

        public IActionResult Update(int villaId)
        {

            Villa? obj = _villaRepo.Get(x => x.Id == villaId);

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
                _villaRepo.Update(obj);
                _villaRepo.Save();
                TempData["success"] = "The villa has been update successfully";
                return RedirectToAction(nameof(Index));

            }
            TempData["error"] = "The Villa could not be update";
            return View();
        }


        public IActionResult Delete(int villaId)
        {

            Villa? obj = _villaRepo.Get(x => x.Id == villaId);

            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }

            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _villaRepo.Get(x => x.Id == obj.Id);

            if (objFromDb is not null)
            {

                _villaRepo.Remove(objFromDb);
                _villaRepo.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction(nameof(Index));

            }

            TempData["error"] = "The Villa could not be deleted";
            return View();
        }
    }
}

