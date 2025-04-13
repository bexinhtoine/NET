using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project.Data;

public partial class ProjetcNetQuanLyMayTinhContext : DbContext
{
    public ProjetcNetQuanLyMayTinhContext()
    {
    }

    public ProjetcNetQuanLyMayTinhContext(DbContextOptions<ProjetcNetQuanLyMayTinhContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MayTinh> MayTinhs { get; set; }

    public virtual DbSet<NapTien> NapTiens { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<SuDungMay> SuDungMays { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-UPDFUOQ\\SQLEXPRESS;Initial Catalog=Projetc_NET_QuanLyMayTinh;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MayTinh>(entity =>
        {
            entity.HasKey(e => e.MaMay).HasName("PK__MayTinh__3A5BBB4100BF969F");

            entity.ToTable("MayTinh");

            entity.Property(e => e.MaMay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DonGia).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MoTa).HasColumnType("text");
            entity.Property(e => e.TenMay).HasMaxLength(50);
            entity.Property(e => e.ThoiGianTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ThoiGianXoa).HasColumnType("datetime");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("Rảnh");
        });

        modelBuilder.Entity<NapTien>(entity =>
        {
            entity.HasKey(e => e.MaNapTien).HasName("PK__NapTien__86747C7689299D6C");

            entity.ToTable("NapTien");

            entity.Property(e => e.MaNapTien)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PhuongThuc).HasMaxLength(20);
            entity.Property(e => e.SoTien).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ThoiGianNap)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.NapTiens)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__NapTien__MaNguoi__6C190EBB");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762EC812742");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC0486C2A5E").IsUnique();

            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.MatKhau)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NgayTaoTaiKhoan)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.NgayXoaTaiKhoan).HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .HasDefaultValue("Khách");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.SoDu)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("Hoạt động");
        });

        modelBuilder.Entity<SuDungMay>(entity =>
        {
            entity.HasKey(e => e.MaSuDung).HasName("PK__SuDungMa__73EF96E9D140A64C");

            entity.ToTable("SuDungMay");

            entity.Property(e => e.MaSuDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaMay)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MaNguoiDung)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ThoiGianBatDau).HasColumnType("datetime");
            entity.Property(e => e.ThoiGianKetThuc).HasColumnType("datetime");
            entity.Property(e => e.TongThoiGian).HasColumnType("decimal(4, 2)");
            entity.Property(e => e.TongTien).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MaMayNavigation).WithMany(p => p.SuDungMays)
                .HasForeignKey(d => d.MaMay)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SuDungMay__MaMay__6754599E");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.SuDungMays)
                .HasForeignKey(d => d.MaNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SuDungMay__MaNgu__68487DD7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
