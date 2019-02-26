USE CHGK
GO

sp_configure 'show advanced options', 1
GO
RECONFIGURE
GO
sp_configure 'clr enabled', 1
GO
RECONFIGURE
GO
sp_configure 'clr strict security', 0;
GO
RECONFIGURE
GO

IF OBJECT_ID (N'dbo.GetRandomNumber') IS NOT NULL
DROP FUNCTION dbo.GetRandomNumber
GO

IF OBJECT_ID (N'dbo.GetPackagesCount') IS NOT NULL
DROP FUNCTION dbo.GetPackagesCount
GO

IF OBJECT_ID (N'dbo.GetMax') IS NOT NULL
DROP AGGREGATE dbo.GetMax
GO

IF OBJECT_ID (N'dbo.UserSum') IS NOT NULL
DROP AGGREGATE dbo.UserSum
GO

IF OBJECT_ID (N'dbo.GetAverageAge') IS NOT NULL
DROP PROCEDURE dbo.GetAverageAge
GO


IF EXISTS (SELECT name FROM sysobjects WHERE name = 'Split')  
   DROP FUNCTION Split;  
GO 

IF EXISTS (SELECT name FROM sysobjects WHERE name = 'Length')
	DROP FUNCTION Length;
GO


IF OBJECT_ID (N'dbo.DeletionTrigger') IS NOT NULL
DROP TRIGGER dbo.DeletionTrigger
GO

DROP TYPE IF EXISTS dbo.Email
GO


DROP ASSEMBLY ClassLibrary1;
GO

CREATE ASSEMBLY ClassLibrary1 
AUTHORIZATION dbo
FROM 'J:\SPDF\Bauman\DataBases\lab4\ClassLibrary1.dll' 
WITH PERMISSION_SET = SAFE;  
GO  

-- Скалярные функции --
-- случайное число --
CREATE FUNCTION GetRandomNumber()
RETURNS INT  
AS EXTERNAL NAME ClassLibrary1.ScalarFunction1.GetRandomNumber;   
GO  

SELECT dbo.GetRandomNumber() AS 'Generated Number';  
GO    


-- Количество пакетов вопросов, где количество вопросов > заданного числа
CREATE FUNCTION dbo.GetPackagesCount(@amount int) RETURNS INT   
EXTERNAL NAME ClassLibrary1.ScalarFunction2.ReturnPackagesCount;   
GO

SELECT dbo.GetPackagesCount(35) AS 'Packages'

-- агрегатные функции --
-- MAX --
CREATE AGGREGATE dbo.GetMax(@value int) RETURNS INT   
EXTERNAL NAME ClassLibrary1.AgregateFunction;   
GO

SELECT dbo.GetMax(QuestionAmount) AS 'Max'
FROM Packages
GO

-- SUM -- 
CREATE AGGREGATE dbo.UserSum(@value int) RETURNS INT   
EXTERNAL NAME ClassLibrary1.AgregateFunction2;   
GO

SELECT dbo.UserSum(QuestionAmount) AS 'SUM'
FROM Packages
GO

-- Табличная функция

CREATE FUNCTION Split(@string NVARCHAR(MAX), @delimiter NCHAR(1))   
RETURNS TABLE (  
   name nvarchar(MAX),
   ID_ORDER INT  
)  
AS EXTERNAL NAME ClassLibrary1.TableFunctions.[Split];  
GO 

SELECT * FROM Split('11,22,33',' ');  
GO


-- Хранимая процедура

CREATE PROCEDURE dbo.GetAverageAge
AS EXTERNAL NAME ClassLibrary1.StoredProcedure.getAverageAge
GO

DECLARE @result INT
EXECUTE @result = dbo.GetAverageAge
PRINT @result
GO

-- Триггер

CREATE TRIGGER DeletionTrigger
ON Packages
FOR DELETE  
AS  
EXTERNAL NAME ClassLibrary1.Triggers.DeletionTrigger
GO

SELECT * FROM Packages 
GO

DELETE FROM Packages
WHERE ID = 403
GO  

SELECT * FROM Packages WHERE id = 403 
GO

-- Пользовательский тип

CREATE TYPE Email
EXTERNAL NAME ClassLibrary1.EmailAddress
GO

CREATE TABLE dbo.CHECKTYPE
(
	ID INT NOT NULL, 
	EM NCHAR(9)
)


INSERT dbo.CHECKTYPE VALUES (1, 'a@lab.com')
SELECT * FROM dbo.CHECKTYPE
SELECT EM FROM CHECKTYPE AS Email

CREATE TABLE dbo.CHECKTYPE2 
(
	ID INT NOT NULL,
	t Email 
)

INSERT dbo.CHECKTYPE2 VALUES (1, 'a@lab.com')
SELECT t.Value FROM CHECKTYPE2 as Email
INSERT dbo.CHECKTYPE2 VALUES (2, 'error')
DROP TABLE dbo.CHECKTYPE2
