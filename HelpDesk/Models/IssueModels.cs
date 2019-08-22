using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDesk.Models
{
    public class IssueModels
    {
        public class Prioritet
        {
            [Key]
            [ScaffoldColumn(false)]
            public int Id { get; set; }
            [Display(Name = "Priorytet")]
            public string Name { get; set; }
        }

        public class Status
        {
            [Key]
            [ScaffoldColumn(false)]
            public int Id { get; set; }
            [Display(Name = "Status")]
            public string Name { get; set; }
        }

        public class AttachmentIssue
        {
            [Key]
            [ScaffoldColumn(false)]
            public int Id { get; set; }
            [Display(Name = "Nawa pliku")]
            public string FileName { get; set; }
            [Display(Name = "Plik")]
            public byte[] Attachment { get; set; }
        }

        public class CategoryIssue
        {
            [Key]
            [ScaffoldColumn(false)]
            public int Id { get; set; }
            [Display(Name = "Kategoria")]
            public string Name { get; set; }
            [Display(Name = "Zgłoszenia")]
            public ICollection<Issue> Issue { get; set; }
        }

        public class Issue
        {
            [Key]
            [ScaffoldColumn(false)]
            public int Id { get; set; }

            [Display(Name = "Osoba zgłaszjąca")]
            public ApplicationUser User { get; set; }

            [Display(Name = "Data zgłoszenia")]
            public DateTime Date { get; set; }

            [Display(Name = "Priorytet")]
            public Prioritet Prioritet { get; set; }

            [Display(Name = "Serwisant")]
            public ApplicationUser ServiceUser { get; set; }

            [Display(Name = "Tytuł")]
            public string Title { get; set; }

            [Display(Name = "Opis")]
            public string Description { get; set; }

            [Display(Name = "Status")]
            public Status Status { get; set; }

            [Display(Name = "Załączniki")]
            public ICollection<AttachmentIssue> Attachment { get; set; }

            [Display(Name = "Kategorie")]
            public ICollection<CategoryIssue> Category { get; set; }
        }
    }
}