using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Lab8
{
    class Tasks
    {
        private readonly string connectionString = @"Data Source = DESKTOP-F796EKR\SQLEXPRESS; Database = CHGK; Integrated Security = true";

        static void Main(string[] args)
        {
            Tasks solution = new Tasks();

            int status = -1;
            while (status == -1)
            {
                Console.WriteLine("1. Info about connection");
                Console.WriteLine("\n2. Scalar query");
                Console.WriteLine("\n3. DataReader");
                Console.WriteLine("\n4. Parametrized Sql Command");
                Console.WriteLine("\n5. Stored Procedure");
                Console.WriteLine("\n6. Create Dataset from Table");
                Console.WriteLine("\n7. Sort Data in Table");
                Console.WriteLine("\n8. Insert Data into Table(Constraints)");
                Console.WriteLine("\n9. Delete Data from Table");
                Console.WriteLine("\n10. Write Data to XML");
                Console.Write("Choice: ");
                status = Convert.ToInt32(Console.ReadLine());
                switch (status)
                {
                    case 1:
                        solution.connectedObjects_task_1_ConnectionString();
                        status = -1;
                        break;
                    case 2:
                        solution.connectedObjects_task_2_SimpleScalarSelection();
                        status = -1;
                        break;
                    case 3:
                        solution.connectedObjects_task_3_SqlCommand_SqlDataReader();
                        status = -1;
                        break;
                    case 4:
                        solution.connectedObjects_task_4_SqlCommandWithParameters();
                        status = -1;
                        break;
                    case 5:
                        solution.connectedObjects_task_5_SqlCommand_StoredProcedure();
                        status = -1;
                        break;
                    case 6:
                        solution.disconnectedObjects_task_6_DataSetFromTable();
                        status = -1;
                        break;
                    case 7:
                        solution.disconnectedObjects_task_7_FilterSort();
                        status = -1;
                        break;
                    case 8:
                        solution.disconnectedObjects_8_Insert();
                        status = -1;
                        break;
                    case 9:
                        solution.disconnectedObjects_9_Delete();
                        status = -1;
                        break;
                    case 10:
                        solution.disconnectedObjects_10_Xml();
                        status = -1;
                        break;
                    case 0:
                        Console.WriteLine("Console Application has stopped");
                        break;
                    default:
                        Console.WriteLine("Unknown parameter");
                        break;
                }
            }
        }

        public void connectedObjects_task_1_ConnectionString()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 1, "[Connected] Shows connection info.");

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");
                Console.WriteLine("Connection properties:");
                Console.WriteLine("\tConnection string: {0}", connection.ConnectionString);
                Console.WriteLine("\tDatabase:          {0}", connection.Database);
                Console.WriteLine("\tData Source:       {0}", connection.DataSource);
                Console.WriteLine("\tServer version:    {0}", connection.ServerVersion);
                Console.WriteLine("\tConnection state:  {0}", connection.State);
                Console.WriteLine("\tWorkstation id:    {0}", connection.WorkstationId);
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the connection creating. Message: " + e.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void connectedObjects_task_2_SimpleScalarSelection()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 2, "[Connected] Simple scalar query.");

            string queryString = @"select Count(*) from Questions where Type = 'C'";
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand scalarQueryCommand = new SqlCommand(queryString, connection);
            Console.WriteLine("Sql command \"{0}\" has been created.", queryString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");
                Console.WriteLine("-------->>> Number of What?Where?When? questions is {0}", scalarQueryCommand.ExecuteScalar());
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql command execution. Message: " + e.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void connectedObjects_task_3_SqlCommand_SqlDataReader()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 3, "[Connected] DataReader for query.");

            string queryString = @"select FullName, Age from Authors where Age > 35";
            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand dataQueryCommand = new SqlCommand(queryString, connection);
            Console.WriteLine("Sql command \"{0}\" has been created.", queryString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");
                SqlDataReader dataReader = dataQueryCommand.ExecuteReader();

                Console.WriteLine("-------->>> Authors older than 35: ");
                while (dataReader.Read())
                {
                    Console.WriteLine("\t{0} {1}", dataReader.GetValue(0), dataReader.GetValue(1));
                }
                Console.WriteLine("-------->>> <<<-------");
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql command execution. Message: " + e.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void connectedObjects_task_4_SqlCommandWithParameters()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 4, "[Connected] SqlCommand (Insert, Delete).");

            string top = Console.ReadLine();
            /*string topQueryString = String.Format(@"select TOP ({0}) FullName from Authors go", top);*/
            string countQueryString = String.Format(@"select {0}(*) from Authors go", top);
            string insertQueryString = @"insert into Authors(ID, FullName, Age, Gender) values (@id, @fullname, @age, @gender)";
            string deleteQueryString = @"delete from Authors where FullName = @fullname";

            SqlConnection connection = new SqlConnection(connectionString);

            /*SqlCommand topQueryCommand = new SqlCommand(topQueryString, connection);*/
            SqlCommand countQueryCommand = new SqlCommand(countQueryString, connection);
            SqlCommand insertQueryCommand = new SqlCommand(insertQueryString, connection);
            SqlCommand deleteQueryCommand = new SqlCommand(deleteQueryString, connection);

            //parameters
            insertQueryCommand.Parameters.Add("@id", SqlDbType.Int);
            insertQueryCommand.Parameters.Add("@fullname", SqlDbType.NVarChar,50);
            insertQueryCommand.Parameters.Add("@age", SqlDbType.Int);
            insertQueryCommand.Parameters.Add("@gender", SqlDbType.NVarChar, 6);
            deleteQueryCommand.Parameters.Add("@fullname", SqlDbType.NVarChar, 50);

            Console.WriteLine("Sql commands: \n1) \"{0}\"\n\n2) \"{1}\"\n\n3) \"{2}\"\n\nhas been created.\n", countQueryString, insertQueryString, deleteQueryString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.\n");
                Console.WriteLine("Current count of authors: {0}\n", countQueryCommand.ExecuteScalar());
                Console.WriteLine("Inserting a new author. Input: ");
                Console.Write("- id = ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.Write("- fullname = ");
                string fullname = Console.ReadLine();
                Console.Write("- age = ");
                int age = Convert.ToInt32(Console.ReadLine());
                Console.Write("-gender = ");
                string gender = Console.ReadLine();


                insertQueryCommand.Parameters["@id"].Value = id;
                insertQueryCommand.Parameters["@fullname"].Value = fullname;
                insertQueryCommand.Parameters["@age"].Value = age;
                insertQueryCommand.Parameters["@gender"].Value = gender;
                deleteQueryCommand.Parameters["@fullname"].Value = fullname;

                Console.WriteLine("\nInsert command: {0}", insertQueryCommand.CommandText);
                insertQueryCommand.ExecuteNonQuery();
                Console.WriteLine("------>>> New count of Authors: {0}", countQueryCommand.ExecuteScalar());

                Console.WriteLine("Delete command: {0}", deleteQueryCommand.CommandText);
                deleteQueryCommand.ExecuteNonQuery();
                Console.WriteLine("------>>> New count of Authors: {0}", countQueryCommand.ExecuteScalar());
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql command execution. Message: " + e.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Bad input! " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void connectedObjects_task_5_SqlCommand_StoredProcedure()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 5, "[Connected] Stored procedure.");

            SqlConnection connection = new SqlConnection(connectionString);

            SqlCommand storedProcedureCommand = connection.CreateCommand();
            storedProcedureCommand.CommandType = CommandType.StoredProcedure;
            storedProcedureCommand.CommandText = "dbo.usp_ThemeInfo";

            Console.WriteLine("Sql command \"{0}\" has been created.", storedProcedureCommand.CommandText);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.\n");
                SqlParameter theme = storedProcedureCommand.Parameters.Add("@Theme", SqlDbType.NVarChar, 50);
                theme.Direction = ParameterDirection.Input;

                Console.Write("Input theme: ");
                theme.Value = Console.ReadLine();

                SqlDataReader result = storedProcedureCommand.ExecuteReader();
                Console.WriteLine("Info about selected theme {0}: ", theme.Value);
                while (result.Read())
                {
                    Console.WriteLine("{0} {1} {2}", result.GetSqlString(0), result.GetInt32(1), result.GetSqlString(2));
                };
                result.Close();

                Console.WriteLine("------>>> {0}", theme);
                /*
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@AverageAge";
                param.SqlDbType = SqlDbType.Int;
                param.Direction = ParameterDirection.Output;
                storedProcedureCommand.Parameters.Add(param);*/
                /*
                var result = storedProcedureCommand.ExecuteScalar();
                Console.WriteLine("Средний возраст авторов - мужчин равен {0}", result.);*/
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql command execution. Message: " + e.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void disconnectedObjects_task_6_DataSetFromTable()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 6, "[Disconnected] DataSet from the table.");

            string query = @"select TOP(10) PackageName, Theme, QuestionAmount from Packages where QuestionAmount > 55";

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "LargePackages");
                DataTable table = dataSet.Tables["LargePackages"];

                Console.WriteLine("Packages with more than 55 questions:");
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine("{0} {1} {2}", row["PackageName"], row["Theme"], row["QuestionAmount"]);
                }
                Console.WriteLine();
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql query execution. Message: " + e.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void disconnectedObjects_task_7_FilterSort()
        {

            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 7, "[Disconnected] Filter and sort.");

            string query = @"select * from Authors";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Authors");
                DataTableCollection tables = dataSet.Tables;
                DataView view = new DataView(tables["Authors"]);
                Console.WriteLine("Input sorting parameter(Id, Full Name, Age or Gender)");
                string sort = Console.ReadLine();
                view.Sort = sort;
                Console.WriteLine("Authors sorted by {0}(ascending)", view.Sort);
                for (int i = 0; i < view.Count; i++)
                {
                    Console.WriteLine("{0}, {1}, {2}, {3}", view[i][0], view[i][1], view[i][2], view[i][3]);
                }
                /*
                Console.Write("Input part of Author's Full Name: ");
                string partOfName = Console.ReadLine();
                Console.WriteLine();
                string filter = "FullName like '%" + partOfName + "%'";
                string sort = "FullName asc";
                Console.WriteLine("Authors who have name like \"" + partOfName + "\":");
                foreach (DataRow row in tables["Authors"].Select(filter, sort))
                {
                    Console.Write("{0} ", row["ID"]);
                    Console.Write("{0} ", row["FullName"]);
                    Console.Write("{0} ", row["Age"]);
                    Console.Write("{0}\n", row["Gender"]);
                }
                Console.WriteLine();
                */

            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql query execution. Message: " + e.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Bad input! Message: " + ex.Message);

            }
            catch(IndexOutOfRangeException ind)
            {
                Console.WriteLine("You try to sort by non-existing column. Message: " + ind.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void disconnectedObjects_8_Insert()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 8, "[Disconnected] Insert.");

            string dataCommand = @"select * from Authors";
            string insertQueryString = @"insert into Authors(ID, FullName, Age, Gender) values (@id, @fullname, @age, @gender)";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");

                Console.WriteLine("Inserting a new Author. Input: ");
                Console.Write("-id = ");
                int id = Convert.ToInt32(Console.ReadLine());
                Console.Write("- name = ");
                string name = Console.ReadLine();
                Console.Write("- age = ");
                int age = Convert.ToInt32(Console.ReadLine());
                Console.Write("- gender = ");
                string gender = Console.ReadLine();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(new SqlCommand(dataCommand, connection));
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Authors");
                DataTable table = dataSet.Tables["Authors"];
                dataSet.Tables["Authors"].Columns["ID"].Unique = false;

                UniqueConstraint idConstraint = new UniqueConstraint("idPrimaryKey",table.Columns["ID"], true);
                table.Constraints.Add(idConstraint);
                dataSet.EnforceConstraints = true;

                DataRow insertingRow = table.NewRow();
                insertingRow["ID"] = id;
                insertingRow["FullName"] = name;
                insertingRow["Age"] = age;
                insertingRow["Gender"] = gender;

                table.Rows.Add(insertingRow);

                Console.WriteLine("Authors");
                foreach (DataRow row in table.Rows)
                {
                    Console.Write("{0} ", row["ID"]);
                    Console.Write("{0} ", row["FullName"]);
                    Console.Write("{0} ", row["Age"]);
                    Console.Write("---- {0}\n", row["Gender"]);
                }

                SqlCommand insertQueryCommand = new SqlCommand(insertQueryString, connection);
                insertQueryCommand.Parameters.Add("@id", SqlDbType.Int);
                insertQueryCommand.Parameters.Add("@fullname", SqlDbType.NVarChar, 35);
                insertQueryCommand.Parameters.Add("@age", SqlDbType.Int);
                insertQueryCommand.Parameters.Add("@gender", SqlDbType.NVarChar, 6);

                insertQueryCommand.Parameters["@id"].Value = id;
                insertQueryCommand.Parameters["@fullname"].Value = name;
                insertQueryCommand.Parameters["@age"].Value = age;
                insertQueryCommand.Parameters["@gender"].Value = gender;

                dataAdapter.InsertCommand = insertQueryCommand;
                dataAdapter.Update(dataSet, "Authors");
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql command execution. Message: " + e.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Bad input! " + ex.Message);
            }
            catch (ConstraintException c)
            {
                Console.WriteLine("ID should not be duplicated! " + c.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void disconnectedObjects_9_Delete()
        {
            Console.WriteLine("".PadLeft(79, '-'));
            Console.WriteLine("Task #{0}: {1}", 9, "[Disconnected] Delete.");

            string dataCommand = @"select * from Authors where Age > 50";
            string deleteQueryString = @"delete from Authors where FullName = @name";

            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Deleting the author. Input: ");
                Console.Write("- name = ");
                string name = Console.ReadLine();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(new SqlCommand(dataCommand, connection));
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Authors");
                DataTable table = dataSet.Tables["Authors"];

                string filter = "FullName = '" + name + "'";
                foreach (DataRow row in table.Select(filter))
                {
                    row.Delete();
                }

                SqlCommand deleteQueryCommand = new SqlCommand(deleteQueryString, connection);
                deleteQueryCommand.Parameters.Add("@name", SqlDbType.NVarChar, 35, "FullName");

                dataAdapter.DeleteCommand = deleteQueryCommand;
                dataAdapter.Update(dataSet, "Authors");

                Console.WriteLine("Authors");
                foreach (DataRow row in table.Rows)
                {
                    Console.Write("{0} ", row["ID"]);
                    Console.Write("{0} ", row["FullName"]);
                    Console.Write("{0} ", row["Age"]);
                    Console.Write("{0}\n", row["Gender"]);
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql command execution. Message: " + e.Message);
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Bad input! " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }

        public void disconnectedObjects_10_Xml()
        {
            Console.WriteLine("".PadLeft(80, '-'));
            Console.WriteLine("Task #{0}: {1}", 10, "WriteXml.");

            string query = @"select TOP(10) FullName, Age, Gender from Authors";

            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("Connection has been opened.");

                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet, "Authors");
                DataTable table = dataSet.Tables["Authors"];

                dataSet.WriteXml("authors.xml");
                Console.WriteLine("Check the authors.xml file.");
            }
            catch (SqlException e)
            {
                Console.WriteLine("There is a problem during the sql query execution. Message: " + e.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Connection has been closed.");
            }
            Console.ReadLine();
        }
    }
}
