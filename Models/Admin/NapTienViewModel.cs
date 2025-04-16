using System.ComponentModel.DataAnnotations;

namespace Project.Models.Admin
{
    public class  NapTienViewModel
    {
        public string? MaNguoiDung { get; set; } // Mã người dùng
        public string? HoTen { get; set; } // Họ tên người dùng
        public string? SoDienThoai { get; set; } // Số điện thoại người dùng

        [Required(ErrorMessage = "Số tiền là bắt buộc.")]
        [Range(10000, 10000000, ErrorMessage = "Số tiền nạp phải nằm trong khoảng từ 10,000 VND đến 10,000,000 VND.")]
        public decimal SoTien { get; set; } // Số tiền cần nạp
    }
}