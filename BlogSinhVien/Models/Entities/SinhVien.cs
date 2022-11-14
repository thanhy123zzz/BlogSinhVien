using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

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

        [Display(Name = "Mã số sinh viên")]
        [Required(ErrorMessage = "Phải có mã số sinh viên")]
        [StringLength(10, MinimumLength = 9, ErrorMessage = "Độ dài chưa chính xác")]
        public string MaSv { get; set; }
        [Required(ErrorMessage = "Phải có tên")]
        [Display(Name = "Tên")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Phải có họ")]
        [Display(Name = "Họ")]
        public string Ho { get; set; }
        [EmailAddress]
        [Display(Name = "Email cá nhân")]
        public string EmailPrivate { get; set; }
        [Display(Name = "Căn cước công dân")]
        [RegularExpression("([0-9]+)", ErrorMessage = "CCCD là 1 dãy số")]
        public string Cccd { get; set; }
        [Display(Name = "Số điện thoại")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Số điện thoại là 1 dãy số")]
        public string Phone { get; set; }
        [EmailEduValidation]
        [Required(ErrorMessage = "Nhập email trường")]
        [Display(Name = "Email trường")]
        public string EmailEdu { get; set; }
        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Nhập giới tính")]
        public bool? GioiTinh { get; set; }
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; }
        [Display(Name = "Tài khoản")]
        public string TaiKhoan { get; set; }
        public byte[] HinhAnh { get; set; }

        public virtual Accounts TaiKhoanNavigation { get; set; }
        public virtual ICollection<BaiDang> BaiDang { get; set; }
        public virtual ICollection<BinhLuan> BinhLuan { get; set; }
        public virtual ICollection<Conversation> ConversationMaSinhVien1Navigation { get; set; }
        public virtual ICollection<Conversation> ConversationMaSinhVien2Navigation { get; set; }
    }
}
