using System;

namespace Temporal
{
    public class Todo
    {
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Hours { get; set; }
        public int TimesPerWeek { get; set; }

        public string Details =>
            $"requiring {Hours} hrs {TimesPerWeek} times per week.";

        public Todo()
        {
        }
        public Todo(string name, int hours=0, int times=0)
        {
            Name = name;
            Hours = hours;
            TimesPerWeek = times;
        }
        public static Todo Blank()
        {
            return new Todo("Blank");
        }

        public override string ToString()
        {
            return $"{Name} - ({Details})";
        }
    }
}