using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Traffic.Tests
{
    internal class TestConsoleMethods : IConsoleMethods
    {
        public string SimulatedInput { private get; set; }
        public string SimulatedOutput { get; private set; }

        public void WriteLine(string message)
        {
            SimulatedOutput = message;
        }

        public string ReadLine()
        {
            return SimulatedInput;
        }
    }
}
