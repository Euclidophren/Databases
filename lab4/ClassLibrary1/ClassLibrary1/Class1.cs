using System;
using System.Data;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;


/* Скалярные функции
   1)Получение случайного числа */
 
public class ScalarFunction1
{
    [Microsoft.SqlServer.Server.SqlFunction]
    public static SqlInt32 GetRandomNumber()
    {
        Random rnd = new Random();
        return rnd.Next();
    }
}

// 2) Получение количества пакетов вопросов, с количеством вопросов больше заданного

public class ScalarFunction2
{
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static int ReturnPackagesCount(int amount)
    {
        using (SqlConnection conn
            = new SqlConnection("context connection=true"))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) FROM Packages WHERE QuestionAmount >  " + amount, conn);
            return (int)cmd.ExecuteScalar();
        }
    }
}

/* определяемые пользователем агрегатные функции
    1) MAX()*/

[Serializable]
[SqlUserDefinedAggregate(
    Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "AgregateFunction")]
public struct AgregateFunction
{
    private int maximal;

    public int GetMax()
    {
        return maximal;
    }

    public void Init()
    {
        maximal = 0;
    }

    public void Accumulate(SqlInt32 Value)
    {
        if (Value > maximal)
        {
            maximal = (int)Value;
        }
    }

    public void Merge(AgregateFunction Group)
    {
        maximal = Group.GetMax();
    }

    public SqlInt32 Terminate()
    {
        if (maximal > 0)
        {
            return new SqlInt32(maximal);
        }
        else
        {
            return SqlInt32.Null;
        }
    }
}

/* 2) SUM()
*/

[Serializable]
[SqlUserDefinedAggregate(
    Format.Native,
    IsInvariantToDuplicates = false,
    IsInvariantToNulls = true,
    IsInvariantToOrder = true,
    IsNullIfEmpty = true,
    Name = "AgregateFunction2")]
public struct AgregateFunction2
{
    private int result;

    public int GetSum()
    {
        return result;
    }

    public void Init()
    {
        result = 0;
    }

    public void Accumulate(SqlInt32 Value)
    {
        if (!Value.IsNull && !Value.IsNull)
        {
            result += (int)Value;
        }
    }

    public void Merge(AgregateFunction2 Group)
    {
        result += Group.GetSum();
    }

    public SqlInt32 Terminate()
    {
        if (result > 0)
        {
            return new SqlInt32(result);
        }
        else
        {
            return SqlInt32.Null;
        }
    }

}

/* 3)Подсчёт гласных*/
[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.Native, MaxByteSize = 8000)]
public struct Vowels
{
    private SqlInt32 count;

    public void Init()
    {
        count = 0;
    }

    public void Accumulate(SqlString value)
    {
        string check = "aeiou";

        for (int i = 0; i < value.ToString().Length; i++)
        {
            for (int j = 0; j < check.Length; j++)
            {
                if (value.Value.Substring(i, 1).ToLower() == check.Substring(j, 1))
                {
                    count += 1;
                }
            }
        }
    }

    public void Merge(Vowels value)
    {
        Accumulate(value.Terminate());
    }

    public SqlString Terminate()
    {
        return count.ToString();
    }
}


/* Табличная функция 
   проверить*/
public class TableFunctions
{
    [SqlFunction(FillRowMethodName = "SplitFillRow", TableDefinition = "part NVARCHAR(MAX), ID_ORDER INT")]

    static public IEnumerator Split(SqlString text, char[] delimiter)
    {
        if (text.IsNull)
        {
            yield break;
        }
        int valueIndex = 1;
        foreach (string s in text.Value.Split(delimiter, StringSplitOptions.RemoveEmptyEntries))
        {
            yield return new Tuple<int, string>(valueIndex++, s.Trim());
        }
    }

    static public void SplitFillRow(object oKeyValuePair, out SqlString value, out SqlInt32 valueIndex)
    {
        Tuple<int, string> keyValuePair = (Tuple<int, string>)oKeyValuePair;

        valueIndex = keyValuePair.Item1;
        value = keyValuePair.Item2;
    }
}

/* Слово и его длина*/

public class TableFunctions2
{
    [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "FillRow",
           TableDefinition = "charpart nchar(1), intpart int")]
    public static IEnumerable StrLen(string InputName)
    {
        yield return new NameRow(InputName, InputName.Length);
    }

    public static void FillRow(object row, out SqlString word, out int len)
    {
        word = ((NameRow)row).word;
        len = ((NameRow)row).len;
    }
}

public class NameRow
{
    public SqlString word;
    public Int32 len;

    public NameRow(SqlString c, Int32 i)
    {
        word = c;
        len = i;
    }
}

/* 4) Хранимая процедура */
public class StoredProcedure
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void getAverageAge()
    {
        SqlConnection connection = new SqlConnection("Context Connection=true");
        connection.Open();

        SqlCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT AVG(Age) FROM Authors WHERE Authors.Gender = 'Male'";
        SqlContext.Pipe.ExecuteAndSend(cmd);
        connection.Close();
    }
}

/* 5) Триггер */
public partial class Triggers
{
    [Microsoft.SqlServer.Server.SqlTrigger(Name = "Trigger", Target = "Packages", Event = "FOR DELETE")]
    public static void DeletionTrigger()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;
        if (triggerContext.TriggerAction == TriggerAction.Delete)
            SqlContext.Pipe.Send("You can't delete from table");
    }
}

/* 6) Пользовательский тип данных 
    E-mail*/

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, IsByteOrdered = true, MaxByteSize = 8000)]
public struct EmailAddress : INullable, IBinarySerialize
{
    private string _value;

    public string Value
    {
        get { return _value; }
        set { _value = value; }
    }
    public string Prefix
    {
        get
        {
            string[] x = Value.Split('@');
            return x[0];
        }
    }

    public string Domain
    {
        get
        {
            string[] x = Value.Split('@');
            return x[1];
        }

    }

    public override string ToString()
    {
        return _value;
    }

    private bool _isNull;
    public bool IsNull
    {
        get
        {
            return _isNull;
        }
    }

    public static EmailAddress Null
    {
        get
        {
            EmailAddress h = new EmailAddress();
            h._isNull = true;
            return h;
        }
    }

    public static EmailAddress Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;

        EmailAddress email = new EmailAddress();
        if (email.ValidateEmail(s))
        {
            email.Value = s.ToString();
            return email;
        }
        else
        {
            throw new SqlTypeException("The email " + s.ToString() + " was not in the proper format.");
        }
    }

    public bool ValidateEmail(SqlString email)
    {
        bool isValid = false;
        if (this.IsNull)
        {
            return true;
        }

        isValid = Regex.IsMatch(email.ToString(), @"^[\w-]+(?:\.[\w-]+)*@(?:[\w-]+\.)+[a-zA-Z]{2,7}$");
        if (isValid)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Write(BinaryWriter w)
    {
        w.Write(_value);
    }

    public void Read(BinaryReader r)
    {
        _value = r.ReadString();
    }

}

