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
        [JsonInclude]
        int ID { get; set; }

        [JsonInclude]
        List<int> Questions { get; set; } = new List<int>();

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Category { get; set; }

        [JsonInclude]
        List<Answer> Answers { get; set; } = new List<Answer>();

        [JsonInclude]
        int Course { get; set; }
        
        int CurrentQuestion = 0;

        public Test(int id, List<int> quest, DateTime start, DateTime end, int cat)
        {
            ID = id;
            Questions = quest;
            StartDate = start;
            EndDate = end;
            Category = cat;
        }

    }
}
