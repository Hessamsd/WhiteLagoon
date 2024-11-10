namespace WhiteLagoon.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IAmenityRepository Amenity { get; }
        IBookingRepository Booking { get; }
        IApplicationUserRepository User { get; }
        IVillaNumberRepository VillaNumber { get; }
        void Save();
    }
}
