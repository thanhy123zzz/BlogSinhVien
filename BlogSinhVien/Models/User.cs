using Microsoft.AspNetCore.Identity;

namespace BlogSinhVien.Models
{
    public class User: IdentityUser
    {
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string MaRole { get; set; }
    }
}
