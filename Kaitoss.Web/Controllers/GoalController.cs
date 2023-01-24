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
    public class GoalController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public GoalController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            IEnumerable<Goal> objectGoalsList = _unitOfWork.goals.GetAll();
            return View(objectGoalsList);
        }
        public IActionResult Upsert(int? id)
        {
            GoalsVM goalVM = new()
            {
                Goal = new()
            };
            if (id == 0 || id == null)
            {
                return View(goalVM);

            }
            else
            {
                //Update Service
                goalVM.Goal = _unitOfWork.goals.GetFirstOrDeafult(u => u.Id == id);
                return View(goalVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(GoalsVM goalVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"Components\Images\Goals");
                    var extension = Path.GetExtension(file.FileName);
                    if (goalVM.Goal.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, goalVM.Goal.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    goalVM.Goal.ImageUrl = @"\Components\Images\Goals\" + fileName + extension;
                }
                if (goalVM.Goal.Id == 0)
                {
                    _unitOfWork.goals.Add(goalVM.Goal);
                    TempData["success"] = " Create Goal is successfully";
                }
                else
                {
                    _unitOfWork.goals.Update(goalVM.Goal);
                    TempData["success"] = " Update Goal is successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(goalVM);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var goalsId = _unitOfWork.goals.GetFirstOrDeafult(u => u.Id == id);
            if (goalsId == null)
            {
                return NotFound();
            }
            return View(goalsId);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var goals = _unitOfWork.goals.GetFirstOrDeafult(u => u.Id == id);
            if (goals == null)
            {
                return NotFound();
            }
            _unitOfWork.goals.Remove(goals);
            _unitOfWork.Save();
            TempData["success"] = "Delete Goal is successfully";
            return RedirectToAction("Index");
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var allGoals = _unitOfWork.goals.GetAll();
            return Json(new { data = allGoals });
        }

        [HttpDelete]
        public IActionResult DeleteItem(int? id)
        {
            var goal = _unitOfWork.goals.GetFirstOrDeafult(u => u.Id == id);
            if (goal == null)
            {
                return Json(new { success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, goal.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.goals.Remove(goal);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful !" });
        }
        #endregion
    }
}