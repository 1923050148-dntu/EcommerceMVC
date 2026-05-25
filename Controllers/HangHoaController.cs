using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ECommerceMVC.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly Hshop2023Context db;

        public HangHoaController(Hshop2023Context context) => db = context;

        // 1. Trang danh sách hàng hóa (Lọc theo loại)
        public IActionResult Index(int? loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View(result);
        }

        // 2. Tìm kiếm hàng hóa
        public IActionResult Search(string query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHh = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });

            return View("Index", result);
        }

        // 3. Chi tiết hàng hóa (Đây chính là Bài 5 nè Nhật Anh!)
        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
                .Include(p => p.MaLoaiNavigation) // Để lấy được tên loại từ bảng Loai
                .SingleOrDefault(p => p.MaHh == id);

            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return RedirectToAction("Index", "Home");
            }

            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHh = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? "",
                Hinh = data.Hinh ?? "",
                MoTaNgan = data.MoTaDonVi ?? "",
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                DiemDanhGia = 5
            };

            return View(result);
        }
    }
}