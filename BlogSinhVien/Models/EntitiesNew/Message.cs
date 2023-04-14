using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class Message
    {
        public int Id { get; set; }
        public int? Idc { get; set; }
        public int? IduserSend { get; set; }
        public DateTime? SendTime { get; set; }
        public string Content { get; set; }
        public bool? TrangThai { get; set; }

        public virtual Conversation IdcNavigation { get; set; }
        public virtual Users IduserSendNavigation { get; set; }
    }
}
