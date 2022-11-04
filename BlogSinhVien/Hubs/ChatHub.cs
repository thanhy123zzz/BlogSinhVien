using BlogSinhVien.Models.Entities;
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
    }
}
