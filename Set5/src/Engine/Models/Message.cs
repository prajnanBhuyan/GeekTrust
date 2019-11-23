using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public class Message
    {
        public readonly Kingdoms Sender;
        public readonly Kingdoms Recipient;
        public readonly string Text;

        public Message(Kingdoms sender, Kingdoms recipient, string secretMessage)
        {
            Sender = sender;
            Recipient = recipient;
            Text = secretMessage;
        }
    }
}
