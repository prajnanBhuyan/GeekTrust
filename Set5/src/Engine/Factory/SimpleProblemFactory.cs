using Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Factory
{
    public class SimpleProblemFactory
    {
        public ICustomGameObject GetProblem(string problem, IConsoleMethods consoleMethods)
        {
            ICustomGameObject customGameObject;

            if (problem == "problem1")
                customGameObject = new AGoldenCrown();
            else if (problem == "problem2")
                customGameObject = new BreakerOfChains(consoleMethods);
            else
                throw new ArgumentException($"{problem} is not a valid Problem to execute.", nameof(problem));

            return customGameObject;
        }
    }
}
