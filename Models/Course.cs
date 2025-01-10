using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace BD.Models
{
    public class Course
    {
        public string? Name { get; set; }

        int _id;

        [JsonInclude]
        public List<User> Teachers { get; set; } = new List<User>();

        [JsonInclude]
        public List<int> Students { get; set; } = new List<int>();

        [JsonInclude]
        public List<int> Tests { get; set; } = new List<int>();

        [JsonInclude]
        public List<int> Results { get; set; } = new List<int>();

        [JsonInclude]
        public string Category { get; set; } = "";

        public string Description { get; set; } = "No description";

        public int ID { get => _id; set => _id = value; }

        // Readonly
        public string MainTeacherName { get; set; } = "";

        public Course(int id, string name, string cat, List<User> teachers)
        {
            ID = id;
            Name = name;
            Teachers = teachers;
            Category = cat;

            MainTeacherName = $"{teachers[0].FirstName} {teachers[0].LastName}";
        }

        public Course(string name, List<User> teachers)
        {
            ID = 0;
            Name = name;
            Teachers = teachers;
            MainTeacherName = $"{teachers[0].FirstName} {teachers[0].LastName}";
        }

        public Course() { }
    }
}
