using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Temporal
{
    class TimeManager
    {
        private readonly string folder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Temporal");

        private readonly string activities_path;
        private Activities activities;
        private PastelConsole pc = new PastelConsole(ColourPalette.MarineFields);

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
                string ans = pc.AskListQuestion(new string[]
                    {"Edit Activities", "Generate Day Plan", "Exit"});
                if (Int32.TryParse(ans, out response))
                {
                    switch (response)
                    {
                        case 1:
                            PromptEditActivities();
                            break; 
                        case 2:
                            PromptPrintAPlan();
                            Console.ReadLine();
                            break;
                    }
                }
            } while (response != 4);
        }

        private void PromptPrintAPlan() //TODO
        {
            //TODO GET SETTINGS

            PrintAPlan(hours);
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
                    string ans = pc.AskListQuestion(new string[]
                        {"Edit Activities", "Add Activity", "Delete Activity", "Back"});
                    if (Int32.TryParse(ans, out response))
                    {
                        switch (response)
                        {
                            case 1:
                                int e_ID = pc.AskIntQuestion("Which activity would you like to edit?");
                                if (e_ID < 0 || e_ID >= activities.Count())
                                    throw new Exception("Cannot delete that item");
                                else
                                {
                                    ActivitiesEdit(e_ID);
                                    Save_Activities();
                                }

                                break;
                            case 2:
                                activities.AddNew();
                                Save_Activities();
                                pc.WriteLine("New blank activity added.");
                                break;
                            case 3:
                                int d_ID = pc.AskIntQuestion("Which activity would you like to delete?");
                                if (d_ID < 0 || d_ID >= activities.Count())
                                    throw new Exception("Cannot delete that item");
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
                    string ans = pc.AskListQuestion(new string[]
                    {
                        "Edit Name", "Edit Start Date", "Edit End Date", "Edit Hours Required", "Edit Times per Week",
                        "Back"
                    });
                    if (Int32.TryParse(ans, out response))
                    {
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
                }
                catch (Exception e)
                {
                    pc.WriteError(e);
                }
            } while (response != 6);
        }

        private void PrintActivities()
        {
            for (int i = 0; i < activities.Count(); i++)
            {
                pc.FormatWriteLine("{3}. {2} ({5})", i.ToString(), activities[i].Name, activities[i].Details);
            }

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