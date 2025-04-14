namespace Project.Models.KhachHang
{
        public class KhachHangHomeViewModel
    {
        public string? HoTen { get; set; }
        public string? SoDienThoai { get; set; }
        public double SoDu { get; set; }
        public string? TrangThaiSuDung { get; set; }
        public string? MaMay { get; set; }
        public string? TenMayDangSuDung { get; set; }
        public double DonGia { get; set; }
        public DateTime? ThoiGianBatDau { get; set; }
        public TimeSpan? ThoiGianDangSuDung { get; set; }
        public double TongTienCanThanhToan { get; set; }
        public List<ChonMayDungViewModel> DanhSachMay { get; set; } = new List<ChonMayDungViewModel>(); // Danh sách máy sẵn sàng
    }
}