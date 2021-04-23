using ServiceHelp.Data;
using ServiceHelp.Models;
using MailKit.Net.Pop3;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace ServiceHelp.Services
{
    public class IssueMailReaderService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContext;

        private string _mail = "czeslaw.czeslaw222@interia.pl";
        private string _pass = "1234Qwer.?";
        private string _host = "poczta.interia.pl";
        private int _portSmtp = 465;
        private int _portPop3 = 995;

        public IssueMailReaderService(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContext, LinkGenerator linkGenerator)
        {
            _db = db;
            _userManager = userManager;
            _httpContext = httpContext;
            _linkGenerator = linkGenerator;
        }

        public void Do()
        {
            List<Issue> donloadedIssues = new List<Issue>();
            using (Pop3Client client = new Pop3Client())
            {
                client.Connect(_host, _portPop3, true);
                client.Authenticate(_mail, _pass);

                int count = client.GetMessageCount();

                for (int i = 0; i < count; i++)
                {
                    var mail = client.GetMessage(i);
                    string email = ((MimeKit.MailboxAddress)mail.From.First()).Address;
                    if (mail.Body.ContentType.MediaType == "text")
                        try
                        {
                            IdentityUser user = _userManager.FindByNameAsync(email).Result;
                            if (user == null)
                            {
                                user = new IdentityUser(email);
                                user.Email = email;
                                _userManager.CreateAsync(user).Wait();
                            }

                            var issue = new Issue();
                            issue.IdUser = user.Id;
                            issue.User = user;
                            issue.Date = DateTime.Now;
                            issue.IdPrioritet = _db.Prioritet.Where(a => a.CodeName == "mid").First().IdPrioritet;
                            issue.IdStatus = _db.Status.Where(a => a.CodeName == "new").First().IdStatus;
                            issue.Title = mail.Subject;
                            issue.Description = ((MimeKit.TextPart)mail.Body).Text;
                            _db.Add(issue);
                            _db.SaveChanges();
                            client.DeleteMessage(i);
                            donloadedIssues.Add(issue);
                        }
                        catch (Exception e)
                        {

                        }
                }
                client.Disconnect(true);
            }

            foreach (var item in donloadedIssues)
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("ServiceHelp", _mail));
                message.To.Add(new MailboxAddress(item.User.UserName, item.User.Email));
                message.Subject = "Przyjęto zgłoszenie";
                message.Body = new TextPart("plain") { Text = $"Zarejestrowano zgłoszenie w systemie ServiceHelp. Link do zgłosznia {_linkGenerator.GetUriByAction("Details", "Issue", new { id = item.IdIssue }, _httpContext.HttpContext.Request.Scheme, _httpContext.HttpContext.Request.Host)}" };

                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_host, _portSmtp, true);
                    //smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    smtp.Authenticate(_mail, _pass);

                    smtp.Send(message);
                    smtp.Disconnect(true);
                }
            };
        }
    }
}
