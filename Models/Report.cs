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
        int _ID;

        public int ID { get => _ID; }

        public List<Result> Results { get; set; }
        public List<User> PassedUsers { get; set; }
        public List<User> FailedUsers { get; set; }

        public int PassedUsersInt { get; set; }
        public int FailedUsersInt { get; set; }
        public double AvgScore { get; set; }

        public Report(List<Result> results, List<User> passedUsers, List<User> failedUsers)
        {
            Results = results;
            PassedUsers = passedUsers;
            FailedUsers = failedUsers;
        }

        public Report()
        {
            Results = new List<Result>();
            PassedUsers = new List<User>();
            FailedUsers = new List<User>();
            _ID = App.DBConnection.AddEmptyReportToTest(this);
        }
        
        public Report(int id, int failed, int succeed, double avg)
        {
            Results = new List<Result>();
            PassedUsers = new List<User>();
            FailedUsers = new List<User>();
            //_ID = App.DBConnection.AddEmptyReportToTest(this); Debug change
            _ID = id;
            FailedUsersInt = failed;
            PassedUsersInt = succeed;
            AvgScore = avg;
        }

        public double AverageScore()
        {
            double d = 0;
            foreach (Result result in Results)
            {
                d += result.Points;
            }
            return d / Results.Count;
        }
    }
}
