using ECommerceMVC.Data;
using ECommerceMVC.ViewModels;
using ECommerceMVC.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly Hshop2023Context db;

        public CartController(Hshop2023Context context)
        {
            db = context;
        }

        const string CART_KEY = "MYCART";

        // Lấy giỏ hàng từ Session - Dùng Helper để lấy List object
        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

        // 1. Trang danh sách giỏ hàng
        public IActionResult Index()
        {
            return View(Cart);
        }

        // 2. Thêm sản phẩm vào giỏ (Xử lý Ajax)
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if (item == null) // Nếu chưa có món này trong giỏ -> Thêm mới
            {
                var hh = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if (hh == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy hàng hóa!" });
                }

                item = new CartItem
                {
                    MaHh = hh.MaHh,
                    TenHh = hh.TenHh,
                    DonGia = hh.DonGia ?? 0,
                    Hinh = hh.Hinh ?? "",
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else // Nếu đã có -> Chỉ tăng số lượng
            {
                item.SoLuong += quantity;
            }

            // Lưu lại vào Session
            HttpContext.Session.Set(CART_KEY, gioHang);

            // Trả về dữ liệu JSON cho Ajax xử lý ở phía Client
            return Json(new
            {
                success = true,
                cartCount = gioHang.Sum(p => p.SoLuong)
            });
        }

        // 3. Xóa một món hàng khỏi giỏ
        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);

            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(CART_KEY, gioHang);
            }

            return RedirectToAction("Index");
        }

        // 4. Xóa sạch giỏ hàng (Dùng khi thanh toán xong hoặc khách muốn xóa hết)
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CART_KEY);
            return RedirectToAction("Index");
        }
        // Hàm cập nhật số lượng món hàng (Dùng cho Ajax)
        public IActionResult UpdateQuantity(int id, int quantity)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHh == id);
            if (item != null)
            {
                item.SoLuong = quantity;
                if (item.SoLuong <= 0) gioHang.Remove(item); // Nếu giảm về 0 thì xóa luôn
                HttpContext.Session.Set(CART_KEY, gioHang);
            }
            return Json(new
            {
                success = true,
                itemTotal = (item.SoLuong * item.DonGia).ToString("#,##0"),
                cartTotal = gioHang.Sum(p => p.SoLuong * p.DonGia).ToString("#,##0"),
                cartCount = gioHang.Sum(p => p.SoLuong)
            });
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            var cart = Cart; // Lấy giỏ hàng hiện tại
            if (cart.Count == 0)
            {
                return Redirect("/"); // Giỏ trống thì cho về trang chủ
            }
            return View(cart); // Trả về view để khách nhập thông tin
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Ở đây đúng ra mình sẽ lưu vào CSDL (Bảng HoaDon, ChiTietHoaDon)
                // 2. Nhưng để Nhật Anh thấy nó "hiện" ra gì đó, mình sẽ xóa giỏ và báo thành công

                HttpContext.Session.Remove("MYCART"); // Xóa giỏ hàng sau khi đặt

                return View("Success"); // Chuyển sang trang báo thành công
            }
            return View(Cart);
        }
    }
}