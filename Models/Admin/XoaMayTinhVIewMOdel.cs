namespace Project.Models.Admin
{
    public class XoaMayTinhViewModel
    {
        public string? MaMay { get; set; } // ID của máy tính
        public string? TenMay{ get; set; } // Tên máy tính
        public string? MoTa { get; set; } // Cấu hình máy tính
        public string? TrangThai { get; set; } // Trạng thái máy tính (Sẵn sàng, Đang sử dụng, Bảo trì, Đã xóa)
        public bool CoDuLieuLienQuan { get; set; } // Có dữ liệu liên quan hay không
    }
}