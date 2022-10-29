using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class BaiDang
    {
        public BaiDang()
        {
            BinhLuan = new HashSet<BinhLuan>();
        }

        public int MaBaiDang { get; set; }
        public string MaSinhVien { get; set; }
        public string MaQl { get; set; }
        public DateTime NgayDang { get; set; }
        public bool? TrangThai { get; set; }
        public string Content { get; set; }

        public virtual QuanLy MaQlNavigation { get; set; }
        public virtual SinhVien MaSinhVienNavigation { get; set; }
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }
    }
}
