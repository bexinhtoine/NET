namespace Project.Models.Admin
{
    public class QuanLyMayTinhViewModel
    {
        public string? MaMay { get; set; } // Mã máy
        public string? TenMay { get; set; } // Tên máy
        public string? TrangThai { get; set; } // Trạng thái (Đang sử dụng, Sẵn sàng, Bảo trì)
        public double DonGia { get; set; } // Đơn giá
        public string? MoTa { get; set; } // Mô tả
    }
}