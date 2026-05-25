using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class MenuThanhVienViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            // Logic kiểm tra đăng nhập có thể thêm ở đây (dùng Identity hoặc Session)
            // Hiện tại trả về view mặc định
            return View();
        }
    }
}