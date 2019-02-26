--B. Четыре хранимых процедуры
--1) Хранимую процедуру без параметров или с параметрами 
--2) Рекурсивную хранимую процедуру или хранимую процедур с рекурсивным ОТВ 
--3) Хранимую процедуру с курсором 
--4) Хранимую процедуру доступа к метаданным 


USE CHGK;
GO
-- Хранимую процедуру без параметров или с параметрами --
IF OBJECT_ID ( N'dbo.SelectMen', 'P' ) IS NOT NULL
      DROP PROCEDURE dbo.SelectMen
GO
CREATE PROCEDURE dbo.SelectMen @Amount INT OUTPUT AS
BEGIN
      SELECT FullName, Age
      FROM Authors
      WHERE Authors.Sex = 'male'

      SET @Amount = @@ROWCOUNT
      RETURN (SELECT MAX(Age) from Authors)
END
GO

DECLARE @OutParm INT, @RetVal INT
EXEC @RetVal = dbo.SelectMen @OutParm OUTPUT
SELECT @OutParm "Количество мужчин", @RetVal "Максимальный возраст" 
GO

-- Рекурсивная хранимая процедура или хранимая процедура с рекурсивным ОТВ --
--пример(изменить)
IF OBJECT_ID ( N'dbo.SelectSurvAdultMen', 'P' ) IS NOT NULL
      DROP PROCEDURE dbo.SelectSurvAdultMen
GO

CREATE PROCEDURE dbo.SelectSurvAdultMen
AS
    WITH SurvAdultMen (Passenger)
    AS 
    (
        SELECT PTS.Passenger
        FROM PTS 
        WHERE PTS.Survival = 1

        UNION ALL

        SELECT P.Passenger
        FROM P JOIN SurvAdultMen ON P.Passenger = SurvAdultMen.Passenger
        WHERE P.Sex = 'male' AND P.Age > 18
    )

    SELECT *
    FROM SurvAdultMen

GO

--Хранимая процедура с курсором--
-- Объявляем переменную
DECLARE @TableName varchar(255)
-- Объявляем курсор
DECLARE TableCursor CURSOR FOR
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
-- Открываем курсор и выполняем извлечение первой записи 
OPEN TableCursor
FETCH NEXT FROM TableCursor INTO @TableName
-- Проходим в цикле все записи из множества
WHILE @@FETCH_STATUS = 0
BEGIN
   PRINT @TableName
FETCH NEXT FROM TableCursor INTO @TableName END
-- Убираем за собой «хвосты»
CLOSE TableCursor
DEALLOCATE TableCursor

--Хранимая процедура доступа к метаданным--
IF OBJECT_ID ( N'dbo.ScalarFunc', 'P' ) IS NOT NULL
      DROP PROCEDURE dbo.ScalarFunc
GO
CREATE PROCEDURE ScalarFunc @Amount INT OUTPUT AS
BEGIN
    DECLARE @funcname varchar(200), @Par varchar(200); SET NOCOUNT ON;
    DECLARE funcName_Cursor CURSOR FOR

    SELECT DISTINCT sys.objects.object_id
    FROM sys.objects JOIN sys.parameters ON sys.objects.object_id = sys.parameters.object_id
    WHERE [type] = 'FN'

    SET @Amount = 0

    OPEN funcName_Cursor;
    FETCH NEXT FROM funcName_Cursor INTO @funcname;
    WHILE @@FETCH_STATUS = 0
    BEGIN

    SET @Par = (SELECT COUNT(name) FROM sys.parameters WHERE sys.parameters.object_id = @funcname)

    IF (@Par != 0)
    BEGIN
        SELECT DISTINCT sys.objects.name AS 'Имя функции', sys.parameters.name AS 'Параметры' 
        FROM sys.objects JOIN sys.parameters ON sys.objects.object_id = sys.parameters.object_id
        WHERE sys.objects.object_id = @funcname
    END
    SET @Amount = @Amount + 1

    FETCH NEXT FROM funcName_Cursor INTO @funcname; 
    END;

    CLOSE funcName_Cursor;
    DEALLOCATE funcName_Cursor;
END;
GO

DECLARE @OutParm INT
EXECUTE ScalarFunc @OutParm OUTPUT;
SELECT @OutParm "Количество функций"
GO
