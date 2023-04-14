using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class ChiTietBaiDang
    {
        public int Id { get; set; }
        public int IdbaiDang { get; set; }
        public string NameFile { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }

        public virtual BaiDang IdbaiDangNavigation { get; set; }
    }
}
