using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Uppdate(Booking booking);
        void UpdateStatus(int bookingId , string bookingStatus, int VillaNumber);
        void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId);
    }
}
