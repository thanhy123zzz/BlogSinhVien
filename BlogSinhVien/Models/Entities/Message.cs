using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class Message
    {
        public int? MaC { get; set; }
        public string MaSinhVienSend { get; set; }
        public string MaSinhVienReceive { get; set; }
        public DateTime? SendTime { get; set; }
        public string Content { get; set; }

        public virtual Conversation MaCNavigation { get; set; }
        public virtual SinhVien MaSinhVienReceiveNavigation { get; set; }
        public virtual SinhVien MaSinhVienSendNavigation { get; set; }
    }
}
