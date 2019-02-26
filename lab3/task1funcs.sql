--A. Четыре функции
--1) Скалярную функцию
--2) Подставляемую табличную функцию 
--3) Многооператорную табличную функцию 
--4) Рекурсивную функцию или функцию с рекурсивным ОТВ 

USE CHGK
GO
-- скалярная функция --
IF OBJECT_ID (N'dbo.AverageQuestionAmount', N'FN') IS NOT NULL
    DROP FUNCTION dbo.AverageQuestionAmount
GO
CREATE FUNCTION dbo.AverageQuestionAmount()
RETURNS INT
WITH SCHEMABINDING
AS
BEGIN
       RETURN (SELECT AVG(QuestionAmount) FROM dbo.Packages)
END
GO

SELECT dbo.AverageQuestionAmount()

-- подставляемая табличная функция --
IF OBJECT_ID (N'dbo.Used', N'FN') IS NOT NULL
    DROP FUNCTION dbo.Used
GO

CREATE FUNCTION dbo.Used()
RETURNS TABLE
AS
RETURN (
    SELECT APQ.Fk_Authors_Id, Authors.FullName, Authors.Age, Authors.Sex, APQ.Fk_Package_Id
    FROM Authors JOIN APQ ON Authors.Pk_Author_Id = APQ.Fk_Authors_Id
    WHERE APQ.Used = 1
    )
GO

SELECT *
FROM dbo.Used()
GO

-- Многооператорная табличная функция --
IF OBJECT_ID (N'dbo.USSR', N'FN') IS NOT NULL
    DROP FUNCTION dbo.USSR
GO

CREATE FUNCTION dbo.USSR()
RETURNS @USSR TABLE 
(
    FullName VARCHAR(85) NOT NULL, 
    Sex VARCHAR(6), 
    Age INT
)
AS
BEGIN
    INSERT INTO @USSR
    SELECT Authors.FullName, Authors.Sex, Authors.Age
    FROM Authors JOIN APQ ON Authors.Pk_Author_Id = APQ.Fk_Authors_Id JOIN Packages ON APQ.Fk_Package_Id = Packages.Pk_Package_Id
    WHERE Packages.Theme = 'USSR'
RETURN
END
GO


SELECT *
FROM dbo.USSR()

--Рекурсивная функция или функция с рекурсивным ОТВ--
CREATE FUNCTION dbo.CalculateFactorial1 (@n int = 1) RETURNS float
WITH RETURNS NULL ON NULL INPUT
AS
BEGIN
      DECLARE @result float;
        SET @result = NULL;
    IF @n > 0 BEGIN
            SET @result = 1.0;
            WITH Numbers (num)
            AS
            (
                SELECT 1
                UNION ALL
                SELECT num + 1
                FROM Numbers
                WHERE num < @n
            )
            SELECT @result = @result * num
            FROM Numbers;
END;
    RETURN @result;
END;
GO
SELECT dbo.CalculateFactorial1 (-10); SELECT dbo.CalculateFactorial1 (NULL); SELECT dbo.CalculateFactorial1 (10);
