using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceHelp.Models
{
    public class AttachmentIssue
    {
        [Key]
        [ScaffoldColumn(false)]
        public int IdAttachmentIssue { get; set; }

        [ForeignKey("Issue")]
        public int IdIssue { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public byte[] Attachment { get; set; }

        public virtual Issue Issue { get; set; }
    }
}