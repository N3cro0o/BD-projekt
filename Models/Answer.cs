using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Models
{
    public class Answer
    {
        public Question.QUESTION_TYPE Type;
        public string? UserAnswer;
        public int Test;
        public int Course;
        public int User;
        public int Points;
    }
}
