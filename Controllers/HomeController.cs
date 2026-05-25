using System.Diagnostics;
using ECommerceMVC.Models;
using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Hshop2023Context db;

        public HomeController(Hshop2023Context context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            // Lấy danh sách hàng hóa đổ ra trang chủ
            var data = db.HangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                // Thêm 2 dòng này để đồng bộ với ViewModel đã nâng cấp
                GiaGoc = (p.DonGia ?? 0) * 1.2, // Fake giá gốc cao hơn 20%
                DiemDanhGia = 5,                // Cho 5 sao cho uy tín
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            }).Take(8).ToList();

            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}