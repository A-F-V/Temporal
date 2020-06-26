using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temporal
{
    public class Todo
    {
        public String Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int Hours { get; set; }
        public int TimesPerWeek { get; set; }
        public String Details =>
            $"{Start.ToShortDateString()} - {End.ToShortDateString()} requiring {Hours} hrs {TimesPerWeek} times per week.";
        public static Todo Blank()
        {
            return new Todo(){Name="Blank Task"};
        }

        public override string ToString()
        {
            return $"{Name} - ({Details})";
        }
    }
}
