using System.ComponentModel.DataAnnotations;

namespace Project.Models.Admin
{
    public class ChinhSuaNguoiDungViewModel
    {
        public String? MaNguoiDung { get; set; } // Mã người dùng

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string? TenDangNhap { get; set; } // Tên đăng nhập

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Mật khẩu không được vượt quá 50 ký tự.")]
        public string? MatKhau { get; set; } // Mật khẩu

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự.")]
        public string? HoTen { get; set; } // Họ tên

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
        public string? SoDienThoai { get; set; } // Số điện thoại

        [Required(ErrorMessage = "Vai trò là bắt buộc.")]
        public string? Role { get; set; } // Vai trò (Admin/Khách)
    }
}