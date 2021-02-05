using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceHelp.Models
{
    public class IssueCategory
    {
    //    [Key]
        [ScaffoldColumn(false)]
        [ForeignKey("Issue")]
        public int IdIssue { get; set; }

     //   [Key]
        [ScaffoldColumn(false)]
        [ForeignKey("Category")]
        public int IdCategory { get; set; }

        public virtual Issue Issue { get; set; }

        public virtual Category Category { get; set; }
    }
}