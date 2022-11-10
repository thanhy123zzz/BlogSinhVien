using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace BlogSinhVien.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string MaC)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            string sv02;
            string imageDataURL;
            if (context.Conversation.Find(int.Parse(MaC)).MaSinhVien1 == user)
            {
                sv02 = context.Conversation.Find(int.Parse(MaC)).MaSinhVien2;
            }
            else
            {
                sv02 = context.Conversation.Find(int.Parse(MaC)).MaSinhVien1;
               
            }
            SinhVien sv = context.SinhVien.Find(user);
            string imageBase64Data = Convert.ToBase64String(sv.HinhAnh);
            imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            context.Database.ExecuteSqlRaw("insert into Message values("+MaC+",'"+user+"','"+ sv02 + "',GETDATE(),N'"+message+"')");

            await Clients.All.SendAsync("ReceiveMessage", user, message, MaC, imageDataURL);
        }

        public async Task CommentInsert(string maSV, string content, int MaBD)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            BinhLuan bl = new BinhLuan();
            bl.MaBaiDang = MaBD;
            bl.MaSinhVien = maSV;
            bl.Content = content;
            bl.NgayDang = DateTime.Now;
            context.BinhLuan.Add(bl);
            context.SaveChanges();
            SinhVien sv = context.SinhVien.Find(maSV);
            string imageBase64Data = Convert.ToBase64String(sv.HinhAnh);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            await Clients.All.SendAsync("DisplayComment", maSV, sv.Ho + " " + sv.Ten, content, MaBD, imageDataURL, DateTime.Now.ToString());
        }
    }
}
