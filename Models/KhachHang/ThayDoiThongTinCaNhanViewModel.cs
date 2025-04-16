using System.ComponentModel.DataAnnotations;

namespace Project.Models.KhachHang
{
    public class ThayDoiThongTinCaNhanViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự.")]
        public string? HoTen { get; set; } 

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string? TenDangNhap { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? SoDienThoai { get; set; } 

        public string? MatKhauHienTai { get; set; } 
        public string? MatKhauMoi { get; set; } 
        public string? XacNhanMatKhauMoi { get; set; } 
    }
}