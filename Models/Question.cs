using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BD.Models
{
    public class Question
    {
        public static QUESTION_TYPE StringToType(string s)
        {
            switch (s.ToLower())
            {
                case "closed":
                    return QUESTION_TYPE.Closed;
                case "open":
                    return QUESTION_TYPE.Open;
            }
            return QUESTION_TYPE.Invalid;
        }

        public enum QUESTION_TYPE
        {
            Closed,
            Open,
            Invalid
        }

        public string Name = "";

        public string Text = "";

        public QUESTION_TYPE QuestionType { get; set; }

        public List<string> Answers = new List<string>();

        public string Category = "";

        public bool Shared = false;

        public double Points { get; set; }

        public List<int> CorrectAnswers = new List<int>();

        int ID;

        public Question(int id, string name, string text, string cat, QUESTION_TYPE type, List<string> answ, double points, List<int> corrAnsw, bool shared)
        {
            ID = id;
            Name = name;
            Text = text;
            Category = cat;
            QuestionType = type;
            Answers = answ;
            Shared = shared;
            CorrectAnswers = corrAnsw;
            Points = points;
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public int GetID()
        {
            return ID;
        }

    }
}
