using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BD.Models
{
    public class Result
    {
        public Test Test { get; set; }

        public int StudentID { get; set; }

        public User Student { get; set; }

        public double Points { get; set; }

        public string Feedback { get; set; }

        public Report? MainReport { get; set; }

        int _reportID = 0;

        int _ID = 0;

        public int ID { get => _ID; }
        public int ReportID { get => _reportID; set => _reportID = value; }

        public Result()
        {

        }

        public Result(int id, double points, string feedback, int student, int report, int test)
        {
            _ID = id;
            Points = points;
            Feedback = feedback;
            Student = App.DBConnection.GetUserByID(student);
            MainReport = App.DBConnection.ReturnReportByID(report).Item2;
            Test = App.DBConnection.ReturnTestsListWithID(test)[0];
            StudentID = student;
            _reportID = report;
        }

        public bool HasReport()
        {
            return MainReport != null;
        }
    }
}
