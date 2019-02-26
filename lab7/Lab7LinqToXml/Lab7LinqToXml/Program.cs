using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LINQ_to_XML
{
    class Program
    {
        static void Main(string[] args)
        {
            ReadXML();
            ReadOneElement();
            UpdateXML();
            WriteXML();
            Console.WriteLine("My job here is done\n");
            Console.ReadLine();
        }

        /* Рабочая функция*/
        static void ReadXML()
        {
            XDocument xdoc = XDocument.Load(@"questions2.xml");
            var result = from question in xdoc.Descendants("Question")
                         select question.Element("Content").Value;
            Console.WriteLine("Найдено  {0} вопроса(ов)", result.Count());
            Console.WriteLine();
            foreach (XElement questionElement in xdoc.Element("CHGK").Elements("Question"))
            {
                XAttribute idAttribute = questionElement.Attribute("QuestionId");
                XElement contentElement = questionElement.Element("Content");
                XElement answerElement = questionElement.Element("Answer");
                XElement sourceElement = questionElement.Element("QuestionSource");

                if (idAttribute != null && contentElement != null && answerElement != null && sourceElement != null)
                {
                    Console.WriteLine("ID: {0}", idAttribute.Value);
                    Console.WriteLine("Content: {0}", contentElement.Value);
                    Console.WriteLine("Answer: {0}", answerElement.Value);
                    Console.WriteLine("Question Source: {0}", sourceElement.Value);
                }
                Console.WriteLine();
            }
        }
        /* Рабочая функция*/
        static void ReadOneElement()
        {
            XDocument xdoc = XDocument.Load(@"questions2.xml");
            XElement questionElement = xdoc.Element("CHGK").Element("Question");
            XAttribute idAttribute = questionElement.Attribute("QuestionId");
            XElement contentElement = questionElement.Element("Content");
            XElement answerElement = questionElement.Element("Answer");
            XElement sourceElement = questionElement.Element("QuestionSource");

            Console.WriteLine("\nПервый найденный вопрос\n");
            Console.WriteLine("ID: {0}", idAttribute.Value);
            Console.WriteLine("Content: {0}", contentElement.Value);
            Console.WriteLine("Answer: {0}", answerElement.Value);
            Console.WriteLine("Question Source: {0}", sourceElement.Value);
        }
        /*рабочая функция*/
        static void UpdateXML()
        {
            XDocument xdoc = XDocument.Load(@"questions2.xml");
            IEnumerable<XElement> answerElements = xdoc.Descendants("Question");
            XElement toChange = answerElements.ElementAtOrDefault(1);
            toChange.SetElementValue("Answer","But you haven't done anything");
            xdoc.Save("updated.xml");
            Console.WriteLine("\n\nИзмененный элемент\n");
            Console.WriteLine(toChange.Element("Content").Value + "\n" + toChange.Element("Answer").Value + "\n" + toChange.Element("QuestionSource").Value + "\n");
        }

        // ПРИМЕР 4. ЗАПИСЬ ДАННЫХ В ДОКУМЕНТ XML
        static void WriteXML()
        {
            XDocument xdoc = new XDocument();
            XElement root = new XElement("Questions");
            XElement newQuestion = new XElement("Question");
            XAttribute id = new XAttribute("QuestionId", "42");
            XElement content = new XElement("Content", "Назовите нобелевского лауреата и овощ одним словом");
            XElement answer = new XElement("Answer", "Пастернак");
            XElement source = new XElement("QuestionSource", "Невероятное совпадение");

            XElement question2 = new XElement("Question", new XAttribute("QuestionId", 43),
                new XElement("Content", "Этот"), new XElement("Answer", "Элемент"),
                new XElement("QuestionSource", "Создан"));

            Console.WriteLine("\nСоздан XML-файл:\n");

            newQuestion.Add(id, content, answer, source);
            root.Add(newQuestion, question2);
            xdoc.Add(root);
            xdoc.Save("written.xml");
        }
    }
}
