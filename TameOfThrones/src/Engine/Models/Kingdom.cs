using System.Collections.Generic;

namespace Engine
{
    public class Kingdom
    {
        public readonly Kingdoms Name;
        public readonly string Emblem;

        public string King;
        public bool IsCompeting { get; set; }
        public List<Kingdoms> Allies { get; }

        #region Constructor(s)
        public Kingdom(Kingdoms kingdom, string emblem)
        {
            Name = kingdom;
            Emblem = emblem;
            Allies = new List<Kingdoms>();
        }
        #endregion

        /// <summary>
        /// Checks if the kingdom's favirote animal has been sneakily added into the message.
        /// </summary>
        /// <param name="message">The message being sent to create an alliance.</param>
        /// <returns></returns>
        private bool IsPleasing(string message)
        {
            var isValid = message.Length >= Emblem.Length;

            for (int i = 0; isValid && i < Emblem.Length; i++)
            {
                int index;

                if ((index = message.IndexOf(Emblem[i])) != -1)
                    message = message.Remove(index, 1);
                else
                    isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// If acceptable, adds the kingdom that sent the message to the list of Allies
        /// </summary>
        /// <param name="message">The secret message to form an alliance</param>
        /// <returns></returns>
        public bool AccepAlliance(Message message)
        {
            var accept = false;

            // Only accept an alliance if:
            //  1. The recieving kingdom itself is not competing
            //  2. It is not already allied with another kingdom
            //  3. If the message contains the emblem animal of the recieving kingdom
            if (!IsCompeting &&
                Allies.Count == 0 &&
                IsPleasing(message.Text))
            {
                Allies.Add(message.Sender);
                accept = true;
            }

            return accept;
        }

        /// <summary>
        /// Sends a message to another kingdom in an attempt to form an alliance.
        /// </summary>
        /// <param name="recipientKingdom">The kingdom the message is sent to</param>
        /// <param name="message">The message to be sent</param>
        /// <remarks>If the alliance request is accepted, the kingdom is added to the list of allies</remarks>
        public void SendMessage(Kingdom recipientKingdom, Message message)
        {
            // Check if:
            //  1. The kingdom is not sending a message to itself
            //  2. The kingdom is already an ally
            //  3. If not, see if they accept the alliance
            if (Name != recipientKingdom.Name &&
                !Allies.Contains(recipientKingdom.Name) &&
                recipientKingdom.AccepAlliance(message))
            {
                // If the recieving kingdom accepts the alliance add them to the Allies list
                Allies.Add(message.Recipient);
            }
        }

        /// <summary>
        /// Break all existing alliances.
        /// </summary>
        public void BreakAlliances()
        {
            Allies.Clear();
        }
    }
}
