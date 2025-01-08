using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


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

        public Question(string name, string text, QUESTION_TYPE type, List<string> answ, double points, List<int> corrAnsw, string cat = "unknown", bool shared = false, int id = 0)
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

        public void PrintQuestionOnConsole()
        {
            Debug.Print(string.Format("ID {0}, Name {1}\nText {2}\nAnswer count {3}, real answ count {4}, type {5}", ID, Name, Text, Answers.Count, CorrectAnswers.Count, QuestionType.ToString()));
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
