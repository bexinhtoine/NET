namespace Project.Models.KhachHang
{
    public class LichSuNapTienViewModel
    {
        public string? MaNapTien { get; set; } // Mã giao dịch nạp tiền
        public decimal SoTien { get; set; } // Số tiền đã nạp
        public string? PhuongThuc { get; set; } // Phương thức nạp tiền (Chuyển khoản, Tiền mặt, ...)
        public DateTime ThoiGianNap { get; set; } // Thời gian nạp tiền
    }
}