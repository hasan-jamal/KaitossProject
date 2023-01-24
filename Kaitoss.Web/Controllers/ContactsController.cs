using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kaitoss.Web.DataAccess.IRepository;
using Kaitoss.Web.Models;
using Kaitoss.Web.Utlity;
using Kaitoss.Web.ViewModels;
using AutoMapper;

namespace Kaitoss.Web.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class ContactsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ContactsController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            IEnumerable<Contact> objectcontactsList = _unitOfWork.contacts.GetAll();
            return View(objectcontactsList);
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.contacts.Add(contact);
                _unitOfWork.Save();
                TempData["success"] = "Add Information Contact is successfully";
                return RedirectToAction("Index","Home");
            }
            TempData["error"] = "Add Information Contact is Failed !!";
            return View();
        }
   
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == 0 && id == null)
            {
                return NotFound();
            }
            var contactsId = _unitOfWork.contacts.GetFirstOrDeafult(u => u.Id == id);
            if (contactsId == null)
            {
                return NotFound();
            }
            return View(contactsId);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var contacts = _unitOfWork.contacts.GetFirstOrDeafult(u => u.Id == id);
            if (contacts == null)
            {
                return NotFound();
            }
            _unitOfWork.contacts.Remove(contacts);
            _unitOfWork.Save();
            TempData["success"] = "Delete contact is successfully";
            return RedirectToAction("Index");
        }
    }
}
