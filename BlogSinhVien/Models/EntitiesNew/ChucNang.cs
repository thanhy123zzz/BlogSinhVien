using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class ChucNang
    {
        public int Id { get; set; }
        public string MaChucNang { get; set; }
        public string TenChucNang { get; set; }
        public int? IdvaiTro { get; set; }
        public bool? Them { get; set; }
        public bool? Xoa { get; set; }
        public bool? Sua { get; set; }
        public bool? TimKiem { get; set; }

        public virtual VaiTro IdvaiTroNavigation { get; set; }
    }
}
