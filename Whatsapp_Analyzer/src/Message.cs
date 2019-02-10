using System;

namespace whatsapp_analytic_tool
{
    public class Message
    {
        public string Text { get; private set; }
        public DateTime Time { get; private set; }
        public int WrittenLetters { get; private set; }

        public Message(string text, DateTime time)
        {
            Text = text;
            Time = time;
            WrittenLetters = Text.Length;
        }
    }
}