using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab7LinqToObjects
{
    public class Author
    {
        public int id;
        public string fullname;
        public int age;
        public string gender;

        public Author(int num, string name, int years, string g)
        {
            id = num;
            fullname = name;
            age = years;
            gender = g;
        }
    }

    public class Package
    {
        public int id;
        public string name;
        public string theme;
        public int questions;
        public string editor;

        public Package(int n, string p, string t, int a, string e)
        {
            id = n;
            name = p;
            theme = t;
            questions = a;
            editor = e;
        }
    }

    public class APQ
    {
        public int authorID;
        public int questionID;
        public int packageID;
        public int used;

        public APQ(int auth, int p, int q, int u)
        {
            authorID = auth;
            packageID = p;
            questionID = q;
            used = u;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Author[] authors = new Author[6];
            authors[0] = new Author(7,"Dannie Hedin",21,"Male");
            authors[1] = new Author(2,"Cinnamon Shiers",41,"Female");
            authors[2] = new Author(42,"Tara Pink",35,"Female");
            authors[3] = new Author(61,"Ave Dyka",28,"Male");
            authors[4] = new Author(77,"Esteban Congreave",50,"Male");
            authors[5] = new Author(80,"Rodina Downing",55,"Female");

            APQ[] authpackq = new APQ[8];
            authpackq[0] = new APQ(7, 30, 45, 3);
            authpackq[1] = new APQ(7, 43, 5, 2);
            authpackq[2] = new APQ(2, 17, 10, 35);
            authpackq[3] = new APQ(42, 2, 4, 6);
            authpackq[4] = new APQ(61, 30, 10, 30);
            authpackq[5] = new APQ(61, 30, 6, 30);
            authpackq[6] = new APQ(77, 17, 7, 35);
            authpackq[7] = new APQ(80, 2, 4, 6);

            Package[] packages = new Package[5];
            packages[0] = new Package(2, "Очевидно-невероятно", "Мистика", 25, "Alex Gonzalez");
            packages[1] = new Package(30, "Дети маминых друзей", "Нобелевские лауреаты", 45, "Alex Gonzalez");
            packages[2] = new Package(17, "Гарри Поттер и подкрадывающийся песец", "Литература конца 19-го века", 12, "Alex Gonzalez");
            packages[3] = new Package(43, "И на словах, и на деле", "Лев Толстой", 44, "Alex Gonzalez");
            packages[4] = new Package(12, "Никто не поверит", "Мистика", 23, "Alex Gonzalez");

            Console.WriteLine("\nЗапрос select where");
            var result = from a in authors
                         where a.age < 40
                         select a.fullname;
            Console.WriteLine("Молодые авторы:");
            for (int i = 0; i < result.Count(); i++)
            {
                Console.WriteLine("имя: " + result.ElementAt(i));
            }

            Console.WriteLine("\nЗапрос select orderby");
            result = from a in authors
                     where a.gender == "Male"
                     orderby a.age ascending
                     select a.fullname;
            Console.WriteLine("Авторы - мужчины, сортировка по возрасту:");
            for (int i = 0; i < result.Count(); i++)
            {
                Console.WriteLine("имя: " + result.ElementAt(i));
            }

            Console.WriteLine("\nЗапрос select group");
            var res = from pack in packages
                     group pack by pack.theme into g
                     orderby g.Key
                     select g;

            foreach (IGrouping<string, Package> packGroup in res)
            {
                Console.WriteLine(packGroup.Key);
                foreach (var pack in packGroup)
                {
                    Console.WriteLine("   {0} - {1}", pack.name, pack.questions);
                }
            }

            Console.WriteLine("\nЗапрос select join");
            var result3 = from a in authpackq
                     where a.used > 10
                     join pack in packages on a.packageID equals pack.id into freqUsed
                     from p in freqUsed
                     select p;

            Console.WriteLine("Часто используемые пакеты вопросов:");
            foreach(var item in result3)
            {
                Console.WriteLine("{0}:{1},{2},{3}", item.id, item.name, item.theme, item.questions);
            }
 
            Console.WriteLine("\nЗапрос select let");
            Console.Write("Введите тему: ");
            String t = Console.ReadLine();
            result = from p in packages
                     let change = t
                     where p.theme == change
                     orderby p.questions descending
                     select p.name;
            Console.WriteLine("Пакеты на тему {0}, сортировка по количеству вопросов(убывание):", t);
            for (int i = 0; i < result.Count(); i++)
            {
                Console.WriteLine("имя: " + result.ElementAt(i));
            }
            Console.ReadLine();
        }
    }
}
