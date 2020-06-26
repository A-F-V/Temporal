using System;
using System.Collections.Generic;
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
        private readonly string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Temporal");
        private readonly string activities_path;
        private readonly string settings_path;
        private Activities activites;
        private Settings settings;
        private PastelConsole pc = new PastelConsole(ColourPalette.MarineFields);
        public TimeManager()
        {
            activities_path = Path.Combine(folder, "activities.json");
            settings_path = Path.Combine(folder, "settings.json");
        }

        public void Run()
        {
            Load();
            int response;
            do
            {
                Console.Clear();
                string ans = pc.AskListQuestion(new string[] {"Edit Activities", "Edit Settings", "Generate Day Plan", "Exit"});
                if (Int32.TryParse(ans, out response))
                {
                    switch (response)
                    {
                        case 1:
                            PromptEditActivities();
                            break;
                        case 2:
                            PromptEditSettings();
                            break;
                        case 3:
                            PrintAPlan();
                            Console.ReadLine();
                            break;

                    }
                }
            } while (response !=4);
        }

        private void PrintAPlan()
        {
            throw new NotImplementedException();
        }

        private void PromptEditSettings()
        {
            throw new NotImplementedException();
        }

        private void PromptEditActivities()
        {
            Console.Clear();
            int response=0;
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
                                int e_ID = pc.AskIntQuestion("Which activity would you like to edit?");//TODO
                                if (e_ID < 0 || e_ID >= activites.Count())
                                    throw new Exception("Cannot delete that item");
                                else
                                {
                                    ActivitesEdit(e_ID);
                                    Save_Activites();
                                }

                                break;
                            case 2:
                                activites.AddNew();
                                Save_Activites();
                                pc.WriteLine("New blank activity added.");
                                break;
                            case 3:
                                int d_ID = pc.AskIntQuestion("Which activity would you like to delete?");
                                if (d_ID < 0 || d_ID >= activites.Count())
                                    throw new Exception("Cannot delete that item");
                                else
                                {
                                    activites.RemoveAt(d_ID);
                                    Save_Activites();
                                    pc.WriteLine($"{d_ID} removed");
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

        private void ActivitesEdit(int eId)
        {
            Console.Clear();
            Todo cact = activites[eId];
            int response = 0;
            do
            {
                try
                {
                    pc.FormatWriteLine("{2} ({5})",cact.Name,cact.Details);
                    string ans = pc.AskListQuestion(new string[]
                        {"Edit Name", "Edit Start Date", "Edit End Date","Edit Hours Required", "Back"});
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
                                if(newStartDate==".")
                                    cact.Start = DateTime.Today;
                                else
                                {
                                    cact.Start = DateTime.Parse(newStartDate);
                                }

                                break;
                            case 3:
                                pc.WriteLine("Insert the new date.");
                                string newEndDate = pc.ReadAnswer();
                                cact.End = DateTime.Parse(newEndDate);
                                break;
                            case 4:
                                int duration = pc.AskIntQuestion("Insert the number of hours a week you would like to use do this for.");
                                cact.Hours = TimeSpan.FromHours(duration);
                                break;

                        }
                    }
                }
                catch (Exception e)
                {
                    pc.WriteError(e);
                }
            } while (response != 5);
        }

        private void PrintActivities()
        {
            for (int i = 0; i < activites.Count(); i++)
            {
                pc.FormatWriteLine("{3}. {2} ({5})",i.ToString(),activites[i].Name,activites[i].Details);
            }

            if(activites.Count()!=0)
                Console.WriteLine();
        }

        private void Load()
        {
            if (Directory.Exists(folder))
            {
                if (File.Exists(activities_path))
                    activites = FileHandler.Load<Activities>(activities_path);
                else
                    activites = new Activities();
                if (File.Exists(settings_path))
                    settings = FileHandler.Load<Settings>(settings_path);
                else
                    settings = new Settings();
            }
            else
            {
                Directory.CreateDirectory(folder);
            }
        }

        private void Save_Activites()
        {
            File.WriteAllText(activities_path,JsonSerializer.Serialize(activites));
        }
        private void Save_Settings()
        {
            File.WriteAllText(settings_path, JsonSerializer.Serialize(settings));
        }
    }
}
