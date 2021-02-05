using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.ViewModels
{
    public class IssueViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Data zgłoszenia")]
        public DateTime Date { get; set; }

        [Required]
        [Display(Name = "Priorytet")]
        public int IdPrioritet { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Status")]
        public int IdStatus { get; set; }

        [Display(Name = "Kategorie")]
        public int[] CategoryIds { get; set; }
    }


    public class IssueViewModelDetails
    {
        public int Id { get; set; }

        [Display(Name = "Data zgłoszenia")]
        public DateTime Date { get; set; }

        [Display(Name = "Priorytet")]
        public string Prioritet { get; set; }

        [Display(Name = "Tytuł")]
        public string Title { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Kategorie")]
        public string[] CategoryNames { get; set; }

        [Display(Name = "Osoba zgłaszjąca")]
        public string User { get; set; }

        [Display(Name = "Serwisant")]
        public string ServiceUser { get; set; }

    }
}