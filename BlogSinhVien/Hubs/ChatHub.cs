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
            if (context.Conversation.Find(int.Parse(MaC)).MaSinhVien1 == user)
            {
                sv02 = context.Conversation.Find(int.Parse(MaC)).MaSinhVien2;
            }
            else
            {
                sv02 = context.Conversation.Find(int.Parse(MaC)).MaSinhVien1;
            }

            context.Database.ExecuteSqlRaw("insert into Message values("+MaC+",'"+user+"','"+ sv02 + "',GETDATE(),N'"+message+"')");

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
