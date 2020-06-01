using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using Microsoft.AspNetCore.Identity;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using MimeKit;
using MailKit.Net.Smtp;

namespace WebApplication1.Class
{
    public class MailAdapter
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public MailAdapter(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public MailAdapter()
        {
        }

        public void SendMail(String userId, String msg, String to)
        {

            // _userManager.GetUserId(HttpContext.User)

           
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("flightviewerticket@gmail.com"));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Ticket Purchase Information";
            message.Body = new TextPart("plain")
            {
                Text = msg
            };
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("flightviewerticket@gmail.com", "Cs308proje");

                client.Send(message);
                client.Disconnect(true);
            }
        }


    }
}
