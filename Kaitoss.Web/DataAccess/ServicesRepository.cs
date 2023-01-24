using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.Repository
{
    public class ServicesRepository : Repository<Service> , IServiceRepository
    {
        private readonly KaitossProjectContext _db;

        public ServicesRepository(KaitossProjectContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Service obj)
        {
            var serviceId = _db.Services.FirstOrDefault(u => u.Id == obj.Id);
            if (serviceId != null)
            {
                serviceId.Id = obj.Id;
                serviceId.Title = obj.Title;
                serviceId.Describe = obj.Describe;

                if (obj.ImageUrl != null)
                {
                    serviceId.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
