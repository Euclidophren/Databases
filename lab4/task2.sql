sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO

USE dbRobots
GO

CREATE ASSEMBLY SqlServerUDF
FROM 'SqlServerUDF/SqlServerUDF/bin/Debug/SqlServerUDF.dll'

CREATE AGGREGATE CountVowels( @text NCHAR(max))
RETURNS INT
EXTERNAL NAME
SqlServerUDF.[CountVowels]
GO

--SELECT dbo.CountVowels(Authors.FullName) AS 
--FROM dbo.Company
--GO

DROP AGGREGATE CountVowels

DROP ASSEMBLY SqlServerUDF