using Engine;
using Engine.Interfaces;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TameOfThrones.Tests
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

    internal class MultiIOConsoleMethods : IConsoleMethods
    {
        private int counter;
        public List<string> SimulatedMultipleInputs;

        public string SimulatedOutput { get; private set; }

        public MultiIOConsoleMethods()
        {
            counter = 0;
            SimulatedMultipleInputs = new List<string>();
        }

        public void WriteLine(string message)
        {
            SimulatedOutput = message;
        }

        public string ReadLine()
        {
            return counter < SimulatedMultipleInputs.Count ? SimulatedMultipleInputs[counter++] : string.Empty;
        }
    }
}