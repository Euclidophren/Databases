using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{        
    [Microsoft.SqlServer.Server.SqlTrigger (Name="Trigger", Target="CHGK", Event="FOR INSERT")]
    public static void BlockInsert ()
    {
        SqlTriggerContext triggerContext = SqlContext.TriggerContext;

        if (triggerContext.TriggerAction == TriggerAction.Insert)
            SqlContext.Pipe.Send("Can't insert.");
    }
}