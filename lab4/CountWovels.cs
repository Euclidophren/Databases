using System;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(Format.UserDefined, MaxByteSize = 8000)]
public struct CountVowels
{
    private SqlInt32 countVowels;

    public void Init()
    {
        countVowels = 0;
    }

    public void Accumulate(SqlString value)
    {
        string vowels = "aeiou";

        for (int i = 0; i < value.ToString().Length; i++)
        {
            for (int j = 0; j < vowels.Length; j++)
            {
                if (value.Value.Substring(i, 1).ToLower() == vowels.Substring(j, 1))
                {
                    countVowels += 1;
                }
            }
        }
    }

    public void Merge(CountVowels value)
    {
        Accumulate(value.Terminate());
    }

    public SqlString Terminate()
    {
        return countVowels.ToString();
    }
}