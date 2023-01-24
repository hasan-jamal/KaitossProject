using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IContactRepository :IRepository<Contact>
    {
        void Update(Contact contact);
    }
}
