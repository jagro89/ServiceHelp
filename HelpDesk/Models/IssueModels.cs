using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.DynamicData;

namespace HelpDesk.Models
{
    public class Prioritet
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Display(Name = "Priorytet")]
        public string Name { get; set; }

        [Required]
        public string CodeName { get; set; }

        [Display(Name = "Zgłoszenia")]
        public virtual ICollection<Issue> Issues { get; set; }
    }

    public class Status
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Display(Name = "Status")]
        public string Name { get; set; }

        [Required]
        public string CodeName { get; set; }

        [Display(Name = "Zgłoszenia")]
        public virtual ICollection<Issue> Issues { get; set; }

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
        public virtual ICollection<Issue> Issue { get; set; }
    }

    public class Issue
    {
        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Display(Name = "Osoba zgłaszjąca")]
        [Required]
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Data zgłoszenia")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Priorytet")]
        public virtual Prioritet Prioritet { get; set; }  // tu nie dociaga sam tych powiazan bo nie ma virtual
        
        [Display(Name = "Serwisant")]
        public virtual ApplicationUser ServiceUser { get; set; }

        [Display(Name = "Tytuł")]
     //   [EmailAddress]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        public virtual Status Status { get; set; }

        [Display(Name = "Załączniki")]
        public virtual ICollection<AttachmentIssue> Attachment { get; set; }

        [Display(Name = "Kategorie")]
        public virtual ICollection<CategoryIssue> Category { get; set; }
    }
}