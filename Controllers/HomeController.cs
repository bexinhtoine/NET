using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.Data; 
using Project.Models.Home;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Project.Models;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProjetcNetQuanLyMayTinhContext _context; // DbContext

        public HomeController(ProjetcNetQuanLyMayTinhContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Đăng Nhập
        [HttpGet]
        public IActionResult DangNhap()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult DangNhap(DangNhapViewModel model)
        {
            if (ModelState.IsValid)
            {
                var nguoiDung = _context.NguoiDungs
                    .FirstOrDefault(nd => nd.TenDangNhap == model.Username && nd.MatKhau == model.Password);
        
                if (nguoiDung != null)
                {
                    // Tạo Claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, nguoiDung.TenDangNhap),
                        new Claim("MaNguoiDung", nguoiDung.MaNguoiDung), // Lưu ID người dùng
                        new Claim("HoTen", nguoiDung.HoTen), // Lưu họ tên
                        new Claim(ClaimTypes.Role, nguoiDung.Role ?? string.Empty) // Lưu vai trò
                    };
        
                    // Tạo ClaimsIdentity
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
                    // Tạo ClaimsPrincipal
                    var principal = new ClaimsPrincipal(identity);
        
                    // Đăng nhập người dùng
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        
                    // Chuyển hướng dựa trên vai trò
                    if (nguoiDung.Role == "Khách")
                    {
                        return RedirectToAction("Home", "KhachHang");
                    }
                    else if (nguoiDung.Role == "Admin")
                    {
                        return RedirectToAction("Home", "Admin");
                    }
                }
        
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
            }
        
            return View(model);
        }
        
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult DangKy(DangKyViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                var existingUserByUsername = _context.NguoiDungs
                    .FirstOrDefault(nd => nd.TenDangNhap == model.Username);
        
                if (existingUserByUsername != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại.");
                    return View(model);
                }
        
                // Kiểm tra xem số điện thoại đã tồn tại chưa
                var existingUserByPhone = _context.NguoiDungs
                    .FirstOrDefault(nd => nd.SoDienThoai == model.PhoneNumber);
        
                if (existingUserByPhone != null)
                {
                    ModelState.AddModelError("PhoneNumber", "Số điện thoại đã được sử dụng.");
                    return View(model);
                }

                // Kiểm tra số điện thoại có đủ 10 số không
                if (model.PhoneNumber == null || model.PhoneNumber.Length != 10 || !model.PhoneNumber.All(char.IsDigit))
                {
                    ModelState.AddModelError("PhoneNumber", "Số điện thoại phải có đúng 10 chữ số.");
                    return View(model);
                }
        
                // Lấy ID lớn nhất hiện có trong cơ sở dữ liệu
                var lastUser = _context.NguoiDungs
                    .OrderByDescending(nd => nd.MaNguoiDung)
                    .FirstOrDefault();
        
                string newId;
                if (lastUser == null)
                {
                    // Nếu chưa có người dùng nào, bắt đầu từ 0000000001
                    newId = "0000000001";
                }
                else
                {
                    // Tăng ID lên 1
                    newId = (long.Parse(lastUser.MaNguoiDung) + 1).ToString("D10");
                }
        
                // Tạo người dùng mới
                var nguoiDungMoi = new NguoiDung
                {
                    MaNguoiDung = newId, // Gán ID mới
                    HoTen = model.FullName ?? string.Empty,
                    TenDangNhap = model.Username ?? string.Empty,
                    MatKhau = model.Password ?? string.Empty,
                    SoDienThoai = model.PhoneNumber,
                    Role = "Khách", // Gán vai trò mặc định là "Khách"
                    NgayTaoTaiKhoan = DateTime.Now, // Gán ngày tạo tài khoản
                    TrangThai = "Hoạt động" // Gán trạng thái mặc định
                };
        
                _context.NguoiDungs.Add(nguoiDungMoi);
                _context.SaveChanges();
        
                // Đăng ký thành công
                TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("DangNhap");
            }
        
            return View(model);
        }
        
        [HttpGet]
        public IActionResult DangXuat()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("DangNhap", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}