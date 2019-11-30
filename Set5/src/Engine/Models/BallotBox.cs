using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Engine.Models
{
    class BallotBox
    {
        private List<Message> messages;

        public int Count { get { return messages.Count; } }

        public BallotBox()
        {
            messages = new List<Message>();
        }

        /// <summary>
        /// Drop a message into the ballot box
        /// </summary>
        /// <param name="message">The message to be dropped into the ballot box</param>
        public void PutInMessage(Message message)
        {
            messages.Add(message);
        }

        /// <summary>
        /// Pick a message from the ballot box.
        /// </summary>
        /// <returns>Returns a random message from the ballot box.</returns>
        public Message PickMessage()
        {
            // Randomizer to choose which messages gets picked
            var rnd = new Random();

            // Randomly choose message index
            var messageIndex = rnd.Next(Count);

            // Randomly choosen message
            var message = messages[messageIndex];

            // Remove message from list so that it is not sent out a second time
            messages.RemoveAt(messageIndex);

            // Return message
            return message;
        }
    }
}
