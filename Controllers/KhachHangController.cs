using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Required for [Authorize]
using Project.Data; // Namespace của DbContext
using System.Linq;
using System.Security.Claims; // Required for Claims
using Project.Models; 
using Project.Models.KhachHang;

namespace Project.Controllers
{
    [Authorize(Roles = "Khách")]
    public class KhachHangController : Controller
    {
        private readonly ProjetcNetQuanLyMayTinhContext _context;

        public KhachHangController(ProjetcNetQuanLyMayTinhContext context)
        {
            _context = context;
        }

        private List<ChonMayDungViewModel> LayDanhSachMaySanSang(string maNguoiDung, string search = "")
        {
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null) return new List<ChonMayDungViewModel>();
        
            var soDu = nguoiDung.SoDu ?? 0;
        
            return _context.MayTinhs
                .Where(m => m.TrangThai == "Sẵn sàng" &&
                            (string.IsNullOrEmpty(search) || 
                            m.MaMay.Contains(search) || 
                            m.TenMay.Contains(search)))
                .Select(m => new ChonMayDungViewModel
                {
                    MaMay = m.MaMay,
                    TenMay = m.TenMay,
                    DonGia = (double?)m.DonGia ?? 0,
                    ThoiGianSuDungToiDa = m.DonGia > 0 ? TimeSpan.FromHours((double)(soDu / m.DonGia)) : TimeSpan.Zero
                })
                .ToList();
        }


        // Action Home: Trang chủ dành cho khách hàng
        public IActionResult Home(string search)
        {
            // Lấy thông tin MaNguoiDung từ Claims
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;

            if (string.IsNullOrEmpty(maNguoiDung))
            {
                // Nếu không tìm thấy MaNguoiDung, chuyển hướng về trang đăng nhập
                return RedirectToAction("DangNhap", "Home");
            }

            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);

            if (nguoiDung == null)
            {
                // Nếu không tìm thấy người dùng, chuyển hướng về trang đăng nhập
                return RedirectToAction("DangNhap", "Home");
            }
            
            // Kiểm tra trạng thái sử dụng máy
            var suDungMay = _context.SuDungMays
                .FirstOrDefault(sdm => sdm.MaNguoiDung == maNguoiDung && sdm.ThoiGianKetThuc == null);

            var mayDangSuDung = suDungMay != null 
                ? _context.MayTinhs.FirstOrDefault(m => m.MaMay == suDungMay.MaMay) 
                : null;

            var viewModel = new KhachHangHomeViewModel
            {
                HoTen = nguoiDung.HoTen,
                SoDienThoai = nguoiDung.SoDienThoai,
                SoDu = (double)(nguoiDung.SoDu ?? 0),
                TrangThaiSuDung = suDungMay != null ? "Đang sử dụng" : "Chưa sử dụng",
                MaMay = suDungMay?.MaMay,
                TenMayDangSuDung = mayDangSuDung?.TenMay,
                DonGia = (double?)mayDangSuDung?.DonGia ?? 0,
                ThoiGianBatDau = suDungMay?.ThoiGianBatDau,
                ThoiGianDangSuDung = suDungMay?.ThoiGianBatDau != null 
                    ? (DateTime.Now - suDungMay.ThoiGianBatDau)
                    : (TimeSpan?)null,
                TongTienCanThanhToan = suDungMay != null
                    ? (mayDangSuDung != null 
                        ? (double)(DateTime.Now - suDungMay.ThoiGianBatDau).TotalHours * (double)mayDangSuDung.DonGia 
                        : 0)
                    : 0,
                DanhSachMay = LayDanhSachMaySanSang(maNguoiDung, search)
            };

            // Tự động dừng thuê máy nếu số dư không đủ
            if (suDungMay != null && viewModel.TongTienCanThanhToan >= (double)(nguoiDung.SoDu ?? 0))
            {
                suDungMay.ThoiGianKetThuc = DateTime.Now;
                suDungMay.TongThoiGian = (decimal?)((suDungMay.ThoiGianKetThuc - suDungMay.ThoiGianBatDau)?.TotalHours ?? 0);
                suDungMay.TongTien = mayDangSuDung?.DonGia != null 
                    ? suDungMay.TongThoiGian.Value * mayDangSuDung.DonGia
                    : 0;
                nguoiDung.SoDu -= suDungMay.TongTien;
                if (mayDangSuDung != null)
                {
                    mayDangSuDung.TrangThai = "Sẵn sàng";
                }
                _context.SaveChanges();
                viewModel.TrangThaiSuDung = "Chưa sử dụng";
                viewModel.TenMayDangSuDung = null;
                viewModel.ThoiGianBatDau = null;
                viewModel.TongTienCanThanhToan = 0;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ChonMayDung(string maMay)
        {
            // Lấy thông tin người dùng hiện tại
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
        
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            // Lấy thông tin người dùng
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            // Kiểm tra trạng thái máy
            var may = _context.MayTinhs.FirstOrDefault(m => m.MaMay == maMay);
            if (may == null || may.TrangThai != "Sẵn sàng")
            {
                TempData["Error"] = "Máy không khả dụng hoặc đang được sử dụng.";
                return RedirectToAction("Home");
            }

            // Lấy mã sử dụng máy cuối cùng từ cơ sở dữ liệu
            var maSuDungMayCuoi = _context.SuDungMays
                .OrderByDescending(nt => nt.MaSuDung)
                .Select(nt => nt.MaSuDung)
                .FirstOrDefault();

            // Tạo mã nạp tiền mới
            string maSuDungMayMoi;
            if (string.IsNullOrEmpty(maSuDungMayCuoi))
            {
                maSuDungMayMoi = "0000000001"; // Nếu chưa có mã nào, bắt đầu từ 0000000001
            }
            else
            {
                // Tăng mã cuối cùng lên 1
                long soThuTu = long.Parse(maSuDungMayCuoi) + 1;
                maSuDungMayMoi = soThuTu.ToString("D10"); // Định dạng thành chuỗi 10 chữ số
            }
        
            // Tạo bản ghi sử dụng máy
            var suDungMay = new SuDungMay
            {
                MaSuDung = maSuDungMayMoi,
                MaMay = maMay,
                MaNguoiDung = maNguoiDung,
                ThoiGianBatDau = DateTime.Now,
                ThoiGianKetThuc = null,
                TongTien = null
            };
        
            // Cập nhật trạng thái máy
            may.TrangThai = "Đang sử dụng";
        
            // Lưu vào cơ sở dữ liệu
            _context.SuDungMays.Add(suDungMay);
            _context.SaveChanges();
        
            TempData["Success"] = "Bạn đã bắt đầu sử dụng máy.";
            return RedirectToAction("Home");
        }
   
        [HttpGet]
        public JsonResult GetThongTinSuDung()
        {
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
        
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return Json(new { success = false, message = "Người dùng không hợp lệ." });
            }
        
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng." });
            }
        
            var suDungMay = _context.SuDungMays
                .Where(s => s.MaNguoiDung == maNguoiDung && s.ThoiGianKetThuc == null)
                .FirstOrDefault();
        
            if (suDungMay == null)
            {
                return Json(new { success = false, message = "Người dùng không sử dụng máy." });
            }
        
            var thoiGianBatDau = suDungMay.ThoiGianBatDau;
            var thoiGianSuDung = DateTime.Now - suDungMay.ThoiGianBatDau;
        
            // Tính toán thời gian sử dụng theo định dạng hh:mm:ss
            var hours = thoiGianSuDung.Hours;
            var minutes = thoiGianSuDung.Minutes;
            var seconds = thoiGianSuDung.Seconds;
            var formattedThoiGianSuDung = $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        
            var mayTinh = _context.MayTinhs.FirstOrDefault(m => m.MaMay == suDungMay.MaMay);
            var chiPhi = ((decimal)thoiGianSuDung.TotalHours) * (mayTinh?.DonGia ?? 0);
            var soDuHienTai = (nguoiDung.SoDu ?? 0) - (decimal)chiPhi;
        
            return Json(new
            {
                success = true,
                thoiGianBatDau = thoiGianBatDau.ToString("HH:mm:ss dd/MM/yyyy"),
                thoiGianSuDung = formattedThoiGianSuDung, // Trả về định dạng hh:mm:ss
                chiPhi = Math.Floor(chiPhi),
                soDuHienTai = Math.Floor(soDuHienTai)
            });
        }

        [HttpPost]
        public IActionResult KetThucSuDung(string maMay)
        {
            if (string.IsNullOrEmpty(maMay))
            {
                TempData["Error"] = "Mã máy không hợp lệ.";
                return RedirectToAction("Home");
            }
        
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["Error"] = "Người dùng không hợp lệ.";
                return RedirectToAction("Home");
            }
        
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Tìm thông tin thuê máy
                    var suDungMay = _context.SuDungMays
                        .FirstOrDefault(sdm => sdm.MaMay == maMay && sdm.ThoiGianKetThuc == null);
        
                    if (suDungMay == null)
                    {
                        TempData["Error"] = "Không tìm thấy thông tin thuê máy.";
                        return RedirectToAction("Home");
                    }
        
                    // Tìm thông tin máy
                    var mayDangThue = _context.MayTinhs.FirstOrDefault(m => m.MaMay == maMay);
                    if (mayDangThue == null)
                    {
                        TempData["Error"] = "Không tìm thấy thông tin máy.";
                        return RedirectToAction("Home");
                    }
        
                    // Cập nhật trạng thái máy
                    mayDangThue.TrangThai = "Sẵn sàng";
        
                    // Cập nhật thời gian kết thúc và tính toán chi phí
                    suDungMay.ThoiGianKetThuc = DateTime.Now;
                    suDungMay.TongThoiGian = (decimal?)((suDungMay.ThoiGianKetThuc - suDungMay.ThoiGianBatDau)?.TotalHours ?? 0);
                    suDungMay.TongTien = suDungMay.TongThoiGian.Value * mayDangThue.DonGia;
        
                    // Cập nhật số dư người dùng
                    var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == suDungMay.MaNguoiDung);
                    if (nguoiDung == null)
                    {
                        TempData["Error"] = "Không tìm thấy thông tin người dùng.";
                        return RedirectToAction("Home");
                    }
        
                    if (suDungMay.TongTien.HasValue)
                    {
                        nguoiDung.SoDu -= suDungMay.TongTien.Value;
                        if (nguoiDung.SoDu < 0)
                        {
                            TempData["Error"] = "Số dư không đủ để thanh toán.";
                            return RedirectToAction("Home");
                        }
                    }
        
                    // Lưu thay đổi vào cơ sở dữ liệu
                    _context.SaveChanges();
        
                    // Commit transaction
                    transaction.Commit();
        
                    TempData["Success"] = "Thuê máy đã kết thúc.";
                    return RedirectToAction("Home");
                }
                catch (Exception ex)
                {
                    // Rollback transaction nếu có lỗi
                    transaction.Rollback();
                    TempData["Error"] = "Đã xảy ra lỗi khi kết thúc thuê máy.";
                    return RedirectToAction("Home");
                }
            }
        }        [HttpGet]
        
        public IActionResult SoDu()
        {
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
    
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("DangNhap", "Home");
            }
    
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
    
            var viewModel = new SoDuViewModel
            {
                HoTen = nguoiDung.HoTen,
                SoDu = (double)(nguoiDung.SoDu ?? 0)
            };
    
            return View(viewModel);
        }
    
        // Xử lý nạp tiền
        [HttpPost]
        public IActionResult NapTien(SoDuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SoDu", model); // Trả về View với lỗi
            }
        
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
        
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }

            // Lấy mã nạp tiền cuối cùng từ cơ sở dữ liệu
            var maNapTienCuoi = _context.NapTiens
                .OrderByDescending(nt => nt.MaNapTien)
                .Select(nt => nt.MaNapTien)
                .FirstOrDefault();

            // Tạo mã nạp tiền mới
            string maNapTienMoi;
            if (string.IsNullOrEmpty(maNapTienCuoi))
            {
                maNapTienMoi = "0000000001"; // Nếu chưa có mã nào, bắt đầu từ 0000000001
            }
            else
            {
                // Tăng mã cuối cùng lên 1
                long soThuTu = long.Parse(maNapTienCuoi) + 1;
                maNapTienMoi = soThuTu.ToString("D10"); // Định dạng thành chuỗi 10 chữ số
            }
            // Ghi log giao dịch nạp tiền
            var lichSuNapTien = new NapTien
            {
                MaNapTien = maNapTienMoi,
                MaNguoiDung = maNguoiDung,
                SoTien = (decimal)model.SoTien,
                ThoiGianNap = DateTime.Now,
                PhuongThuc = "Chuyển khoản",
            };
            _context.NapTiens.Add(lichSuNapTien);
            _context.SaveChanges();
        
            // Cập nhật số dư
            nguoiDung.SoDu = (nguoiDung.SoDu ?? 0) + (decimal)model.SoTien;
            _context.SaveChanges();
        
            TempData["Success"] = $"Bạn đã nạp thành công {model.SoTien.ToString("C")} vào tài khoản.";
            return RedirectToAction("SoDu");
        }
    
        [HttpGet]
        public IActionResult LichSu()
        {
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
        
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            var lichSuSuDung = _context.SuDungMays
                .Where(s => s.MaNguoiDung == maNguoiDung)
                .OrderByDescending(s => s.ThoiGianBatDau)
                .Select(s => new LichSuViewModel
                {
                    MaMay = s.MaMay,
                    TenMay = _context.MayTinhs.Where(m => m.MaMay == s.MaMay).Select(m => m.TenMay).FirstOrDefault(),
                    ThoiGianBatDau = s.ThoiGianBatDau,
                    ThoiGianKetThuc = s.ThoiGianKetThuc,
                    TongThoiGian = s.TongThoiGian,
                    TongTien = s.TongTien,
                })
                .ToList();
            ViewData["SoDu"] = (double)(_context.NguoiDungs.Where(nd => nd.MaNguoiDung == maNguoiDung).Select(nd => nd.SoDu).FirstOrDefault() ?? 0);
            return View(lichSuSuDung);
        }

        [HttpGet]
        public IActionResult ThayDoiThongTinCaNhan()
        {
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
        
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            var viewModel = new ThayDoiThongTinCaNhanViewModel
            {
                HoTen = nguoiDung.HoTen,
                TenDangNhap = nguoiDung.TenDangNhap,
                SoDienThoai = nguoiDung.SoDienThoai,
            };
        
            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult ThayDoiThongTinCaNhan(ThayDoiThongTinCaNhanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
        
            string maNguoiDung = User.FindFirst("MaNguoiDung")?.Value ?? string.Empty;
        
            if (string.IsNullOrEmpty(maNguoiDung))
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
            if (nguoiDung == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
        
            // Kiểm tra số điện thoại có đủ 10 số không
            if (string.IsNullOrEmpty(model.SoDienThoai) || model.SoDienThoai.Length != 10 || !model.SoDienThoai.All(char.IsDigit))
            {
                ModelState.AddModelError("SoDienThoai", "Số điện thoại phải có đúng 10 chữ số.");
                return View(model);
            }
        
            // Kiểm tra mật khẩu hiện tại
            if (nguoiDung.MatKhau != model.MatKhauHienTai)
            {
                ModelState.AddModelError("MatKhauHienTai", "Mật khẩu hiện tại không đúng.");
                return View(model);
            }
        
            // Cập nhật thông tin
            nguoiDung.HoTen = model.HoTen;
            nguoiDung.TenDangNhap = model.TenDangNhap;
            nguoiDung.SoDienThoai = model.SoDienThoai;
        
            // Cập nhật mật khẩu nếu có
            if (!string.IsNullOrEmpty(model.MatKhauMoi) && model.MatKhauMoi == model.XacNhanMatKhauMoi)
            {
                nguoiDung.MatKhau = model.MatKhauMoi;
            }
            else if (!string.IsNullOrEmpty(model.MatKhauMoi))
            {
                ModelState.AddModelError("XacNhanMatKhauMoi", "Mật khẩu mới và xác nhận mật khẩu không khớp.");
                return View(model);
            }
        
            _context.SaveChanges();
        
            TempData["Success"] = "Thông tin cá nhân đã được cập nhật thành công.";
            return RedirectToAction("Home");
        }    }
}