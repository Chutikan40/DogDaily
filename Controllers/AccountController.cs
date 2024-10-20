using DogDaily.Models.AccountModel;
using DogDaily.Models.AuthModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DogDaily.Controllers
{
    public class AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
    {
        private readonly UserManager<IdentityUser> _userManager = userManager;
        private readonly SignInManager<IdentityUser> _signInManager = signInManager;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // ถ้าการลงทะเบียนสำเร็จ เปลี่ยนไปที่หน้า Login
                    return RedirectToAction("Login", "Account");
                }

                // แสดงข้อผิดพลาดถ้าลงทะเบียนไม่สำเร็จ
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // ถ้าการเข้าสู่ระบบสำเร็จ เปลี่ยนไปที่หน้า Home
                    return RedirectToAction("Index", "Home");
                }

                // แสดงข้อความแจ้งเตือนถ้าการเข้าสู่ระบบไม่สำเร็จ
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); // เปลี่ยนไปที่หน้า Home หลังออกจากระบบ
        }

    }
}
