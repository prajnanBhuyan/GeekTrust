using System.Collections.Generic;

namespace Engine.Interfaces
{
    public interface ISoutheros
    {
        Dictionary<Kingdoms, Kingdom> AllKingdoms { get; }
        Kingdom RulingKingdom { get; set; }
    }
}