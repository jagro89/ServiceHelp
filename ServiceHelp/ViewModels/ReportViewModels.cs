using System;
using System.Collections.Generic;

namespace ServiceHelp.ViewModels
{
    public class ReportItemViewModels
    {
        public string Category { get; set; }
        public int CountIssue { get; set; }
    }

    public class ReportIssueViewModels
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }


    public class ReportViewModels
    {
        public List<ReportIssueViewModels> Issues { get; set; }

        public List<ReportItemViewModels> ReportItems { get; set; }
    }
}