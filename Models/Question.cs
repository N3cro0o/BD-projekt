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
        public enum QUESTION_TYPE
        {
            Closed,
            Open
        }

        [JsonInclude]
        string Text = "";

        [JsonInclude]
        QUESTION_TYPE QuestionType {  get; set; }

        [JsonInclude]
        List<string> Answers = new List<string>();

        [JsonInclude]
        int Points { get; set; }

        [JsonInclude]
        List<int> CorrectAnswers = new List<int>();

        [JsonInclude]
        int ID;
        
        public Question(int id, string text, QUESTION_TYPE type, List<string> answ, int points, List<int> corrAnsw)
        {
            ID = id;
            Text = text;
            QuestionType = type;
            Answers = answ;
            CorrectAnswers = corrAnsw;
            Points = points;
        }

        
    }
}
