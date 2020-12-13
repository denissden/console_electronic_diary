using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public struct ChoiceMapElement
    {
        public dynamic Element { get; set; }
        public string State { get; set; }
        public string InitState { get; }
        public bool Changed { get; set; }

        public ChoiceMapElement(dynamic element, string state, string init_state)
        {
            Element = element;
            State = state;
            InitState = init_state;
            Changed = false;
        }

    }
}
