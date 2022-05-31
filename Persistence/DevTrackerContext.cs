using DevTracker.API.Entities;

namespace DevTracker.API.Persistence
{
    public class DevTrackerContext
    {
        public DevTrackerContext()
        {
            Packages = new List<Package>();
        }

        public List<Package> Packages { get; set; }

    }
}