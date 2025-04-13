using System.ComponentModel.DataAnnotations;

namespace Project.Models.Admin
{
    public class ThemMayTinhViewModel
    {
        [Required(ErrorMessage = "Tên máy là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Tên máy không được vượt quá 100 ký tự.")]
        public string? TenMay { get; set; } // Tên máy

        [Required(ErrorMessage = "Đơn giá là bắt buộc.")]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải là số dương.")]
        public decimal? DonGia { get; set; } // Đơn giá sử dụng máy

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự.")]
        public string? MoTa { get; set; } // Mô tả chi tiết về máy
    }
}