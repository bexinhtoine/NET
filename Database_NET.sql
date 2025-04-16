CREATE DATABASE Projetc_NET_QuanLyMayTinh;
USE Projetc_NET_QuanLyMayTinh;

-- Bảng MayTinh
CREATE TABLE MayTinh (
    MaMay VARCHAR(10) PRIMARY KEY,          -- Mã định danh duy nhất cho máy
    TenMay NVARCHAR(50) NOT NULL,            -- Tên hoặc số hiệu của máy
    TrangThai NVARCHAR(30) DEFAULT N'Rảnh',   -- Trạng thái máy (Rảnh, Đang sử dụng, Bảo trì)
    DonGia DECIMAL(10, 2) NOT NULL,         -- Giá sử dụng máy theo giờ
    MoTa TEXT,                              -- Mô tả chi tiết về máy
    ThoiGianTao DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian tạo máy
    ThoiGianXoa DATETIME                    -- Thời gian xóa máy (nếu có)
);

-- Bảng NguoiDung
CREATE TABLE NguoiDung (
    MaNguoiDung VARCHAR(10) PRIMARY KEY,    -- Mã định danh duy nhất cho người dùng
    HoTen NVARCHAR(100) NOT NULL,            -- Họ và tên của người dùng
    TenDangNhap VARCHAR(30) UNIQUE NOT NULL, -- Tên đăng nhập (duy nhất)
    MatKhau VARCHAR(30) NOT NULL,          -- Mật khẩu (nên được mã hóa)
    SoDienThoai VARCHAR(10),                -- Số điện thoại của người dùng
    Role NVARCHAR(10) DEFAULT N'Khách',        -- Vai trò (Admin, User)
    SoDu DECIMAL(12, 2) DEFAULT 0,          -- Số tiền hiện có trong tài khoản
    NgayTaoTaiKhoan DATETIME DEFAULT CURRENT_TIMESTAMP, -- Ngày tạo tài khoản
    NgayXoaTaiKhoan DATETIME,               -- Ngày tài khoản bị xóa (nếu có)
    TrangThai NVARCHAR(30) DEFAULT N'Hoạt động' -- Trạng thái tài khoản (Hoạt động, Bị khóa)
);

-- Bảng SuDungMay
CREATE TABLE SuDungMay (
    MaSuDung VARCHAR(10) PRIMARY KEY,        -- Mã định danh duy nhất cho mỗi lần sử dụng
    MaMay VARCHAR(10) NOT NULL,              -- Mã máy tính (liên kết với bảng MayTinh)
    MaNguoiDung VARCHAR(10) NOT NULL,        -- Mã người dùng (liên kết với bảng NguoiDung)
    ThoiGianBatDau DATETIME NOT NULL,        -- Thời gian bắt đầu sử dụng máy
    ThoiGianKetThuc DATETIME,                -- Thời gian kết thúc sử dụng máy
    TongThoiGian DECIMAL(4, 2),                        -- Tổng thời gian sử dụng (phút)
    TongTien DECIMAL(10, 2),                 -- Tổng số tiền phải trả
    FOREIGN KEY (MaMay) REFERENCES MayTinh(MaMay),
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);

-- Bảng NapTien
CREATE TABLE NapTien (
    MaNapTien VARCHAR(10) PRIMARY KEY, -- Mã định danh duy nhất cho mỗi lần nạp tiền
    MaNguoiDung VARCHAR(10) NOT NULL,         -- Mã người dùng (liên kết với bảng NguoiDung)
    SoTien DECIMAL(10, 2) NOT NULL,           -- Số tiền được nạp vào tài khoản
    ThoiGianNap DATETIME DEFAULT CURRENT_TIMESTAMP, -- Thời gian nạp tiền
    PhuongThuc NVARCHAR(20) NOT NULL,          -- Phương thức nạp tiền (Tiền mặt, Chuyển khoản)
    FOREIGN KEY (MaNguoiDung) REFERENCES NguoiDung(MaNguoiDung)
);