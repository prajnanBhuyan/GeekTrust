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
        private readonly string rulerName;
        private readonly Kingdoms kingdomType;

        public string Name { get { return kingdomType.ToString(); } }
        public string Ruler { get { return string.IsNullOrWhiteSpace(rulerName) ? "ruler" : rulerName; } }
        public List<Kingdoms> Allies { get; }
        public string Emblem { get; }
        public bool HasRightToRule { get { return Allies.Count >= 3; } }
        public bool IsKingNameKnown { get { return !string.IsNullOrWhiteSpace(rulerName); } }

        public Kingdom(Kingdoms kingdom)
        {
            this.kingdomType = kingdom;
            Emblem = Engine.Emblems[(int)kingdom];
            Allies = new List<Kingdoms>();
        }

        public Kingdom(Kingdoms kingdom, string rulerName)
        {
            this.kingdomType = kingdom;
            this.rulerName = rulerName;
            Emblem = Engine.Emblems[(int)kingdom];
            Allies = new List<Kingdoms>();
        }

        public void SendMessage(Kingdoms targetKingdom, string secretMessage)
        {
            if (!Allies.Contains(targetKingdom) &&
                Engine.IsValidSecretMessage(targetKingdom, secretMessage))
                Allies.Add(targetKingdom);
        }
    }
}
