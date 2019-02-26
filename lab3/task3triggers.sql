USE CHGK;
GO

--C. Два `DML` триггера
--1) Триггер `AFTER` 
--2) Триггер `INSTEAD OF` 

-- Триггер `AFTER `--

-- Триггер `INSTEAD OF` --
CREATE TRIGGER DenyDelete
ON Packages
INSTEAD OF DELETE
AS
BEGIN
    RAISERROR('This is uneditable table.',10,1);
END;