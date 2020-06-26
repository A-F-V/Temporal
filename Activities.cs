using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Temporal
{
    class Activities
    {
        public List<Todo> Todos { get; set; }

        public Activities()
        {
            Todos = new List<Todo>();
        }
        public void AddNew()
        {
            Todos.Add(Todo.Blank());
        }

        public int Count()
        {
            return Todos.Count();
        }

        public void RemoveAt(int dId)
        {
            Todos.RemoveAt(dId);
        }

        public Todo this[int index]
        {
            get { return Todos[index]; }
        }
    }
}
