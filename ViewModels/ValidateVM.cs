using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BD.Models;

namespace BD.ViewModels
{
    internal class ValidateVM
    {
        public (bool, int) ValidateQuestions()
        {
            int count = 0;
            foreach (Question q in App.DBConnection.ReturnQuestionList())
            {
                bool check = false;
                Answer a = App.DBConnection.FetchAnswer(q.AnswerID);
                // Score
                if (a.Points != q.Points)
                {
                    check = true;
                }
                // Answers
                // We are not using (a, b, c, d) columns anyway

                if (check)
                {
                    count++;
                    q.QuestionType = Question.QUESTION_TYPE.Invalid;
                    App.DBConnection.UpdateQuestion(q);
                }
            }
            if (count > 0)
                return (false, count);
            return (true, 0);
        }
    }
}
