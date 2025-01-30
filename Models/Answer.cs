using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Models
{
    public class Answer
    {
        int _ID;
        int _questionID;
        int _userID;
        int _testID;
        public double Points { get; set; }
        public string PointsString { get => Points.ToString(CultureInfo.InvariantCulture); }
        public int AnswerKey { get; set; }
        public string AnswerBody { get; set; } = "";

        public int ID { get { return _ID; } }
        public int UserID { get { return _userID; } }
        public string UserName { get; set; } = "";
        public int TestID { get { return _testID; } }
        public string TestName { get; set; } = "";
        public int QuestID { get { return _questionID; } }
        public string QuestName { get; set; } = "";

        public Answer(int id,  double points, int key, string body)
        {
            _ID = id;
            Points = points;
            AnswerKey = key;
            AnswerBody = body;
        }

        public Answer(int id, int user, int quest, int test, double point, int key, string text)
        {
            _ID = id;
            _userID = user;
            _testID = test;
            _questionID = quest;
            Points = point;
            AnswerKey = key;
            AnswerBody = text;
        }
        
        public Answer()
        {
            _ID = -1;
        }
    }
}
