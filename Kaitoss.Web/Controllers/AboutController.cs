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
    public class AboutController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AboutController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<About> objectAboutsList = _unitOfWork.abouts.GetAll();

            return View(objectAboutsList);
        }
        public IActionResult Upsert(int? id)
        {
            AboutVM aboutVM = new()
            {
                About = new()
            };
            if (id == 0 || id == null)
            {
                return View(aboutVM);

            }
            else
            {
                //Update Service
                aboutVM.About = _unitOfWork.abouts.GetFirstOrDeafult(u => u.Id == id);
                return View(aboutVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(AboutVM aboutVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Components\Images\Abouts");
                    var extension = Path.GetExtension(file.FileName);
                
                    if (aboutVM.About.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, aboutVM.About.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                  
                    aboutVM.About.ImageUrl = @"\Components\Images\Abouts\" + fileName + extension;

                }
                if (aboutVM.About.Id == 0)
                {
                    _unitOfWork.abouts.Add(aboutVM.About);
                    TempData["success"] = " Create About Data is successfully";
                }
                else
                {
                    _unitOfWork.abouts.Update(aboutVM.About);
                    TempData["success"] = " Update About Data is successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(aboutVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var aboutsId = _unitOfWork.abouts.GetFirstOrDeafult(u => u.Id == id);
            if (aboutsId == null)
            {
                return NotFound();
            }
            return View(aboutsId);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var abouts = _unitOfWork.abouts.GetFirstOrDeafult(u => u.Id == id);
            if (abouts == null)
            {
                return NotFound();
            }
            _unitOfWork.abouts.Remove(abouts);
            _unitOfWork.Save();
            TempData["success"] = "Delete About is successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allAbouts= _unitOfWork.abouts.GetAll();
            return Json(new { data = allAbouts });
        }

        [HttpDelete]
        public IActionResult DeleteItem(int? id)
        {
            var about = _unitOfWork.abouts.GetFirstOrDeafult(u => u.Id == id);
            if (about == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, about.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
         
            _unitOfWork.abouts.Remove(about);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful !" });
        }
        #endregion
    }
}