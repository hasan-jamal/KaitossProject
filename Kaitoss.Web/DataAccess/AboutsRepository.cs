using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.Repository
{
    public class AboutsRepository : Repository<About> , IAboutsRepository
    {
        private readonly KaitossProjectContext _db;

        public AboutsRepository(KaitossProjectContext db) : base(db)
        {
            _db = db;
        }

        public void Update(About obj)
        {
            var aboutId = _db.Abouts.FirstOrDefault(u => u.Id == obj.Id);
            if (aboutId != null)
            {
                aboutId.Id = obj.Id;
                aboutId.Title = obj.Title;
                aboutId.WhoWeAre = obj.WhoWeAre;
                aboutId.Vision= obj.Vision;
                aboutId.History = obj.History;
  
                if (obj.ImageUrl != null)
                {
                    aboutId.ImageUrl = obj.ImageUrl;
                }

            }
        }
    }
}
