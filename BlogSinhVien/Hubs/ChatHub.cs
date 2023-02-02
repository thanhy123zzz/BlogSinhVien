using BlogSinhVien.Models.EntitiesNew;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace BlogSinhVien.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, string MaC)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Message m = new Message();
            m.SendTime = DateTime.Now;
            m.IduserSend = int.Parse(user);
            m.Content = message;
            m.Idc = int.Parse(MaC);
            m.TrangThai = false;
            context.Message.Add(m);

            Conversation c = context.Conversation.Find(int.Parse(MaC));
            c.LastTime = m.SendTime;
            c.TrangThai = false;
            c.IduserLast = int.Parse(user);
            context.Conversation.Update(c);

            context.SaveChanges();
            string imageDataURL = context.Users.Find(int.Parse(user)).HinhAnh;
            await Clients.All.SendAsync("ReceiveMessage", user, message, MaC, imageDataURL);
        }

        public async Task CommentInsert(string maSV, string content, int MaBD)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            BinhLuan bl = new BinhLuan();
            try
            {
                bl.IdbaiDang = MaBD;
                bl.Content = content;
                bl.Iduser = Int32.Parse(maSV);
                bl.Sllike = 0;
                bl.NgayDang = DateTime.Now;

                context.BinhLuan.Add(bl);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                context.SaveChanges();
            }
            Users nguoiDung = context.Users.Find(Int32.Parse(maSV));
            string imageDataURL = "/images/avts/" + nguoiDung.HinhAnh;
            await Clients.All.SendAsync("DisplayComment", nguoiDung.Ho
                + " " + nguoiDung.Ten, content, MaBD, imageDataURL,
                DateTime.Now.ToString("dd-MM-yyyy HH:mm"), bl.Id, maSV);

        }
        public async Task Vote(int MaBD, int MaCmt, string MaUser)
        {
            BlogSinhVienNewContext context = new BlogSinhVienNewContext();
            Vote c = context.Vote.Find(MaCmt, int.Parse(MaUser));
            BinhLuan bl = context.BinhLuan.Find(MaCmt);
            if (c != null)
            {
                context.Remove(c);
                bl.Sllike -= 1;
                context.BinhLuan.Update(bl);
                context.SaveChanges();
            }
            else
            {
                Vote v = new Vote();
                v.MaUser = int.Parse(MaUser);
                v.TimeVote = DateTime.Now;
                v.MaCmt = MaCmt;
                bl.Sllike += 1;
                context.BinhLuan.Update(bl);
                context.Add(v);
                context.SaveChanges();
            }
            await Clients.All.SendAsync("IncreateVote", MaBD, MaCmt, MaUser, bl.Sllike);
        }
    }
}
