using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections;


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

        public string Name { get; set; } = "";

        public string Text { get; set; } = "";

        public QUESTION_TYPE QuestionType { get; set; }

        public string Answers { get; set; } = "";

        public string Category { get; set; } = "";

        public bool Shared { get; set; } = false;

        public double Points { get; set; }

        /// <summary>
        /// In DB, we have columns A, B, C, D. Here, we store them inside a bitmask. To get binary string use CorrectAnswersBinary.
        /// </summary>
        public int CorrectAnswers { get; set; } = 0;

        public string CorrectAnswersBinary { get => CorrectAnswers.ToString("b"); }

        int _id;

        public int ID { get => _id; set => _id = value; }

        public Question(string name, string text, QUESTION_TYPE type, string answ, double points, int corrAnsw, string cat = "unknown", bool shared = false, int id = 0)
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

            validateKey();
        }

        public Question()
        {
            ID = -1;
            validateKey();
        }
        public void PrintQuestionOnConsole()
        {
            Debug.Print(string.Format("ID {0}, Name {1}\nText {2}\nAnswer count {3}, real answ count {4}, type {5}", ID, Name, Text, Answers, CorrectAnswers, QuestionType.ToString()));
        }

        public void SetID(int id)
        {
            ID = id;
        }

        public int GetID()
        {
            return ID;
        }

        public bool IsEmpty()
        {
            return ID < 0;
        }

        public string ReturnCorrectAnswerString()
        {
            return ((CorrectAnswers & 1 << 3) >> 3 == 1 ? "A" : "") + ((CorrectAnswers & 1 << 2) >> 2 == 1 ? "B" : "") + 
                ((CorrectAnswers & 1 << 1) >> 1 == 1 ? "C" : "") + ((CorrectAnswers & 1 << 0) >> 0 == 1 ? "D" : "");
        }

        void validateKey()
        {
            QUESTION_TYPE newType = QUESTION_TYPE.Invalid;
            if (CorrectAnswers > 0)
                newType = QUESTION_TYPE.Closed;
            else
                newType = QUESTION_TYPE.Open;

            if (newType != QuestionType)
                Debug.Print("WRONG QUESTION TYPE: " + Name);
        }
    }
}
