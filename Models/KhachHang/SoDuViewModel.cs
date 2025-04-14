using System.ComponentModel.DataAnnotations;

namespace Project.Models.KhachHang
{
    public class SoDuViewModel
    {
        public string? HoTen { get; set; }

        [Required(ErrorMessage = "Số tiền là bắt buộc.")]
        [Range(10000, 10000000, ErrorMessage = "Số tiền nạp phải nằm trong khoảng từ 10,000 VND đến 10,000,000 VND.")]
        public double SoTien { get; set; }

        public double SoDu { get; set; }
    }
}