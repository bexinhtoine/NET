using System;

namespace Project.Models.Admin
{
    public class ThongKeViewModel
    {
        // Thông tin chung
        public string? MaMay { get; set; } // Mã máy
        public string? TenMay { get; set; } // Tên máy
        public string? MaNguoiDung { get; set; } // Mã người dùng (nếu cần)
        public string? TenNguoiDung { get; set; } // Tên người dùng (nếu cần)
        public string? SoDienThoai { get; set; } // Số điện thoại người dùng (nếu cần)

        // Thống kê sử dụng
        public int SoLanSuDung { get; set; } // Số lần sử dụng
        public double TongGioSuDung { get; set; } // Tổng số giờ sử dụng
        public decimal TongDoanhThu { get; set; } // Tổng doanh thu

        // Chi tiết từng lần sử dụng
        public DateTime? ThoiGianBatDau { get; set; } // Thời gian bắt đầu sử dụng
        public DateTime? ThoiGianKetThuc { get; set; } // Thời gian kết thúc sử dụng
        public decimal? DoanhThuLanSuDung { get; set; } // Doanh thu của lần sử dụng

        // Thông tin bổ sung
        public string? TrangThai { get; set; } // Trạng thái máy (Sẵn sàng, Đang sử dụng, Bảo trì, Đã xóa)
        public string? MoTa { get; set; } // Mô tả thêm (nếu cần)
    }
}