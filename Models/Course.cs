using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BD.Models
{
    public class Course
    {
        public string? Name { get; set; }

        public int ID { get; set; }

        [JsonInclude]
        List<int> Teachers { get; set; } = new List<int>();

        [JsonInclude]
        List<int> Students { get; set; } = new List<int>();

        [JsonInclude]
        List<int> Tests { get; set; } = new List<int>();

        [JsonInclude]
        List<int> Results { get; set; } = new List<int>();
        
        [JsonInclude]
        public int Category { get; set; }

        [JsonInclude]
        public int Owner { get; set; }


        public Course(int id, string name, int cat, List<int> teachers, List<int> students, List<int> tests)
        {
            ID = id;
            Name = name;
            Teachers = teachers;
            Category = cat;
            Students = students;
            Tests = tests;
        }

        
    }
}
