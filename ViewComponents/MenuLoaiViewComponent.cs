using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceMVC.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;

        public MenuLoaiViewComponent(Hshop2023Context context) => db = context;

        public IViewComponentResult Invoke()
        {
            // Lấy danh sách loại, đếm số lượng hàng hóa và lấy tên file hình ảnh
            var data = db.Loais.Select(loai => new MenuLoaiVM
            {
                MaLoai = loai.MaLoai,
                TenLoai = loai.TenLoai,
                SoLuong = loai.HangHoas.Count,
                Hinh = loai.Hinh // Dòng này cực kỳ quan trọng để hiện icon nè!
            }).OrderBy(p => p.TenLoai);

            return View(data);
        }
    }
}