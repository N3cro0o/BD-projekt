using System.Diagnostics;
using BD.Models;

namespace BD.ViewModels
{
    internal class ReportVM
    {
        private readonly AdminPanelUIVM _admin;
        public void ValidateQuestions()
        {

            //new Thread(() =>
            //{
            //    int count = 0;
            //    foreach (Question q in App.DBConnection.ReturnQuestionList())
            //    {
            //        bool check = false;
            //        Answer a = App.DBConnection.FetchAnswer(q.AnswerID);
            //        // Score
            //        if (a.Points != q.Points)
            //        {
            //            check = true;
            //        }
            //        // Answers
            //        // We are not using (a, b, c, d) columns anyway

            //        if (check)
            //        {
            //            count++;
            //            q.QuestionType = Question.QUESTION_TYPE.Invalid;
            //            App.DBConnection.UpdateQuestion(q);
            //        }
            //    }
            //    if (count == 0)
            //    {
            //        MessageBox.Show("Everything is fine", "Validate questions", MessageBoxButton.OK);
            //    }
            //    else
            //    {
            //        MessageBox.Show($"Incorrect number of questions:\n{count}.", "Validate questions", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    }
            //}).Start();
        }

        public ReportVM(AdminPanelUIVM admin) 
        {
            _admin = admin;
        }

        public List<Result> GenerateResults(IEnumerable<Answer> answArr,Test test, double maxScore)
        {
            List<Result> results = new List<Result>();
            Dictionary<int, double> scores = new Dictionary<int, double>();

            foreach (Answer answ in answArr) 
            {
                if (!scores.ContainsKey(answ.UserID))
                    scores[answ.UserID] = 0;
                scores[answ.UserID] += answ.Points;
            }
            Debug.Print($"Scores size: {scores.Count}");
            foreach (KeyValuePair<int, double> score in scores)
            {
                var r = new Result
                {
                    Test = test,
                    StudentID = score.Key,
                    Student = App.DBConnection.GetUserByID(score.Key),
                    Points = score.Value,
                    Feedback = (score.Value >= maxScore * 0.5) ? "Passed" : "Failed"
                };
                results.Add(r);
            }

            return results;
        }
    }
}
