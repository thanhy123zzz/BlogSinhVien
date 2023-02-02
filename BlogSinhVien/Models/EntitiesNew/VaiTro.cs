using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class VaiTro
    {
        public VaiTro()
        {
            Accounts = new HashSet<Accounts>();
            ChucNang = new HashSet<ChucNang>();
        }

        public int Id { get; set; }
        public string MaVaiTro { get; set; }
        public string TenVaiTro { get; set; }

        public virtual ICollection<Accounts> Accounts { get; set; }
        public virtual ICollection<ChucNang> ChucNang { get; set; }
    }
}
