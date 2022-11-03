using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class QuanLy
    {
        public QuanLy()
        {
            BaiDang = new HashSet<BaiDang>();
            BinhLuan = new HashSet<BinhLuan>();
        }

        public string MaQl { get; set; }
        public string Ten { get; set; }
        public string Ho { get; set; }
        public string EmailPrivate { get; set; }
        public string Cccd { get; set; }
        public string Phone { get; set; }
        public string EmailEdu { get; set; }
        public bool? GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string TaiKhoan { get; set; }
        public byte[] HinhAnh { get; set; }

        public virtual Accounts TaiKhoanNavigation { get; set; }
        public virtual ICollection<BaiDang> BaiDang { get; set; }
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }
    }
}
