using System.Diagnostics;
using BD.Models;

namespace BD.ViewModels
{
    internal class ReportVM
    {
        private readonly AdminPanelUIVM _admin;
        private List<Result> _lastResults = new List<Result>();
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

        public List<Result> GenerateResults(IEnumerable<Answer> answArr, Test test, double maxScore)
        {
            List<Result> results = new List<Result>();
            Dictionary<int, double> scores = new Dictionary<int, double>();
            Dictionary<int, bool> toCheck = new Dictionary<int, bool>();

            foreach (Answer answ in answArr)
            {
                if (!scores.ContainsKey(answ.UserID))
                {
                    scores[answ.UserID] = 0;
                }
                scores[answ.UserID] += answ.Points;
                toCheck[answ.UserID] = App.DBConnection.ReturnIfQuestionIsOpen(answ.QuestID);
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
                    Feedback = toCheck[score.Key] ? "ToCheck" : ((score.Value >= maxScore * 0.5) ? "Passed" : "Failed")
                };
                results.Add(r);
            }
            _lastResults = results;
            return results;
        }
        /// <summary>
        /// Combines result list from database into previously generated or already prepared result list.
        /// </summary>
        /// <param name="resultsDB">List from which results will be taken</param>
        /// <param name="results">List to be added</param>
        public List<Result> MergeResultLists(List<Result> resultsDB, List<Result> results)
        {
            if (results.Count == 0 && _lastResults.Count == 0)
                return resultsDB;
            if (results.Count == 0)
                results = _lastResults;

            for (int i = 0; i < resultsDB.Count; i++)
            {
                bool check = false;
                var resDB = resultsDB[i];
                for (int j = 0; i < results.Count; i++)
                {
                    var res = results[j];
                    if (resDB.StudentID == res.StudentID)
                    {
                        if (resDB.Points > res.Points)
                            _lastResults[j] = resDB;
                        check = true;
                        break;
                    }
                }
                if (!check)
                {
                    results.Add(resDB);
                }
            }

            return results;
        }
        /// <summary>
        /// Combines result list from database into previously generated or already prepared result list.
        /// </summary>
        /// <param name="resultsDB">List from which results will be taken</param>
        public List<Result> MergeResultLists(List<Result> resultsDB)
        {
            if (_lastResults.Count == 0)
                return resultsDB;
            for (int i = 0; i < resultsDB.Count; i++)
            {
                bool check = false;
                var resDB = resultsDB[i];
                Debug.Print(_lastResults.Count.ToString());
                Debug.Print(resDB.ID.ToString());
                for (int j = 0; j < _lastResults.Count; j++)
                {
                    var res = _lastResults[j];
                    if (resDB.StudentID == res.StudentID)
                    {
                        if (resDB.Points > res.Points)
                            _lastResults[j] = resDB;
                        check = true;
                        Debug.Print(res.StudentID.ToString());
                        break;
                    }
                }
                Debug.Print(_lastResults.Count.ToString());
                if (!check)
                {
                    _lastResults.Add(resDB);
                }
            }

            return _lastResults;
        }

        public (bool, Report?) GenerateReport(List<Result> results, List<Result> resultsDB)
        {
            Report report;
            Debug.Print($"Generated size: {results.Count}, DB size: {resultsDB.Count}");
            if (resultsDB == null || !resultsDB.FirstOrDefault(new Result()).HasReport())
            {
                report = new Report();
            }
            else
            {
                report = resultsDB.First().MainReport;
                // Merge copies
                MergeResultLists(resultsDB);
            }
            if (report == null)
                return (false, null);

            Debug.Print($"Report ID: {report.ID}");
            foreach (Result res in results)
            {
                res.MainReport = report;
                res.ReportID = report.ID;

                switch (res.Feedback)
                {
                    case "ToCheck":
                        report.ToCheckUsers.Add(res);
                        break;
                    case "Failed":
                        report.FailedUsers.Add(res);
                        break;
                    case "Passed":
                        report.PassedUsers.Add(res);
                        break;
                }
            }
            App.DBConnection.AddResultsToTest(results);
            App.DBConnection.UpdateReport(report);
            return (true,  report);
        }
    }

}
