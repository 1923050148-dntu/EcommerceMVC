using Microsoft.AspNetCore.Mvc;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Data; // Thay bằng namespace DbContext của bạn
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ECommerceMVC.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly Hshop2023Context _db; // Thay bằng DbContext của bạn

        public KhachHangController(Hshop2023Context db) => _db = db;

        [HttpGet]
        public IActionResult DangKy() => View();

        [HttpPost]
        public IActionResult DangKy(DangKyVM model)
        {
            if (ModelState.IsValid)
            {
                // Lưu vào database (Lưu ý: Thực tế nên mã hóa mật khẩu)
                var kh = new KhachHang
                {
                    MaKh = model.MaKh,
                    MatKhau = model.MatKhau,
                    HoTen = model.HoTen,
                    Email = model.Email
                };
                _db.Add(kh); _db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DangNhap() => View();

        [HttpPost]
        public async Task<IActionResult> DangNhap(DangNhapVM model)
        {
            var kh = _db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
            if (kh != null && kh.MatKhau == model.Password)
            {
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, kh.HoTen),
                    new Claim("CustomerID", kh.MaKh)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Loi = "Sai thông tin đăng nhập";
            return View();
        }

        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}