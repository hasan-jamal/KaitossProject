using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IGoalRepository : IRepository<Goal>
    {
        void Update(Goal goal);
    }
}
