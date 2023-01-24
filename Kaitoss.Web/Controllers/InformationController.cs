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
    public class InformationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public InformationController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Information> objectInformationsList = _unitOfWork.informations.GetAll();
            return View(objectInformationsList);
        }
        public IActionResult Upsert(int? id)
        {
            InformationVM informationVM = new()
            {
                Information = new()
            };
            if (id == 0 || id == null)
            {
                return View(informationVM);

            }
            else
            {
                //Update Information
                informationVM.Information = _unitOfWork.informations.GetFirstOrDeafult(u => u.Id == id);
                return View(informationVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(InformationVM informationVM, IFormFile? file, IFormFile? file2)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null && file2 != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Components\Images\Informations");
                    var extension = Path.GetExtension(file.FileName);

                    if (informationVM.Information.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, informationVM.Information.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    informationVM.Information.ImageUrl = @"\Components\Images\Informations\" + fileName + extension;

                }
                if (informationVM.Information.Id == 0)
                {
                    _unitOfWork.informations.Add(informationVM.Information);
                    TempData["success"] = " Create Information Data is successfully";
                }
                else
                {
                    _unitOfWork.informations.Update(informationVM.Information);
                    TempData["success"] = " Update Information Data is successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(informationVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var informationId = _unitOfWork.informations.GetFirstOrDeafult(u => u.Id == id);
            if (informationId == null)
            {
                return NotFound();
            }
            return View(informationId);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var informations = _unitOfWork.informations.GetFirstOrDeafult(u => u.Id == id);
            if (informations == null)
            {
                return NotFound();
            }
            _unitOfWork.informations.Remove(informations);
            _unitOfWork.Save();
            TempData["success"] = "Delete Information is successfully";
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allInformations= _unitOfWork.informations.GetAll();
            return Json(new { data = allInformations });
        }

        [HttpDelete]
        public IActionResult DeleteItem(int? id)
        {
            var information = _unitOfWork.informations.GetFirstOrDeafult(u => u.Id == id);
            if (information == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            _unitOfWork.informations.Remove(information);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful !" });
        }
        #endregion
    }
}