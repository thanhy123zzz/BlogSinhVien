using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class Conversation
    {
        public int MaC { get; set; }
        public string MaSinhVien1 { get; set; }
        public string MaSinhVien2 { get; set; }

        public virtual SinhVien MaSinhVien1Navigation { get; set; }
        public virtual SinhVien MaSinhVien2Navigation { get; set; }
    }
}
