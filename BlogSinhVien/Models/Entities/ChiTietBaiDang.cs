using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class ChiTietBaiDang
    {
        public int? MaBaiDang { get; set; }
        public byte[] Files { get; set; }

        public virtual BaiDang MaBaiDangNavigation { get; set; }
    }
}
