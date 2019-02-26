CREATE ASSEMBLY SqlServerUDF
FROM 'SqlServerUDF/SqlServerUDF/bin/Debug/SqlServerUDF.dll'
GO

USE CHGK
GO

CREATE TRIGGER BlockInsert
ON Authors
INSTEAD OF INSERT
AS
EXTERNAL NAME
SqlServerUDF.[Triggers].BlockInsert
GO

INSERT INTO dbo.Authors VALUES(1001, "Antony Hopkins", 65, "male")
GO

DROP TRIGGER BlockInsert

DROP ASSEMBLY SqlServerUDF