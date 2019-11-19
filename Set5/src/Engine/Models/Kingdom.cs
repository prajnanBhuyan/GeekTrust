using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public enum Kingdoms
    {
        Space = 0,
        Land = 1,
        Water = 2,
        Ice = 3,
        Air = 4,
        Fire = 5,
        None
    };

    public class Kingdom
    {
        private readonly Kingdoms kingdomType;

        public string King;
        public readonly string Emblem;

        public string Name { get { return kingdomType.ToString(); } }
        public bool IsCompeting { get; set; }
        public List<Kingdoms> Allies { get; }

        public Kingdom(Kingdoms kingdom)
        {
            this.kingdomType = kingdom;
            Emblem = Engine.Emblems[(int)kingdom];
            Allies = new List<Kingdoms>();
        }

        public Kingdom(Kingdoms kingdom, string rulerName)
        {
            this.kingdomType = kingdom;
            King = rulerName;
            Emblem = Engine.Emblems[(int)kingdom];
            Allies = new List<Kingdoms>();
        }
    }
}
