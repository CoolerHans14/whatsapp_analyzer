using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace whatsapp_analytic_tool
{
    public class Analyzer
    {
        public int Messages { get; private set; }
        private List<string> messagesList;
        private List<User> Users;
        private List<string> nameList;

        public Analyzer(string path)
        {
            messagesList = new List<string>();
            Users = new List<User>();
            nameList = new List<string>();
            ReadChat(path);
            Analyz();
        }

        private void Analyz()
        {
            foreach (var message in messagesList)
            {
                var text = message.Substring(17).Split(":");
                if (!nameList.Contains(text[0]))
                {
                    Users.Add(new User(text[0]));
                    nameList.Add(text[0]);
                }

                var tempText = "";
                for (var i = 1; i < text.Length; i++)
                {
                    tempText += text[i];
                }

                foreach (var user in Users)
                {
                    if (user.Name != text[0]) continue;
                    user.NewMessage(tempText, message.Substring(0, 15));
                    break;
                }
            }
        }

        private void WriteAnalyze()
        {
        }

        private void ReadChat(string path)
        {
            var sr = new StreamReader(path);
            string chatLine;
            while ((chatLine = sr.ReadLine()) != null)
            {
                if (chatLine.Length <= 0) continue;
                var isParse = true;
                if (chatLine.Length > 14)
                {
                    for (var i = 0; i <= 14; i++)
                    {
                        switch (i)
                        {
                            case 2:
                            case 5:
                            case 12:
                                i++;
                                continue;
                            case 8:
                                i += 2;
                                continue;
                        }

                        if (!int.TryParse(chatLine[i].ToString(), out var temp))
                        {
                            isParse = false;
                        }
                    }
                }
                else
                {
                    isParse = false;
                }


                if (!isParse)
                {
                    messagesList[messagesList.Count - 1] += " " + chatLine;
                }
                else
                {
                    messagesList.Add(chatLine);
                }
            }

            sr.Close();
        }

        public override string ToString()
        {
            var temp = "";
            foreach (var u in Users)
            {
                temp += "\n" + u.ToString();
            }

            return temp;
        }
    }
}