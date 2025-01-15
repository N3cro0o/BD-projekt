using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Models
{
    public class Answer
    {
        int _id;
        public double Points;
        public int AnswerKey;
        public string AnswerBody = "";

        public int ID { get { return _id; } }

        public Answer(int id,  double points, int key, string body)
        {
            _id = id;
            Points = points;
            AnswerKey = key;
            AnswerBody = body;
        }
        
        public Answer()
        {
            _id = -1;
        }
    }
}
