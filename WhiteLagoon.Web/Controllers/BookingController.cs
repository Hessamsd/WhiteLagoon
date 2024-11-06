﻿using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Repository;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {

        private readonly UnitOfWork _unitOfWork;

        public BookingController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult FinalizeBooking(int villaId, DateOnly CheckIndate, int nights)
        {
            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(x => x.Id == villaId, includeProperties: "VillaAmenity"),
                CheckInDate = CheckIndate,
                Nights = nights,
                CheckOutDate = CheckIndate.AddDays(nights),

            };
            return View(booking);
        }
    }
}