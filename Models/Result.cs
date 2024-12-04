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
        int Course { get; set; }

        int Test { get; set; }

        public List<Answer> Answers = new List<Answer>();

        int ID { get; set; }

        public Result(int id, int course, int test, List<Answer> answs)
        {
            ID = id;
            Course = course;
            Test = test;
            Answers = answs;
        }

    }
}
