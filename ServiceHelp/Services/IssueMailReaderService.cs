using ServiceHelp.Data;
using ServiceHelp.Models;
using MailKit.Net.Pop3;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace ServiceHelp.Services
{
    public class IssueMailReaderService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public IssueMailReaderService(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public void Do()
        {
            using (Pop3Client client = new Pop3Client())// new ProtocolLogger("pop3.log"));
            {
                client.Connect("poczta.interia.pl", 995, true);
                client.Authenticate("czeslaw.czeslaw222@interia.pl", "1234Qwer.?");

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
                            issue.Date = DateTime.Now;
                            issue.IdPrioritet = _db.Prioritet.Where(a => a.CodeName == "mid").First().IdPrioritet;
                            issue.IdStatus = _db.Status.Where(a => a.CodeName == "new").First().IdStatus;
                            issue.Title = mail.Subject;
                            issue.Description = ((MimeKit.TextPart)mail.Body).Text;
                            _db.Add(issue);
                            _db.SaveChanges();
                            client.DeleteMessage(i);
                        }
                        catch (Exception e)
                        {

                        }
                }
                client.Disconnect(true);
            }
        }
    }
}
