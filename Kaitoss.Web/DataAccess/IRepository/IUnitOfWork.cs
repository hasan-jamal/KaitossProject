namespace Kaitoss.Web.DataAccess.IRepository
{
    public interface IUnitOfWork
    {
        IContactRepository contacts { get; }
        IGoalRepository goals { get; }
        IServiceRepository services { get; }
        IAboutsRepository abouts { get; }
        IInformationRepository informations { get; }
        IBlogsRepository blogs { get; }


        void Save();
    }
}
