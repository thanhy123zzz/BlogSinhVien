using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class Vote
    {
        public int MaCmt { get; set; }
        public string MaUser { get; set; }
        public DateTime? TimeVote { get; set; }

        public virtual BinhLuan MaCmtNavigation { get; set; }
    }
}
