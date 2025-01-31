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
        public List<Result> PassedUsers { get; set; }
        public List<Result> FailedUsers { get; set; }
        public List<Result> ToCheckUsers { get; set; }

        public int PassedUsersInt { get; set; }
        public int FailedUsersInt { get; set; }
        public double AvgScore { get; set; }

        public Report(List<Result> passedUsers, List<Result> failedUsers)
        {
            PassedUsers = passedUsers;
            FailedUsers = failedUsers;
        }

        public Report()
        {
            PassedUsers = new List<Result>();
            FailedUsers = new List<Result>();
            ToCheckUsers = new List<Result>();
            _ID = App.DBConnection.AddEmptyReportToTest(this);
        }

        public Report(int id, int failed, int succeed, double avg)
        {
            PassedUsers = new List<Result>();
            FailedUsers = new List<Result>();
            ToCheckUsers = new List<Result>();
            _ID = id;
            FailedUsersInt = failed;
            PassedUsersInt = succeed;
            AvgScore = avg;
        }

        public double AverageScore()
        {
            double d = 0;
            foreach (Result result in PassedUsers)
            {
                d += result.Points;
            }
            foreach (Result result in FailedUsers)
            {
                d += result.Points;
            }
            foreach (Result result in ToCheckUsers)
            {
                d += result.Points;
            }
            return d / (PassedUsers.Count + FailedUsers.Count + ToCheckUsers.Count);
        }
    }
}
