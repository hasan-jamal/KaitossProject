using Kaitoss.Web.Data;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.DataAccess.Repository;
using Kaitoss.Web.Models;

namespace Kaitoss.Web.DataAccess
{
    public class BlogsRepository : Repository<Blog>, IBlogsRepository
    {
        private readonly KaitossProjectContext _db;

        public BlogsRepository(KaitossProjectContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Blog obj)
        {
            var blogId = _db.Blogs.FirstOrDefault(u => u.Id == obj.Id);
            if (blogId != null)
            {
                blogId.Id = obj.Id;
                blogId.Title = obj.Title;
                blogId.Description = obj.Description;
                blogId.Writer = obj.Writer;

                if (obj.ImageUrl != null)
                {
                    blogId.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
