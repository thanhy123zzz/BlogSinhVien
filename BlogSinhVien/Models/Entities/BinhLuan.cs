using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class BinhLuan
    {
        public int MaCmt { get; set; }
        public int? MaBaiDang { get; set; }
        public string MaSinhVien { get; set; }
        public string MaQl { get; set; }
        public DateTime NgayDang { get; set; }
        public string Content { get; set; }

        public virtual BaiDang MaBaiDangNavigation { get; set; }
        public virtual QuanLy MaQlNavigation { get; set; }
        public virtual SinhVien MaSinhVienNavigation { get; set; }
    }
}
