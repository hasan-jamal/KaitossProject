using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.DataAccess.Repository;

namespace Kaitoss.Web.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KaitossProjectContext _db;
        public UnitOfWork(KaitossProjectContext db)
        {
            _db = db;
            contacts = new ContactsRepository(_db);
            goals = new GoalsRepository(_db);
            services = new ServicesRepository(_db);
            abouts = new AboutsRepository(_db);
            informations = new InformationRepository(_db);
            blogs = new BlogsRepository(_db);

        }
        public IContactRepository contacts { get; private set; }
        public IGoalRepository goals { get; private set; }
        public IServiceRepository services { get; private set; }
        public IAboutsRepository abouts { get; private set; }
        public IInformationRepository informations { get; private set; }
        public IBlogsRepository blogs { get; private set; }




        public void Save()
        {
          _db.SaveChanges();
        }
    }
}
