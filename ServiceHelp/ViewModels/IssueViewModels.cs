using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ServiceHelp.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Data zgłoszenia")]
        public DateTime Date { get; set; }

        [Required]
        [DisplayName("Priorytet")]
        public int IdPrioritet { get; set; }

        [Required]
        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [DisplayName("Opis")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public int IdStatus { get; set; }

        [DisplayName("Kategorie")]
        public int[] CategoryIds { get; set; }
    }

    public class IssueViewModelDetails
    {
        public int Id { get; set; }

        [DisplayName("Data zgłoszenia")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DisplayName("Priorytet")]
        public string Prioritet { get; set; }

        [DisplayName("Tytuł")]
        public string Title { get; set; }

        [DisplayName("Opis")]
        public string Description { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Kategorie")]
        public string[] CategoryNames { get; set; }

        [DisplayName("Osoba zgłaszjąca")]
        public string User { get; set; }

        [DisplayName("Serwisant")]
        public string ServiceUser { get; set; }

    }
}