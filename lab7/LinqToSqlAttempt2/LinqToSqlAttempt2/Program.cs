using System;
using System.Linq;
using System.Data.Linq;


namespace LinqToSqlAttempt2
{
    class Program
    {
        public static DataClasses1DataContext db;
        public static void selectAuthors()
        {
            try
            {
                IQueryable<Authors> query = from c in db.GetTable<Authors>()
                                            where c.Gender == "Female"
                                            select c;
                foreach (Authors author in query)
                {
                    Console.WriteLine("{0} -- {1}", author.FullName, author.Age);
                }
            }
            catch(NullReferenceException e)
            {
                Console.WriteLine("No item in query: " + e.Message);
            }
        }

        public static void selectJoin()
        {
            try
            {
                var joinQuery = from pack in db.GetTable<Packages>()
                                join con in db.GetTable<APQ>()
                                on pack.ID equals con.PackageID
                                where con.Used > 150
                                select new { Name = pack.PackageName, Editor = pack.Editor, Used = con.Used };

                foreach (var item in joinQuery)
                {
                    Console.WriteLine("{0}{1}{2}", item.Name, item.Editor, item.Used);
                }
            }
            catch(NullReferenceException e1)
            {
                Console.WriteLine("No item in query: " + e1.Message);
            }
        }


        public static void insertInfo()
        {
            try
            {
                Questions question = new Questions
                {
                    ID = 1001,
                    Content = "Какой несъедобный предмет Бродский образно назвал желтой слезой, висящей на растрепанной косице?",
                    Answer = "[Электрическую] лампочку.",
                    QuestionSource = "И.А. Бродский. Полторы комнаты.",
                    Type = 'C'
                };
                Table<Questions> table = db.GetTable<Questions>();
                table.InsertOnSubmit(question);
                db.SubmitChanges(ConflictMode.FailOnFirstConflict);

                var query = from q in db.GetTable<Questions>()
                            where q.ID == 1001
                            select q;
                foreach (var item in query)
                {
                    Console.WriteLine(item.Content);
                    Console.WriteLine(item.Answer);
                }
            }
            catch (NullReferenceException e3)
            {
                Console.WriteLine("No item in query: " + e3.Message);
                throw new NullReferenceException(e3.Message);
            }
            catch(Exception e) {

            }
            finally
            {
                Console.WriteLine("Completed.");
            }
        }
        public static void updateInfo()
        {
            try
            {
                Table<Authors> authorTable = db.GetTable<Authors>();

                var result = from a in authorTable
                             where a.FullName == "Jarad Huxster"
                             select a;
                foreach (var r in result)
                {
                    Console.WriteLine($"\"{r.ID}\" {r.FullName}\" {r.Age}\" {r.Gender}\n");
                }
                if (result.Count() > 0)
                {
                    foreach (var r in result)
                    {
                        r.FullName = "Dmitry Avdeenko";
                        Console.WriteLine($"\"{r.ID}\" {r.FullName}\" {r.Age}\" {r.Gender}\n");
                    }
                    db.SubmitChanges();
                }
                else
                {
                    Console.WriteLine("Нет информации для обновления\n");
                }
            }
            catch (NullReferenceException e4)
            {
                Console.WriteLine("No item in query: " + e4.Message);
            }
        }

        public static void deleteInfo()
        {
            try
            {
                var question = db.GetTable<Questions>().OrderByDescending(q => q.ID).FirstOrDefault();

                if (question != null)
                {
                    Console.WriteLine("Удаляемый вопрос:");
                    Console.WriteLine("Содержание: {0}", question.Content);
                    Console.WriteLine("Ответ: {0}", question.Answer);

                    db.GetTable<Questions>().DeleteOnSubmit(question);
                    db.SubmitChanges();
                    Console.WriteLine("Вопрос удален");
                }
            }
            catch (NullReferenceException e5)
            {
                Console.WriteLine("No item in query: " + e5.Message);
            }
        }
        public static void defend()
        {
            try
            {
                var joinQuery = from pack in db.GetTable<Packages>()
                                join con in db.GetTable<APQ>()
                                on pack.ID equals con.PackageID
                                where con.Used > 150
                                join auth in db.GetTable<Authors>()
                                on con.AuthorID equals auth.IDy
                                select new { Name = pack.PackageName, Editor = pack.Editor, Used = con.Used , Author = auth.FullName};

                foreach (var item in joinQuery)
                {
                    Console.WriteLine("{0}-{1}-{2}-{3}", item.Name, item.Editor, item.Used, item.Author);
                }
            }
            catch (NullReferenceException e1)
            {
                Console.WriteLine("No item in query: " + e1.Message);
            }
        }
        public static void Main(string[] args)
        {
            /*db = new DataContext(@"C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\CHGK.mdf");*/
            db = new DataClasses1DataContext(@"Data Source = DESKTOP-F796EKR\SQLEXPRESS; Database = CHGK; Integrated Security = true");
            int status = -1;
            while (status == -1)
            {
                Console.WriteLine("\n1. Однотабличный запрос\n2. Многотабличный запрос\n3. Добавить данные\n4. Изменить данные\n5. Удалить данные\n6. Доступ к данным\n0. Выход");
                Console.WriteLine("Выбор: ");
                status = Convert.ToInt32(Console.ReadLine());
                switch (status)
                {
                    case 1:
                        Console.WriteLine("\n:Авторы:\n");
                        selectAuthors();
                        Console.WriteLine(" ");
                        status = -1;
                        break;
                    case 2:
                        selectJoin();
                        status = -1;
                        break;
                    case 3:
                        Console.WriteLine("\nДобавление нового вопроса:\n");
                        insertInfo();
                        status = -1;
                        break;
                    case 4:
                        updateInfo();
                        status = -1;
                        break;
                    case 5:
                        deleteInfo();
                        status = -1;
                        break;
                    case 6:
                        Console.WriteLine("Средний возраст авторов-мужчин");
                        /*UserContext df = new UserContext(@"Data Source = DESKTOP-F796EKR\SQLEXPRESS; Database = CHGK; Integrated Security = true");
                        int avgAge = 0;
                        df.SelectMen(ref avgAge);*/
                        System.Nullable<int> averageAge = 0;
                        ISingleResult<usp_SelectMenResult> avgAge = db.usp_SelectMen(ref averageAge);
                        foreach (var a in avgAge)
                        {
                            Console.WriteLine(a.Column1);
                        }
                        status = -1;
                        break;
                    case 7:
                        defend();
                        status = -1;
                        break;
                    case 0:
                        Console.WriteLine("\nЗавершение\n");
                        break;
                    default:
                        Console.WriteLine("\nНеизвестный пункт меню\n");
                        status = -1;
                        break;
                }
            }
        }
    }
}
