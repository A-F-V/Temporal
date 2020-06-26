using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Temporal
{
    internal class TimeManager
    {
        private readonly string activities_path;

        private readonly string folder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Temporal");

        private Activities activities;
        private readonly PastelConsole pc = new PastelConsole(ColourPalette.MarineFields);

        public TimeManager()
        {
            activities_path = Path.Combine(folder, "activities.json");
        }

        public void Run()
        {
            Load();
            int response;
            do
            {
                Console.Clear();
                string ans = pc.AskListQuestion(new[]
                    {"Edit Activities", "Generate Day Plan", "Exit"});
                if (int.TryParse(ans, out response))
                    switch (response)
                    {
                        case 1:
                            PromptEditActivities();
                            break;
                        case 2:
                            PromptPrintAPlan();
                            break;
                    }
            } while (response != 4);
        }

        private void PromptPrintAPlan()
        {
            if (activities.Count() == 0)
            {
                pc.WriteLine("No activities to make a plan from.", -3);
            }
            else
            {
                Console.Clear();
                try
                {
                    int minHoursAvailable = pc.AskIntQuestion("How many hours are available today?");
                    PrintAPlan(minHoursAvailable);
                    Console.ReadLine();
                }
                catch (Exception e)
                {
                    pc.WriteError(e);
                    Console.ReadLine();
                }
            }
        }

        private void PrintAPlan(int hours)
        {
            Plan plan = Plan.Generate(activities, hours);
            pc.FormatWriteLine("The plan for {-3} is:", DateTime.Now.ToShortDateString());
            foreach (Todo act in plan)
            {
                pc.FormatWriteLine("{4} for {0} hrs",act.Name,act.Hours);
            }
        }

        private void PromptEditActivities()
        {
            Console.Clear();
            int response = 0;
            do
            {
                try
                {
                    PrintActivities();
                    string ans = pc.AskListQuestion(new[]
                        {"Edit Activities", "Add Activity", "Delete Activity", "Back"});
                    if (int.TryParse(ans, out response))
                        switch (response)
                        {
                            case 1:
                                int e_ID = pc.AskIntQuestion("Which activity would you like to edit?");
                                if (e_ID < 0 || e_ID >= activities.Count())
                                {
                                    throw new Exception("Cannot delete that item");
                                }
                                else
                                {
                                    ActivitiesEdit(e_ID);
                                    Save_Activities();
                                }

                                break;
                            case 2:
                                string name = pc.AskQuestion("What is the name of the new activity?");
                                int hours = pc.AskIntQuestion(
                                    "How many hours a day would you spend doing this activity?");
                                int timesAWeek =
                                    pc.AskIntQuestion("How many times a week would you like to do this activity?");
                                activities.AddNew(name,hours,timesAWeek);
                                Save_Activities();
                                pc.FormatWriteLine("{-3} added.", name);
                                break;
                            case 3:
                                int d_ID = pc.AskIntQuestion("Which activity would you like to delete?");
                                if (d_ID < 0 || d_ID >= activities.Count())
                                {
                                    throw new Exception("Cannot delete that item");
                                }
                                else
                                {
                                    string nameToDelete = activities[d_ID].Name;
                                    activities.RemoveAt(d_ID);
                                    Save_Activities();
                                    pc.WriteLine($"{nameToDelete} removed");
                                }

                                break;
                        }
                }
                catch (Exception e)
                {
                    pc.WriteError(e);
                }
            } while (response != 4);
        }

        private void ActivitiesEdit(int eId)
        {
            Console.Clear();
            Todo cact = activities[eId];
            int response = 0;
            do
            {
                try
                {
                    pc.FormatWriteLine("{2} ({5})", cact.Name, cact.Details);
                    string ans = pc.AskListQuestion(new[]
                    {
                        "Edit Name", "Edit Start Date", "Edit End Date", "Edit Hours Required", "Edit Times per Week",
                        "Back"
                    });
                    if (int.TryParse(ans, out response))
                        switch (response)
                        {
                            case 1:
                                pc.WriteLine("Insert the new name.");
                                string newName = pc.ReadAnswer();
                                cact.Name = newName;
                                break;
                            case 2:
                                pc.WriteLine("Insert the new date (Put '.' if today).");
                                string newStartDate = pc.ReadAnswer();
                                cact.Start = newStartDate == "." ? DateTime.Today : DateTime.Parse(newStartDate);
                                break;
                            case 3:
                                pc.WriteLine("Insert the new date.");
                                string newEndDate = pc.ReadAnswer();
                                cact.End = DateTime.Parse(newEndDate);
                                break;
                            case 4:
                                int duration =
                                    pc.AskIntQuestion(
                                        "Insert the number of hours a week you would like to use do this for.");
                                cact.Hours = duration;
                                break;
                            case 5:
                                int repeats =
                                    pc.AskIntQuestion("Insert the number of times a week you want to do this.");
                                cact.TimesPerWeek = repeats;
                                break;
                        }
                }
                catch (Exception e)
                {
                    pc.WriteError(e);
                }
            } while (response != 6);
        }

        private void PrintActivities()
        {
            if (activities.Count() != 0)
                Console.WriteLine();

            for (int i = 0; i < activities.Count(); i++)
                pc.FormatWriteLine("{3}. {2} ({5})", i.ToString(), activities[i].Name, activities[i].Details);

            if (activities.Count() != 0)
                Console.WriteLine();
        }

        private void Load()
        {
            if (Directory.Exists(folder))
            {
                if (File.Exists(activities_path))
                    activities = FileHandler.Load<Activities>(activities_path);
                else
                    activities = new Activities();
            }
            else
            {
                Directory.CreateDirectory(folder);
            }
        }

        private void Save_Activities()
        {
            File.WriteAllText(activities_path, JsonSerializer.Serialize(activities));
        }
    }
}