using AutoMapper;
using Kaitoss.Web.Data;
using Kaitoss.Web.Models;
using Kaitoss.Web.Utlity;
using Kaitoss.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Kaitoss.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly KaitossProjectContext _context;
        private readonly IMapper _mapper;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            KaitossProjectContext context,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _mapper = mapper;
            _roleManager = roleManager;
        }

        public IActionResult AccessDenied() => View();

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Users(string? filter)
        {
            if (filter != null && await _roleManager.RoleExistsAsync(filter))
            {
                var usersRoles = await _userManager.GetUsersInRoleAsync(filter);
                return View(usersRoles.ToList());
            }
            return View(await _context.Users.ToListAsync());
        }

        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Update(string emaile)
        {
            var getUser = await _userManager.FindByEmailAsync(emaile);
            if (getUser == null)
                return RedirectToAction(nameof(Users));

            var userRoles = await _context.UserRoles.
                Where(x => x.UserId == getUser.Id).
                Select(x => x.RoleId).ToListAsync();
            var userRoleVM = new UserRolesViewModel
            {
                Email = getUser.Email,
                Name = getUser.Name!,
                UserRoles = userRoles,
                Roles = new SelectList(
                await _context.Roles.ToListAsync(), "Id", "Name",
                userRoles)
            };
            return View(userRoleVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Update(UserRolesViewModel userVM)
        {
            var user = await _userManager.FindByEmailAsync(userVM.Email);
            if (userVM.UserRoles?.Count != 0)
            {
                foreach (var role in userVM.UserRoles!)
                {
                    var roleModel = await _context.Roles.FirstOrDefaultAsync(x => x.Id == role);
                    if (roleModel == null) break;
                    if (!await _userManager.IsInRoleAsync(user, roleModel.Name))
                        await _userManager.AddToRoleAsync(user, roleModel.Name);

                }
            }


            return RedirectToAction(nameof(Users));
        }
        public IActionResult Index()
        {
            if (User.Identity!.IsAuthenticated) {

                TempData["Error"] = "Already logged in !!!";
                return RedirectToAction("Index", "Home");
            } 

            return View(new LoginVM());
        }

        //POST: Acount/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVM login)
        {
            if (!ModelState.IsValid) return View(login);
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null) { TempData["Error"] = "Wrong credentials. please try again!"; return View(login); }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, login.Password);
            if (passwordCheck)
            {
                var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Goal");
                }
            }

            TempData["Error"] = "Wrong credentials. please try again!";
            return View(login);

        }
        [HttpGet]
        [Authorize(Roles = SD.Role_Admin)]
        public IActionResult Register() => View(new RegisterVM());

        //POST: Acount/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null) { TempData["Error"] = "The Email Address Already Exist"; return View(registerVM); }
            var newuser = new ApplicationUser()
            {
                Name = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };
            var result = await _userManager.CreateAsync(newuser, registerVM.Password);
            if (!result.Succeeded)
                return View(registerVM);

            var rseultRole = await _userManager.AddToRoleAsync(newuser, SD.Role_Admin);
            if (!rseultRole.Succeeded)
            {
                TempData["Error"] = "Wrong credentials. please try again!";
                return View(registerVM);
            }

            TempData["success"] = "Success Register";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
    
        public IActionResult RegisterCustomer() => View(new RegisterCustomerVM());

        //POST: Acount/RegisterCustomer
        [HttpPost]
        [ValidateAntiForgeryToken]
 
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null) { TempData["Error"] = "The Email Address Already Exist"; return View(registerVM); }
            var newuser = _mapper.Map<ApplicationUser>(registerVM);
            var result = await _userManager.CreateAsync(newuser, registerVM.Password);
            if (!result.Succeeded)
                return View(registerVM);

            var rseultRole = await _userManager.AddToRoleAsync(newuser, SD.Role_Admin);
            if (!rseultRole.Succeeded)
                return View(registerVM);
            return RedirectToAction("Index", "Goal");
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
