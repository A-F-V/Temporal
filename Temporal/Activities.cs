using System.Collections.Generic;
using System.Linq;

namespace Temporal
{
    internal class Activities
    {
        public List<Todo> Todos { get; set; }

        public Todo this[int index] => Todos[index];

        public Activities()
        {
            Todos = new List<Todo>();
        }

        public void AddNew(string name = "Blank", float hours = 0, int times = 0)
        {
            Todos.Add(new Todo(name,hours,times));
        }

        public int Count()
        {
            return Todos.Count();
        }

        public void RemoveAt(int dId)
        {
            Todos.RemoveAt(dId);
        }

        public int[] CumulativeWeighting()
        {
            int[] output = new int[Todos.Count];
            for (int i = 0; i < Todos.Count; i++)
                if (i == 0)
                    output[i] = Todos[i].TimesPerWeek;
                else
                    output[i] = output[i - 1] + Todos[i].TimesPerWeek;

            return output;
        }

        public int TotalWeight()
        {
            return Todos.Sum(x => x.TimesPerWeek);
        }
    }
}