using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace BD.Models
{
    public class Test
    {
        int _id;

        List<int> Questions { get; set; } = new List<int>();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Category { get; set; }
        
        List<Answer> Answers { get; set; } = new List<Answer>();

        public int CourseID { get; set; }

        public string Name { get; set; } = "";

        public int ID { get => _id; set => _id = value; }

        public Test(int id, string name, int courseID, List<int> quest, DateTime start, DateTime end, string cat)
        {
            ID = id;
            CourseID = courseID;
            Questions = quest;
            StartDate = start;
            Name = name;
            EndDate = end;
            Category = cat;
        }

        public Test (int courseID, string cat, DateTime start, DateTime end)
        {
            ID = 0;
            CourseID = courseID;
            Questions = new List<int>();
            StartDate = start;
            EndDate = end;
            Category = cat;
        }

    }
}
