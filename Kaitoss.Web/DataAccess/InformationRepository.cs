using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.Repository
{
    public class InformationRepository : Repository<Information> , IInformationRepository
    {
        private readonly KaitossProjectContext _db;

        public InformationRepository(KaitossProjectContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Information obj)
        {
            var informationId = _db.Informations.FirstOrDefault(u => u.Id == obj.Id);
            if (informationId != null)
            {
                informationId.Id = obj.Id;
                informationId.Email = obj.Email;
                informationId.Phone = obj.Phone;
                informationId.Detailedaddress = obj.Detailedaddress;
                informationId.Country = obj.Country;
                informationId.Schedule = obj.Schedule;
                informationId.OfficeTime = obj.OfficeTime;

                if (obj.ImageUrl != null)
                {
                    informationId.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
