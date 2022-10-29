using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class Roles
    {
        public string MaRole { get; set; }
        public string TenRole { get; set; }
        public byte[] HinhAnh { get; set; }
        public virtual ICollection<Accounts> Accounts { get; set; }
    }
}
