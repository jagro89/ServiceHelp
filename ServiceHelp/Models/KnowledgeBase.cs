using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceHelp.Models
{
    public class KnowledgeBase
    {
        [Key]
        [ScaffoldColumn(false)]
        public int IdKnowledgeBase { get; set; }

        [Required]
        [DisplayName("Pytanie")]
        public string Question { get; set; }

        [Required]
        [DisplayName("Odpowiedź")]
        public string Answer { get; set; }
    }
}