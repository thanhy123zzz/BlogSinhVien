using BlogSinhVien.Models;
using BlogSinhVien.Models.Entities;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace BlogSinhVien.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string MaC)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            string sv02;
            string imageBase64Data;
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
            if (sv.HinhAnh == null) imageDataURL = "/images/avt.png";
            else
            {
                imageBase64Data = Convert.ToBase64String(sv.HinhAnh);
                imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            }
            context.Database.ExecuteSqlRaw("insert into Message values(" + MaC + ",'" + user + "','" + sv02 + "',GETDATE(),N'" + message + "')");

            await Clients.All.SendAsync("ReceiveMessage", user, message, MaC, imageDataURL);
        }

        public async Task CommentInsert(string maSV, string content, int MaBD)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            BinhLuan bl = new BinhLuan();

            bl.MaBaiDang = MaBD;
            bl.Content = content;
            if (context.SinhVien.Find(maSV) != null)
            {
                bl.MaSinhVien = maSV;
                bl.NgayDang = DateTime.Now;
                context.BinhLuan.Add(bl);
                context.SaveChanges();

                BinhLuan binhLuan = context.BinhLuan.Where(x => x.NgayDang == bl.NgayDang).FirstOrDefault();

                SinhVien sv = context.SinhVien.Find(maSV);
                string imageBase64Data = Convert.ToBase64String(sv.HinhAnh);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                if (sv.HinhAnh == null) imageDataURL = "images/avt.png";
                await Clients.All.SendAsync("DisplayComment", maSV, sv.Ho + " " + sv.Ten, content, MaBD, imageDataURL, DateTime.Now.ToString(),binhLuan.MaCmt, maSV);
            }
            if (context.QuanLy.Find(maSV) != null)
            {
                string imageBase64Data;
                string imageDataURL;
                bl.MaQl = maSV;
                bl.NgayDang = DateTime.Now;
                context.BinhLuan.Add(bl);
                context.SaveChanges();
                BinhLuan binhLuan = context.BinhLuan.Where(x => x.NgayDang == bl.NgayDang).FirstOrDefault();

                QuanLy sv = context.QuanLy.Find(maSV);

                if (sv.HinhAnh == null) imageDataURL = "images/avt.png";
                else
                {
                    imageBase64Data = Convert.ToBase64String(sv.HinhAnh);
                    imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                }
                
                await Clients.All.SendAsync("DisplayComment", maSV, sv.Ho + " " + sv.Ten, content, MaBD, imageDataURL, DateTime.Now.ToString(), binhLuan.MaCmt, maSV);
            }

        }
        public async Task Vote(int MaBD, int MaCmt, string MaUser)
        {
            BlogSinhVienContext context = new BlogSinhVienContext();
            Vote c = context.Vote.Find(MaCmt, MaUser);
            if(c!= null)
            {
                context.Remove(c);
                context.SaveChanges();
            }
            else
            {
                Vote v = new Vote();
                v.MaUser = MaUser;
                v.TimeVote = DateTime.Now;
                v.MaCmt = MaCmt;
                context.Add(v);
                context.SaveChanges();
            }
            int sl = context.Vote.Where(x => x.MaCmt == MaCmt).Count();
            await Clients.All.SendAsync("IncreateVote", MaBD, MaCmt, MaUser,sl);
        }
    }
}
