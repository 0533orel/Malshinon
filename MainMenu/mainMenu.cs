using Malshinon.Checks;
using Malshinon.Checks;
using Malshinon.Controller;
using Malshinon.DAL;
using Malshinon.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Menu
{
    public static class mainMenu
    {
        public static void Menu()
        {
            Console.WriteLine("Welcome to malshinon! \n");
            bool exit = false;
            do
            {
                Console.WriteLine("Please choose what you want to do? \n" +
                "1. Log in and report \n" +
                "2. Sign up \n" +
                "3. Exit \n");
                string userSelection = Console.ReadLine().Trim();

                switch (userSelection)
                {
                    case "1":
                        {
                            string userCode = Control.GetUserCode();
                            People reporter = PeopleDAL.GetPeopleBySecretCode(userCode);
                            if (reporter == null)
                            {
                                Console.WriteLine("\nYou not recognized in the system. You must register first. \n");
                                string firsNameReporter = Control.GetReporterFirstName();
                                string lastNameReporter = Control.GetReporterLastName();
                                reporter = Control.CreateNewReporter(firsNameReporter, lastNameReporter, userCode);
                            }
                            Console.WriteLine($"\nWelcome {reporter.FirstName} {reporter.LastName}! \n");

                            string firsNameTarget = Control.GetTargetFirstName();
                            string lastNameTarget = Control.GetTargetLastName();
                            string text = Control.GetText();

                            People target = PeopleDAL.GetPeopleByFullName(firsNameTarget, lastNameTarget);
                            if (target == null)
                                target = Control.CreateNewTarget(firsNameTarget, lastNameTarget);

                            IntelReports newIntel = Control.CreateIntelReports(reporter.Id, target.Id, text);
                            IntelReportsDAL.Add(newIntel);

                            PeopleDAL.UpdateNumMentions(target.Id);

                            Check.HeIsReporterTo(target);

                            Check.HeIsDangerousTarget(target, newIntel.Timestamp);

                            PeopleDAL.UpdateNumReports(reporter.Id);

                            Check.CanBeAgent(reporter);

                            Console.WriteLine("\nThe report was successfully received. \n");
                            break;
                        }
                    case "2":
                        {
                            string userCode = Control.SignUp();
                            string firsNameReporter = Control.GetReporterFirstName();
                            string lastNameReporter = Control.GetReporterLastName();
                            People newReporter = Control.CreateNewReporter(firsNameReporter, lastNameReporter, userCode);
                            break;
                        }
                    case "3":
                        {
                            exit = true;
                            Console.WriteLine("Have a nice day.");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Incorrect key, please try again. \n");
                            break;
                        }
                }
            } while (!exit);


        }
    }
}
