namespace Project.Models.Admin
{
    public class QuanLyNguoiDungViewModel
    {
        public string? MaNguoiDung { get; set; } // Mã người dùng
        public string? TenDangNhap { get; set; } // Tên đăng nhập
        public string? HoTen { get; set; } // Họ tên
        public string? SoDienThoai { get; set; } // Số điện thoại
        public string? MatKhau { get; set; } // Mật khẩu
        public string? Role { get; set; } // Vai trò (Admin/Khách)
    }
}