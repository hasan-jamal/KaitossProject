using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;
using Kaitoss.Web.Utlity;
using Kaitoss.Web.ViewModels;
using System.Data;

namespace Kaitoss.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BlogController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Blog> objectBlogssList = _unitOfWork.blogs.GetAll();

            return View(objectBlogssList);
        }
        public IActionResult Upsert(int? id)
        {
            BlogVM blogVM = new()
            {
                Blog = new()
            };
            if (id == 0 || id == null)
            {
                return View(blogVM);

            }
            else
            {
                //Update Service
                blogVM.Blog = _unitOfWork.blogs.GetFirstOrDeafult(u => u.Id == id);
                return View(blogVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BlogVM blogVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null )
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Components\Images\Blogs");
                    var extension = Path.GetExtension(file.FileName);
                
                    if (blogVM.Blog.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, blogVM.Blog.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    blogVM.Blog.ImageUrl = @"\Components\Images\Blogs\" + fileName + extension;

                }
                if (blogVM.Blog.Id == 0)
                {
                    _unitOfWork.blogs.Add(blogVM.Blog);
                    TempData["success"] = " Create Blog Data is successfully";
                }
                else
                {
                    _unitOfWork.blogs.Update(blogVM.Blog);
                    TempData["success"] = " Update Blog Data is successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(blogVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var blogsId = _unitOfWork.blogs.GetFirstOrDeafult(u => u.Id == id);
            if (blogsId == null)
            {
                return NotFound();
            }
            return View(blogsId);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var blogs = _unitOfWork.blogs.GetFirstOrDeafult(u => u.Id == id);
            if (blogs == null)
            {
                return NotFound();
            }
            _unitOfWork.blogs.Remove(blogs);
            _unitOfWork.Save();
            TempData["success"] = "Delete Blog is successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allBlogs= _unitOfWork.blogs.GetAll();
            return Json(new { data = allBlogs });
        }

        [HttpDelete]
        public IActionResult DeleteItem(int? id)
        {
            var blog = _unitOfWork.blogs.GetFirstOrDeafult(u => u.Id == id);
            if (blog == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, blog.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
         
            _unitOfWork.blogs.Remove(blog);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful !" });
        }
        #endregion
    }
}