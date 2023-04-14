using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class Conversation
    {
        public Conversation()
        {
            Message = new HashSet<Message>();
        }

        public int Id { get; set; }
        public int? Iduser1 { get; set; }
        public int? Iduser2 { get; set; }
        public DateTime NgayTao { get; set; }
        public int? IduserLast { get; set; }
        public DateTime? LastTime { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Users Iduser1Navigation { get; set; }
        public virtual Users Iduser2Navigation { get; set; }
        public virtual Users IduserLastNavigation { get; set; }
        public virtual ICollection<Message> Message { get; set; }
    }
}
