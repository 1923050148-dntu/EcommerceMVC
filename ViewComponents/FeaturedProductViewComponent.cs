using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class FeaturedProductViewComponent : ViewComponent
    {
        private readonly Hshop2023Context db;
        public FeaturedProductViewComponent(Hshop2023Context context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.HangHoas
                .OrderBy(p => Guid.NewGuid()) // Lấy ngẫu nhiên cho nó mới mẻ
                .Select(p => new FeaturedProductVM
                {
                    MaHh = p.MaHh,
                    TenHh = p.TenHh,
                    Hinh = p.Hinh ?? "",
                    DonGia = p.DonGia ?? 0,
                    GiaGoc = (p.DonGia ?? 0) * 1.2, // Fake giá gốc cao hơn để thấy gạch đỏ
                    DiemDanhGia = 4 // Cho 4 sao
                }).Take(3).ToList(); // Lấy đúng 3 món như hình bạn gửi

            return View(data);
        }
    }
}