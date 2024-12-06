using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BD.Models
{
    public class Report
    {
        List<int> Results { get; set; } = new List<int>();
        List<int> PassedUsers { get; set; } = new List<int>();
        List<int> FailedUsers { get; set; } = new List<int>();

        public Report(List<int> results, List<int> passedUsers, List<int> failedUsers)
        {
            Results = results;
            PassedUsers = passedUsers;
            FailedUsers = failedUsers;
        }

       
    } 
}
