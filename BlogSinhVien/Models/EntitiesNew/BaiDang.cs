using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class BaiDang
    {
        public BaiDang()
        {
            BinhLuan = new HashSet<BinhLuan>();
            ChiTietBaiDang = new HashSet<ChiTietBaiDang>();
            ToCao = new HashSet<ToCao>();
        }

        public int Id { get; set; }
        public int? Iduser { get; set; }
        public DateTime NgayDang { get; set; }
        public bool? TrangThai { get; set; }
        public string Content { get; set; }
        public int? Slreport { get; set; }
        public bool? Ghim { get; set; }
        public int? IdUserGhim { get; set; }
        public DateTime? NgayGhim { get; set; }

        public virtual Users IdUserGhimNavigation { get; set; }
        public virtual Users IduserNavigation { get; set; }
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }
        public virtual ICollection<ChiTietBaiDang> ChiTietBaiDang { get; set; }
        public virtual ICollection<ToCao> ToCao { get; set; }
    }
}
