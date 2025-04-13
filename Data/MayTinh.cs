using System;
using System.Collections.Generic;

namespace Project.Data;

public partial class MayTinh
{
    public string MaMay { get; set; } = null!;

    public string TenMay { get; set; } = null!;

    public string? TrangThai { get; set; }

    public decimal DonGia { get; set; }

    public string? MoTa { get; set; }

    public DateTime? ThoiGianTao { get; set; }

    public DateTime? ThoiGianXoa { get; set; }

    public virtual ICollection<SuDungMay> SuDungMays { get; set; } = new List<SuDungMay>();
}
