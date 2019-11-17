using System;
using System.Collections.Generic;

namespace Engine
{

    // Consider to be the High Priest
    public class Engine
    {
        public static readonly string[] Emblems =
        {
            "gorilla",
            "panda",
            "octopus",
            "mammoth",
            "owl",
            "dragon"
        };

        private Kingdom rulingKingdom;
        private Kingdom kingdomSendingMessages;
        private readonly Dictionary<Kingdoms, Kingdom> competingKingdoms;

        public const string InvalidKingdomErrorMessage = "Our messengers couldn't reach '{0}'. Maybe we should try sending messaged just to our neighbouring kingdoms for now.";

        public Engine()
        {
            competingKingdoms = new Dictionary<Kingdoms, Kingdom>();
        }

        public string ProcessInput(string input)
        {
            string output = "Invalid Input";
            int index;

            if (input == "who is the ruler of southeros?")
            {
                output = FindRuler();
            }

            else if (input.Contains("allies of"))
            {
                output = FindAlliesOfRuler();
            }

            else if ((index = input.IndexOf('"')) != -1)
            {
                var recipient = input.Substring(0, index).Trim(new char[] { ' ', ',' });
                var messageText = input.Substring(index + 1, input.LastIndexOf('"') - index - 1);

                if (Enum.TryParse(recipient, true, out Kingdoms recipientKingdom))
                {
                    kingdomSendingMessages.SendMessage(recipientKingdom, messageText);

                    if (kingdomSendingMessages.HasRightToRule)
                        rulingKingdom = kingdomSendingMessages;

                    output = string.Empty;
                }
                else
                {
                    output = string.Format(InvalidKingdomErrorMessage, recipient);
                }
            }


            return output;
        }

        private void SendMessage(Message message)
        {
            // Only send a message if the Recipient is not contending for the throne
            if (!competingKingdoms.ContainsKey(message.Recipient))
                competingKingdoms[message.Sender].SendMessage(message.Recipient, message.SecretMessage);
        }

        private string FindRuler()
        {
            var ruler = "None";

            if (rulingKingdom != null)
                ruler = rulingKingdom.IsKingNameKnown ? rulingKingdom.Ruler : rulingKingdom.Name;

            return ruler;
        }

        private string FindAlliesOfRuler()
        {
            var allies = "None";

            if (rulingKingdom != null)
                allies = string.Join(", ", rulingKingdom.Allies);

            return allies;
        }

        public void AddCompetingKingdom(Kingdoms kingdom, string rulerName = null)
        {
            competingKingdoms.Add(kingdom, new Kingdom(kingdom, rulerName));
            kingdomSendingMessages = competingKingdoms[kingdom];
        }

        public void ChangeActiveKingdomTo(Kingdoms kingdom)
        {
            if (competingKingdoms.ContainsKey(kingdom))
                kingdomSendingMessages = competingKingdoms[kingdom];
            else
                throw new ArgumentException($"{kingdom} is not competing for the throne.", "kingdom");
        }

        public static bool IsValidSecretMessage(Kingdoms kingdom, string message)
        {

            var emblem = Emblems[(int)kingdom];

            var isValid = message.Length >= emblem.Length;

            for (int i = 0; isValid && i < emblem.Length; i++)
            {
                int index;

                if ((index = message.IndexOf(emblem[i])) != -1)
                    message = message.Remove(index, 1);
                else
                    isValid = false;
            }

            return isValid;
        }
    }
}
