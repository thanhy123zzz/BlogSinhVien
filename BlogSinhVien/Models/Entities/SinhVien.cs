using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.Entities
{
    public partial class SinhVien
    {
        public SinhVien()
        {
            BaiDang = new HashSet<BaiDang>();
            BinhLuan = new HashSet<BinhLuan>();
            ConversationMaSinhVien1Navigation = new HashSet<Conversation>();
            ConversationMaSinhVien2Navigation = new HashSet<Conversation>();
        }

        public string MaSv { get; set; }
        public string Ten { get; set; }
        public string Ho { get; set; }
        public string EmailPrivate { get; set; }
        public string Cccd { get; set; }
        public string Phone { get; set; }
        public string EmailEdu { get; set; }
        public bool? GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string TaiKhoan { get; set; }

        public virtual Accounts TaiKhoanNavigation { get; set; }
        public virtual ICollection<BaiDang> BaiDang { get; set; }
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }
        public virtual ICollection<Conversation> ConversationMaSinhVien1Navigation { get; set; }
        public virtual ICollection<Conversation> ConversationMaSinhVien2Navigation { get; set; }
    }
}
