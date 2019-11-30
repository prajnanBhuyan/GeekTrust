namespace Engine
{
    // Changed Message to struct from Class to avoid an Anemic Domain Model
    public struct Message
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
