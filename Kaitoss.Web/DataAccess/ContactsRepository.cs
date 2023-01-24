using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess.Repository
{
    public class ContactsRepository : Repository<Contact> , IContactRepository
    {
        private readonly KaitossProjectContext _db;

        public ContactsRepository(KaitossProjectContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Contact coverType)
        {
            _db.Contacts.Update(coverType);
        }
    }
}
