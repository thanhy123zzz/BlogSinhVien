using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class ChiTietCmt
    {
        public int? MaCmt { get; set; }
        public byte[] Files { get; set; }
        public string Type { get; set; }
        public string NameFile { get; set; }

        public virtual BinhLuan MaCmtNavigation { get; set; }
    }
}
