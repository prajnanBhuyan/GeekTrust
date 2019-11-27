using System;

namespace Engine.Interfaces
{
    public interface IGameEngine
    {
        Func<string, bool> CustomInputValidator { get; set; }
        Func<string, ISoutheros, string> CustomInputAction { get; set; }
        void Execute();
        string ProcessInput(string input);
    }
}
