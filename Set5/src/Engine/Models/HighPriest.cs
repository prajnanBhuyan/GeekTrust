using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    class HighPriest
    {
        int messagesToChoose;

        public HighPriest(int messagesToChoose)
        {
            this.messagesToChoose = messagesToChoose;
        }

        public List<Message> PickRandomMessages(BallotBox ballotBox)
        {
            // In some cases the number of alliance requests might be less 
            // than the number of messages the high priest intended to select
            messagesToChoose = Math.Min(ballotBox.Count, messagesToChoose);

            var choosenMessages = new List<Message>();

            // The High Priest of Southeros chooses the messages to send out
            for (int i = 0; i < messagesToChoose; i++)            
                choosenMessages.Add(ballotBox.PickMessage());

            return choosenMessages;
        }

        public void HandOverMessages()
        { 
        
        }
    }
}
