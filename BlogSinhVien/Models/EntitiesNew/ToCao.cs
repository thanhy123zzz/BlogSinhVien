using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class ToCao
    {
        public int Id { get; set; }
        public int? Iduser { get; set; }
        public int? Idbd { get; set; }
        public DateTime? NgayToCao { get; set; }

        public virtual BaiDang IdbdNavigation { get; set; }
        public virtual Users IduserNavigation { get; set; }
    }
}
