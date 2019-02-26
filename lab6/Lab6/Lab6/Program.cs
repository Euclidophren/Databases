using System;
using System.Xml;
using System.IO;

namespace Lab6
{
    class Program
    {

        static public int DisplayMenu()
        {
            Console.WriteLine("Работа с Xml-файлом");
            Console.WriteLine();
            Console.WriteLine("1. Поиск информации, содержащейся в документе");
            Console.WriteLine("2. Доступ к содержимому узлов");
            Console.WriteLine("3. Изменение документа");
            Console.WriteLine("4. Выход из программы");
            Console.WriteLine("Выберите опцию: ");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }
        static void Main(string[] args)
        {
            //int choice = DisplayMenu();
            // 1. Reading document
            XmlDocument inputXML = new XmlDocument();
            inputXML.Load("questions.xml");
            while(true)
            {
                int choice = DisplayMenu();
                switch (choice)
                {
                    case 1:
                        //2. Finding information 
                        Console.Write("This question contents were found in the document:\r\n");
                        XmlNodeList contents = inputXML.GetElementsByTagName("Content");
                        for (int i = 0; i < contents.Count; i++)
                            Console.Write(contents[i].ChildNodes[0].Value + "\r\n");

                        Console.Write("\n\nThis question has id = 18:\r\n");
                        XmlElement questionID = inputXML.GetElementById("18");
                        //Console.WriteLine(questionID.OuterXml);
                        Console.Write(questionID.ChildNodes[0].ChildNodes[0].Value + "\t");
                        Console.Write(questionID.ChildNodes[1].ChildNodes[0].Value + "\r\n");

                        Console.Write("\n\nQuestions with answer 'magnis dis' are:\r\n");
                        XmlNodeList answer = inputXML.SelectNodes("//Question/Content/text()[../../Answer/text()='magnis dis']");
                        for (int i = 0; i < answer.Count; i++)
                            Console.Write(answer[i].Value + "\r\n");

                        Console.Write("\n\n First question with answer 'magnis dis' is:\r\n");
                        XmlNode firstAnswer = inputXML.SelectSingleNode("//Question/Content/text()[../../Answer/text()='magnis dis']");
                        Console.Write(firstAnswer.Value + "\r\n");

                        break;
                    case 2:
                        // 3. Access to nodes
                        Console.Write("\n" + inputXML.DocumentElement.InnerXml + "\r\n");

                        Console.Write("\n\nInformation about questions: \n");
                        XmlNodeList question = inputXML.GetElementsByTagName("Question");
                        for (int i = 0; i < question.Count; i++)
                        {
                            Console.Write(question[i].ChildNodes[0].InnerText + "\t" + question[i].ChildNodes[3].Value + "\r\n");
                            if(question[i].ChildNodes[1].Value ==  question[i].ChildNodes[3].Value)
                            {
                               Console.WriteLine("\nAnswer matching comment\n");
                               Console.WriteLine(question[i].ChildNodes[0].InnerText + "\r\n");
                            }
                        }

                        Console.WriteLine("\n\nAnswer matching comment\n");
                        Console.WriteLine(question[1].ChildNodes[0].InnerText + "\r\n");
                        if (inputXML.FirstChild is XmlProcessingInstruction)
                        {
                            XmlProcessingInstruction processInfo = (XmlProcessingInstruction)inputXML.FirstChild;
                            Console.WriteLine(processInfo.Data);
                            Console.WriteLine(processInfo.Name);
                        }

                        Console.Write("\n\nIDs of questions: \n");
                        for (int i = 0; i < question.Count; i++)
                            Console.Write(question[i].ChildNodes[0].InnerText + " : " + question[i].Attributes[0].Value + "\r\n");

                        break;
                    case 3:
                        //3. Changes file
                        XmlNodeList change = inputXML.GetElementsByTagName("Question");
                        XmlElement pcElement = (XmlElement)inputXML.GetElementsByTagName("Content")[1];
                        change[1].RemoveChild(pcElement);
                        Console.Write("\n Delete the second questions's content..." + "\r\n");
                        inputXML.Save("questions-deleted.xml");

                        XmlNodeList contentValues = inputXML.SelectNodes("//Question/Content/text()");
                        for (int i = 0; i < contentValues.Count; i++)
                            contentValues[i].Value = contentValues[i].Value + ": that is the question.";
                        Console.Write("\n Change format of content..." + "\r\n");
                        inputXML.Save("questions-chg.xml");

                        XmlElement questionElement = inputXML.CreateElement("Question");
                        XmlElement contentElement = inputXML.CreateElement("Content");
                        XmlElement answerElement = inputXML.CreateElement("Answer");

                        XmlText contentText = inputXML.CreateTextNode("To be or not to be?");
                        XmlText answerText = inputXML.CreateTextNode("Who knows, not me");

                        contentElement.AppendChild(contentText);
                        answerElement.AppendChild(answerText);

                        questionElement.AppendChild(contentElement);
                        questionElement.AppendChild(answerElement);

                        inputXML.DocumentElement.AppendChild(questionElement);
                        inputXML.Save("questions-new.xml");

                        XmlElement newElement = (XmlElement)inputXML.GetElementsByTagName("Question")[3];
                        newElement.SetAttribute("QuestionId", "142");
                        inputXML.Save("questions-new.xml");
                        break;

                    case 4:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
