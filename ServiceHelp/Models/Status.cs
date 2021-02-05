using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceHelp.Models
{
    public class Status
    {
        public Status()
        {
            Issue = new HashSet<Issue>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int IdStatus { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string CodeName { get; set; }

        public virtual ICollection<Issue> Issue { get; set; }
    }
}