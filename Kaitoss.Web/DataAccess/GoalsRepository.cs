using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.Repository
{
    public class GoalsRepository : Repository<Goal> , IGoalRepository
    {
        private readonly KaitossProjectContext _db;

        public GoalsRepository(KaitossProjectContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Goal obj)
        {
            var goalId = _db.Goals.FirstOrDefault(u => u.Id == obj.Id);
            if (goalId != null)
            {
                goalId.Id = obj.Id;
                goalId.Title = obj.Title;
                goalId.SubTitle = obj.SubTitle;
                goalId.LinkVideo = obj.LinkVideo;
 
                if (obj.ImageUrl != null)
                {
                    goalId.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
