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


        public static void HeIsDangerousTarget(People target, DateTime timestamp)
        {

            bool threeReports = IntelReportsDAL.ThereIsThreeReports(target.Id, timestamp);
            if (threeReports || target.NumMentions >= 20)
            {
                if (target.Type != "dangerous target and reporter" && target.NumReports > 0)
                    PeopleDAL.UpdateType(target.Id, 7);
                else if(target.Type != "dangerous target" && target.NumReports == 0)
                    PeopleDAL.UpdateType(target.Id, 6);

                Console.WriteLine($"bizzzzzzz bekerful!!! the target: {target.FirstName} {target.LastName} I.D: {target.Id} - is dangerous!!! \n");
            }
        }


        public static void CanBeAgent(People reporter)
        {
            bool potentialTpAgent = false;
            bool canBeAgent = false;
            List<string> texts = IntelReportsDAL.GetTextByReporterId(reporter.Id);
            int countLengthText = 0;
            foreach (string text in texts)
            {
                if (text.Length >= 3)
                    countLengthText++;
            }
            

            if (texts.Count >= 10 && countLengthText >= 100)
                potentialTpAgent = true;


            bool heIsBoth = Check.HeIsBoth(reporter);
            bool heNotAgent = reporter.Type != "potential agent" && reporter.Type != "agent";
            if (heNotAgent && !heIsBoth && potentialTpAgent)
            {
                if (texts.Count >= 20 && countLengthText >= 100)
                    canBeAgent = true;

                if (potentialTpAgent && !canBeAgent)
                {
                    PeopleDAL.UpdateType(reporter.Id, 4);
                    Console.WriteLine($"The reporter {reporter.FirstName} {reporter.LastName} has potential to be an agent \n");
                }
                else if (canBeAgent)
                {
                    PeopleDAL.UpdateType(reporter.Id, 5);
                    Console.WriteLine($"The reporter {reporter.FirstName} {reporter.LastName} became to agent \n");
                }
            }

            
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
