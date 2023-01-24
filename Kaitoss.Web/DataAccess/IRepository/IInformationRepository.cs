using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IInformationRepository : IRepository<Information>
    {
        void Update(Information information);
    }
}
