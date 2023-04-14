using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class Accounts
    {
        public Accounts()
        {
            Users = new HashSet<Users>();
        }

        public int Id { get; set; }
        public string TaiKhoan { get; set; }
        public string MatKhau { get; set; }
        public int? IdvaiTro { get; set; }
        public bool? TrangThai { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual VaiTro IdvaiTroNavigation { get; set; }
        public virtual ICollection<Users> Users { get; set; }
    }
}
