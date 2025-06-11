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
            bool isValid = false;
            do
            {
                Console.Write("Please enter your secret code. If you don't have one, create a new one: ");
                userCode = Console.ReadLine()!;
                if (userCode != null)
                    isValid = true;
            } while (!isValid);
            return userCode;
        }


        public static string SignUp()
        {
            string userCode;
            bool isValid = false;
            do
            {
                Console.Write("Please enter a new secret code: ");
                userCode = Console.ReadLine()!;

                bool availableCode = Check.SecretCodeAvailable(userCode);
                if (!availableCode)
                    Console.WriteLine("The code is taken, please try again. \n");

                if (userCode != null && availableCode)
                    isValid = true;

            } while (!isValid);
            Console.WriteLine("Done! \n");
            return userCode;
        }


        public static string GetTargetFirstName()
        {
            bool isValid = false;
            string firsName;
            do
            {
                Console.Write("Please enter the first name of the terrorist you want to report: ");
                firsName = Console.ReadLine()!;
                if (firsName != null)
                    isValid = true;
            } while (!isValid);
            return firsName;
        }


        public static string GetTargetLastName()
        {
            bool isValid = false;
            string LastName;
            do
            {
                Console.Write("Please enter the last name of the terrorist you want to report: ");
                LastName = Console.ReadLine()!;
                if (LastName != null)
                    isValid = true;
            } while (!isValid);
            return LastName;
        }


        public static string GetReporterFirstName()
        {
            bool isValid = false;
            string firsName;
            do
            {
                Console.Write("Please enter your first name: ");
                firsName = Console.ReadLine()!;
                if (firsName != null)
                    isValid = true;
            } while (!isValid);
            return firsName;
        }


        public static string GetReporterLastName()
        {
            bool isValid = false;
            string LastName;
            do
            {
                Console.Write("Please enter your last name: ");
                LastName = Console.ReadLine()!;
                if (LastName != null)
                    isValid = true;
            } while (!isValid);
            return LastName;
        }


        public static string GetText()
        {
            bool isValid = false;
            string text;
            do
            {
                isValid = false;
                Console.WriteLine("\nEnter the information you have about the terrorist.");
                text = Console.ReadLine()!;
                if (text != null)
                    isValid = true;
            } while (!isValid);
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

