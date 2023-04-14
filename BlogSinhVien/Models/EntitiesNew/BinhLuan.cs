using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class BinhLuan
    {
        public BinhLuan()
        {
            ChiTietBinhLuan = new HashSet<ChiTietBinhLuan>();
            Vote = new HashSet<Vote>();
        }

        public int Id { get; set; }
        public int? IdbaiDang { get; set; }
        public int? Iduser { get; set; }
        public DateTime NgayDang { get; set; }
        public string Content { get; set; }
        public int? Sllike { get; set; }

        public virtual BaiDang IdbaiDangNavigation { get; set; }
        public virtual Users IduserNavigation { get; set; }
        public virtual ICollection<ChiTietBinhLuan> ChiTietBinhLuan { get; set; }
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
