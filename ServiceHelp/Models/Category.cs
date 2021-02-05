using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceHelp.Models
{
    public class Category
    {
        public Category()
        {
            IssueCategory = new HashSet<IssueCategory>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int IdCategory { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<IssueCategory> IssueCategory { get; set; }
    }
}