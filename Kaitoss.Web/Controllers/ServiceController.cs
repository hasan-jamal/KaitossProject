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
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ServiceController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            IEnumerable<Service> objectServicesList = _unitOfWork.services.GetAll();

            return View(objectServicesList);
        }
        public IActionResult Upsert(int? id)
        {
            ServiceVM serviceVM = new()
            {
                Service = new()
            };
            if (id == 0 || id == null)
            {
                return View(serviceVM);

            }
            else
            {
                //Update Service
                serviceVM.Service = _unitOfWork.services.GetFirstOrDeafult(u => u.Id == id);
                return View(serviceVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ServiceVM serviceVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Components\Images\Services");
                    var extension = Path.GetExtension(file.FileName);
                    if (serviceVM.Service.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, serviceVM.Service.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    serviceVM.Service.ImageUrl = @"\Components\Images\Services\" + fileName + extension;
                }
                if (serviceVM.Service.Id == 0)
                {
                    _unitOfWork.services.Add(serviceVM.Service);
                    TempData["success"] = " Create Service is successfully";
                }
                else
                {
                    _unitOfWork.services.Update(serviceVM.Service);
                    TempData["success"] = " Update Service is successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(serviceVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var servicesId = _unitOfWork.services.GetFirstOrDeafult(u => u.Id == id);
            if (servicesId == null)
            {
                return NotFound();
            }
            return View(servicesId);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var services = _unitOfWork.services.GetFirstOrDeafult(u => u.Id == id);
            if (services == null)
            {
                return NotFound();
            }
            _unitOfWork.services.Remove(services);
            _unitOfWork.Save();
            TempData["success"] = "Delete Service is successfully";
            return RedirectToAction("Index");
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allServices= _unitOfWork.services.GetAll();
            return Json(new { data = allServices });
        }

        [HttpDelete]
        public IActionResult DeleteItem(int? id)
        {
            var service = _unitOfWork.services.GetFirstOrDeafult(u => u.Id == id);
            if (service == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, service.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.services.Remove(service);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful !" });
        }
        #endregion
    }
}