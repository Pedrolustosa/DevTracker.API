using DevTracker.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevTracker.API.Persistence.Repository
{
    public class PackageRepository : IPackageRepository
    {
        public readonly DevTrackerContext _context;
        public PackageRepository(DevTrackerContext context)
        {
            _context = context;
        }

        public List<Package> GetAll()
        {
            return _context.Packages.ToList();
        }

        public Package GetByCode(string code)
        {
            return _context.Packages.Include(p => p.Updates)
                                    .SingleOrDefault(p => p.Code == code);
        }

        public void Add(Package package)
        {
            _context.Packages.Add(package);
            _context.SaveChanges();
        }

        public void Update(Package package)
        {
            _context.Entry(package).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}