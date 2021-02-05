using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceHelp.Models
{
    public class Issue
    {
        public Issue()
        {
            Attachment = new HashSet<AttachmentIssue>();
            IssueCategory = new HashSet<IssueCategory>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int IdIssue { get; set; }

        [Required]
        [ForeignKey("User")]
        public string IdUser { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [ForeignKey("Prioritet")]
        public int IdPrioritet { get; set; } 

        [ForeignKey("ServiceUser")]
        public string IdServiceUser { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        [ForeignKey("Status")]
        public int IdStatus { get; set; }


        public virtual IdentityUser User { get; set; }

        public virtual Prioritet Prioritet { get; set; } 

        public virtual IdentityUser ServiceUser { get; set; }

        public virtual Status Status { get; set; }

        public virtual ICollection<AttachmentIssue> Attachment { get; set; }

        public virtual ICollection<IssueCategory> IssueCategory { get; set; }
    }
}