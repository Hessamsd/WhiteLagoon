using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IVillaRepository Villa { get; private set; }
        public IAmenityRepository Amenity { get; private set; }
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IBookingRepository Booking { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villa = new VillaRepository(db);
            Booking = new BookingRepository(db);
            Amenity = new AmenityRepository(db);
            VillaNumber = new VillaNumberRepository(db);



        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
