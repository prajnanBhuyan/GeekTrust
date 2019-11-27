using Engine.Interfaces;
using System;

namespace Engine.Factory
{
    public class AGoldenCrown : ICustomGameObject
    {
        // Custom Input Parser for the 'A Golden Crown' problem
        public bool CustomInputValidator(string input)
        {
            var index = input.IndexOf('"');
            var lastIndex = input.LastIndexOf('"');
            // We expect an input that contains:
            //  1. The name of the kingdom it is addressed to
            //  2. The message enclosed in inverted commas
            //  Example:
            //      a. air, "send owl"      Kingdom: air    Message: send owl
            //      b. air"send"~"owl"      Kingdom: air    Message: send"~"owl
            return index > 0 &&
                    lastIndex > index + 1;

        }

        // Custom Input Action for the 'A Golden Crown' problem
        public string CustomInputAction(string input, ISoutheros southeros)
        {
            // Declare local variables
            var output = string.Empty;
            // The only competing kingdom as per the problem is the SpaceKingdom (this can 
            // be changed here any time)
            var competingKingdom = Kingdoms.Space;
            // Required right to rule is the number of kingdoms the competingKingdom kingdom
            // needs to be allied with in order to be crowned as the ruler
            var reqRightToRule = 3;
            // Give the gorilla king of the Space kingdom a name
            southeros.AllKingdoms[Kingdoms.Space].King = "King Shan";

            // Try parsing the input into target kingdom and message
            var index = input.IndexOf('"');
            var recipient = input.Substring(0, index).Trim(new char[] { ' ', ',' });
            var messageText = input.Substring(index + 1, input.LastIndexOf('"') - index - 1);

            // Try parsing the target kingdom to verify target is valid
            if (Enum.TryParse(recipient, true, out Kingdoms recipientKingdom))
            {
                // Create message object
                var message = new Message(competingKingdom, recipientKingdom, messageText);

                // Send out message
                southeros.AllKingdoms[competingKingdom].SendMessage(southeros.AllKingdoms[recipientKingdom], message);

                // Check if the competeing kingdom now has the required number of allies
                if (southeros.AllKingdoms[competingKingdom].Allies.Count >= reqRightToRule)
                    // If so, set the value for the ruling kingdom
                    southeros.RulingKingdom = southeros.AllKingdoms[competingKingdom];
            }
            // If target is invalid, return appropriate error message
            else
            {
                output = string.Format(Utility.InvalidKingdomMessage, recipient);
            }

            return output;
        }
    }
}
