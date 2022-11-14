using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class Accounts
    {
        public Accounts()
        {
            QuanLy = new HashSet<QuanLy>();
            SinhVien = new HashSet<SinhVien>();
        }

        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public string MaRole { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Roles MaRoleNavigation { get; set; }
        public virtual ICollection<QuanLy> QuanLy { get; set; }
        public virtual ICollection<SinhVien> SinhVien { get; set; }
    }
}
