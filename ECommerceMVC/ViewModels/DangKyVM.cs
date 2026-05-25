using System.ComponentModel.DataAnnotations;

namespace ECommerceMVC.ViewModels
{
    public class DangKyVM
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [Display(Name = "Tên đăng nhập")]
        public string MaKh { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string HoTen { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
    }
}