using Malshinon.Checks;
using Malshinon.DAL;
using Malshinon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Malshinon.Controller
{
    public static class Control
    {

        public static string GetUserCode()
        {
            string userCode;
            while(true)
            {
                Console.Write("Please enter your secret code. If you don't have one, create a new one: ");
                userCode = Console.ReadLine()!.Trim();
                if (!String.IsNullOrEmpty(userCode))
                    break;
            }
            return userCode;
        }


        public static string SignUp()
        {
            string userCode;
            while(true)
            {
                Console.Write("\nPlease enter a new secret code: ");
                userCode = Console.ReadLine()!.Trim();

                bool availableCode = Check.SecretCodeAvailable(userCode);
                if (!availableCode)
                    Console.WriteLine("\nThe code is taken, please try again.");

                if (!String.IsNullOrEmpty(userCode) && availableCode)
                    break;

            } 
            Console.WriteLine("\nDone! \n");
            return userCode;
        }


        public static string GetTargetFirstName()
        {
            string firsName;
            while(true)
            {
                Console.Write("Please enter the first name of the terrorist you want to report: ");
                firsName = Console.ReadLine()!.Trim();
                if (!String.IsNullOrEmpty(firsName))
                    break;
            } 
            return firsName;
        }


        public static string GetTargetLastName()
        {
            string LastName;
            while(true)
            {
                Console.Write("Please enter the last name of the terrorist you want to report: ");
                LastName = Console.ReadLine()!.Trim();
                if (!String.IsNullOrEmpty(LastName))
                    break;
            } 
            return LastName;
        }


        public static string GetReporterFirstName()
        {
            string firsName;
            while(true)
            {
                Console.Write("Please enter your first name: ");
                firsName = Console.ReadLine()!.Trim();
                if (!String.IsNullOrEmpty(firsName))
                    break;
            }  
            return firsName;
        }


        public static string GetReporterLastName()
        {
            string LastName;
            while(true)
            {
                Console.Write("Please enter your last name: ");
                LastName = Console.ReadLine()!.Trim();
                if (!String.IsNullOrEmpty(LastName))
                    break;
            } 
            return LastName;
        }


        public static string GetText()
        {
            string text;
            while(true)
            {
                Console.WriteLine("\nEnter the information you have about the terrorist.");
                text = Console.ReadLine()!.Trim();
                if (!String.IsNullOrEmpty(text))
                    break;
            } 
            return text;
        }

        public static IntelReports CreateIntelReports(int reporterId, int targetId, string text)
        {
            IntelReports newIntel = new IntelReports
            {
                ReporterId = reporterId,
                TargetId = targetId,
                Text = text,
                Timestamp = DateTime.Now
            };
            return newIntel;
        }


        public static People CreateNewTarget(string firsNameTatget, string lastNameTarget)
        {
            Random random = new Random();
            string secretCode;
            while (true)
            {
                secretCode = random.Next(1, 101).ToString();
                bool secretCodeAvailable = Check.SecretCodeAvailable(secretCode);
                if (secretCodeAvailable)
                    break;
            }
            People newTarget = new People
            {
                FirstName = firsNameTatget,
                LastName = lastNameTarget,
                SecretCode = secretCode,
                NumMentions = 0,
                Type = "target"
            };
            PeopleDAL.Add(newTarget);
            newTarget = PeopleDAL.GetPeopleBySecretCode(secretCode);

            return newTarget;
        }


        public static People CreateNewReporter(string firsNameReporter, string lastNameReporter, string userCode)
        {
            People newReporter = new People
            {

                FirstName = firsNameReporter,
                LastName = lastNameReporter,
                SecretCode = userCode,
                Type = "reporter"
            };
            PeopleDAL.Add(newReporter);
            newReporter = PeopleDAL.GetPeopleBySecretCode(userCode);
            Console.WriteLine("\nYou have successfully registered in the system.");
            return newReporter;
        }
    }
}

