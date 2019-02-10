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
                if (
                    text[0].Contains(
                        "hat eine neue Telefonnummer. Tippe, um eine Nachricht zu schreiben oder die neue Nummer hinzuzufügen.") ||
                    text[0].Contains("hinzugefügt.") || text[0].Contains("hat die Gruppenbeschreibung geändert.") ||
                    text[0].Contains("entfernt.") ||
                    text[0].Contains("ist dieser Gruppe mit dem Einladungslink beigetreten.") ||
                    text[0].Contains("hat die Gruppe verlassen.") || text[0].Contains("gewechselt.") ||
                    text[0].Contains(
                        "hat in den Gruppen-Einstellungen festgelegt, dass nur Admins die Gruppeninfo bearbeiten können.") ||
                    text[0].Contains("hat das Gruppenbild geändert.") || text[0]
                        .Contains(
                            "Nachrichten an diese Gruppe sind jetzt mit Ende-zu-Ende-Verschlüsselung geschützt. Tippe für mehr Infos.")
                ) continue;                                                                                                                            //for german Whatsapp.
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

        public void WriteAnalyze(string path)
        {
            var sw = new StreamWriter(path);
            var userStatistics = "";
            var userNames = "";
            foreach (var user in Users)
            {
                userStatistics += "\n" + user.ToString();
                userNames += user.Name + ", " + "\n";
            }
            sw.Write("Names:\n" + userNames + "\n\n" + userStatistics);
            sw.Close();
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
            var userStatistics = "";
            var userNames = "";
            foreach (var user in Users)
            {
                userStatistics += "\n" + user.ToString();
                userNames += user.Name + ", " + "\n";
            }

            return "Names:\n" + userNames + "\n\n" + userStatistics;
        }
    }
}