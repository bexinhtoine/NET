namespace Project.Models.Admin
{
    public class XoaNguoiDungViewModel
    {
        public string? MaNguoiDung { get; set; } // ID người dùng
        public string? HoTen { get; set; } // Họ tên người dùng
        public string? SoDienThoai { get; set; } // Số điện thoại người dùng
        public decimal SoDu { get; set; } // Số dư tài khoản
        public bool CoDuLieuLienQuan { get; set; } // Có dữ liệu liên quan hay không
    }
}