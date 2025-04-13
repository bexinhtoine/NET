using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class NguoiDung
{
    public string MaNguoiDung { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Role { get; set; }

    public decimal? SoDu { get; set; }

    public DateTime? NgayTaoTaiKhoan { get; set; }

    public DateTime? NgayXoaTaiKhoan { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<NapTien> NapTiens { get; set; } = new List<NapTien>();

    public virtual ICollection<SuDungMay> SuDungMays { get; set; } = new List<SuDungMay>();
}
