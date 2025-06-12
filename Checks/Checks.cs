using Malshinon.DAL;
using Malshinon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Checks
{
    public static class Check
    {
        public static bool UserExists(string userLogin)
        {
            int id = PeopleDAL.GetIdBySecretCode(userLogin);
            if (id == -1)
                return false;
            else
                return true;
        }



        public static bool TargetExists(string firstName, string lastName)
        {
            int id = PeopleDAL.GetIdByFullName(firstName, lastName);
            if (id == -1)
                return false;
            else
                return true;
        }


        public static Alert CreateAlert(int targetId, string reason)
        {
            Alert alert = new Alert
            {
                TargetId = targetId,
                Reason = reason,
                CreatedAt = DateTime.Now
            };
            AlertDal.Add(alert);
            return alert;
        }


        public static void HeIsDangerousTarget(People target, DateTime timestamp)
        {
            Alert alert;
            bool threeQuickReports = IntelReportsDAL.ThereIsThreeReports(target.Id, timestamp);
            if (threeQuickReports || target.NumMentions >= 20)
            {
                if (target.Type != "dangerous target and reporter" && target.NumReports > 0)
                    PeopleDAL.UpdateType(target.Id, 7);
                else if (target.Type != "dangerous target" && target.NumReports == 0)
                    PeopleDAL.UpdateType(target.Id, 6);
                if (threeQuickReports)
                {
                    string reason = "There are several reports on the target in a short time";
                    alert = CreateAlert(target.Id, reason);
                }
                else if (target.NumMentions >= 20)
                {
                    string reason = "There are many reports on the target.";
                    alert = CreateAlert(target.Id, reason);
                }
            }
            if (target.Type.Contains("dangerous") || threeQuickReports || target.NumMentions >= 20)
                Console.WriteLine($"\nbizzzzzzz bekerful!!! the target: {target.FirstName} {target.LastName} I.D: {target.Id} - is dangerous!!! \n");
        }


        public static void CanBeAgent(People reporter)
        {
            if (!reporter.Type.Contains("agent"))
            {
                List<string> texts = IntelReportsDAL.GetTextByReporterId(reporter.Id);
                int countLengthTexts = 0;
                foreach (string text in texts)
                {
                    countLengthTexts += text.Length;
                }
                double avgLenTexts = countLengthTexts / texts.Count;

                if (texts.Count >= 10 && avgLenTexts >= 100)
                {
                    PeopleDAL.UpdateType(reporter.Id, 4);
                    Console.WriteLine($"\nThe reporter {reporter.FirstName} {reporter.LastName} has the potential to become an agent \n");
                }
            }
            else
                Console.WriteLine($"\nThe reporter {reporter.FirstName} {reporter.LastName} has the potential to become an agent \n");
        }


        public static bool SecretCodeAvailable(string secetCode)
        {
            int availableCode = PeopleDAL.GetIdBySecretCode(secetCode);
            if (availableCode == -1)
                return true;
            else
                return false;
        }


        public static bool HeIsBoth(People people)
        {
            if (people.NumReports > 0 && people.NumMentions > 0)
            {
                PeopleDAL.UpdateType(people.Id, 3);
                return true;
            }
            else
                return false;
        }


        public static void HeIsReporterTo(People target)
        {
            bool heNotDangerous = target.Type != "dangerous target and reporter" && target.Type != "dangerous target";
            if (heNotDangerous && target.NumReports > 0)
                PeopleDAL.UpdateType(target.Id, 3);
        }
    }
}
