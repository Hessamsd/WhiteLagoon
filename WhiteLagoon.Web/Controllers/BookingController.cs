﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Web.Controllers
{
    public class BookingController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [Authorize]
        public IActionResult FinalizeBooking(int villaId, DateOnly CheckIndate, int nights)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ApplicationUser user = _unitOfWork.User.Get(x => x.Id == userId);

            Booking booking = new()
            {
                VillaId = villaId,
                Villa = _unitOfWork.Villa.Get(x => x.Id == villaId, includeProperties: "VillaAmenity"),
                CheckInDate = CheckIndate,
                Nights = nights,
                CheckOutDate = CheckIndate.AddDays(nights),
                UserId = userId,
                Phone = user.PhoneNumber,
                Email = user.Email,
                Name = user.Name

            };


            booking.TotalCost = booking.Villa.Price * nights;
            return View(booking);


        }

    }
}
