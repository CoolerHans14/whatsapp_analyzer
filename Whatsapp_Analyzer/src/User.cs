using System;
using System.Collections.Generic;
using System.Transactions;

namespace whatsapp_analytic_tool
{
    public class User
    {
        public int WrittenMessages { get; private set; }
        public int WrittenLettersAverage { get; private set; }
        public List<int> WrittenLetters { get; private set; }
        public string Name { get; private set; }

        public List<Message> Messages { get; private set; }

        public User(string name)
        {
            WrittenLetters = new List<int>();
            Messages = new List<Message>();
            this.Name = name;
        }

        public void NewMessage(string text, string data)
        {
            WrittenLetters.Add(text.Length);
            CalculateAverageLetters();
            WrittenMessages++;
            Messages.Add(new Message(text, new DateTime(Convert.ToInt32("20" + data.Substring(6,2)), Convert.ToInt32(data.Substring(3,2)),Convert.ToInt32(data.Substring(0,2)), Convert.ToInt32(data.Substring(10,2)), Convert.ToInt32(data.Substring(13,2)), 0)));
        }

        private void CalculateAverageLetters()
        {
            var count = 0;
            foreach (var letters in WrittenLetters)
            {
                count += letters;
            }
            WrittenLettersAverage = count / WrittenLetters.Count;
        }

        public override string ToString()
        {
            var temp = "";
            foreach (var message in Messages)
            {
                temp += "\n\t\t"+message.Time + ":" + message.Text;
            }

            return Name + ":\n\tWrittenMessages: " + WrittenMessages.ToString() + "\n\tWritten Average Letters: " +
                   WrittenLettersAverage + "\n\t all Messages: " + temp+"\n-----------------------------------------------------";
        }
    }
}