using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BlogSinhVien.Models.EntitiesNew
{
    public partial class Users
    {
        public Users()
        {
            BaiDangIdUserGhimNavigation = new HashSet<BaiDang>();
            BaiDangIduserNavigation = new HashSet<BaiDang>();
            BinhLuan = new HashSet<BinhLuan>();
            ConversationIduser1Navigation = new HashSet<Conversation>();
            ConversationIduser2Navigation = new HashSet<Conversation>();
            ConversationIduserLastNavigation = new HashSet<Conversation>();
            InverseUserTaoNavigation = new HashSet<Users>();
            Message = new HashSet<Message>();
            ToCao = new HashSet<ToCao>();
            Vote = new HashSet<Vote>();
        }

        public int Id { get; set; }
        public string MaSv { get; set; }
        public string Ten { get; set; }
        public string Ho { get; set; }
        public string EmailPrivate { get; set; }
        public string Cccd { get; set; }
        public string Phone { get; set; }
        public string EmailEdu { get; set; }
        public bool? GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public int? Idtk { get; set; }
        public string HinhAnh { get; set; }
        public int? UserTao { get; set; }
        public DateTime? NgayTao { get; set; }
        public string GhiChu { get; set; }

        public virtual Accounts IdtkNavigation { get; set; }
        public virtual Users UserTaoNavigation { get; set; }
        public virtual ICollection<BaiDang> BaiDangIdUserGhimNavigation { get; set; }
        public virtual ICollection<BaiDang> BaiDangIduserNavigation { get; set; }
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }
        public virtual ICollection<Conversation> ConversationIduser1Navigation { get; set; }
        public virtual ICollection<Conversation> ConversationIduser2Navigation { get; set; }
        public virtual ICollection<Conversation> ConversationIduserLastNavigation { get; set; }
        public virtual ICollection<Users> InverseUserTaoNavigation { get; set; }
        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<ToCao> ToCao { get; set; }
        public virtual ICollection<Vote> Vote { get; set; }
    }
}
