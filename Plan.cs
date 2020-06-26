using System;
using System.Collections.Generic;

namespace Temporal
{
    internal class Plan
    {
        private const int trailTimes = 50;

        public List<Todo> Todos { get; set; }

        public Plan(List<Todo> todos)
        {
            Todos = todos;
        }

        public static Plan Generate(Activities activities, int hours)
        {
            int[] Weightings = activities.CumulativeWeighting();
            int maxWeight = activities.TotalWeight();
            int failures = 0;
            List<int> todoIDs = new List<int>();
            List<Todo> todos = new List<Todo>();
            int hoursSpent = 0;
            Random rand = new Random();
            while (hoursSpent < hours && failures < trailTimes)
            {
                int p = rand.Next(0, maxWeight);
                int id = 0;
                int trial = activities[id].TimesPerWeek;
                while (trial < p)
                {
                    id++;
                    trial += activities[id].TimesPerWeek;
                }

                if (todoIDs.Contains(id))
                    failures++;
                else
                    todos.Add(activities[id]);
            }

            return new Plan(todos);
        }
    }
}