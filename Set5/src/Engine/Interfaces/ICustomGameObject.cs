using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Interfaces
{
    public interface ICustomGameObject
    {
        public bool CustomInputValidator(string input);
        public string CustomInputAction(string input, ISoutheros southeros);
    }
}
