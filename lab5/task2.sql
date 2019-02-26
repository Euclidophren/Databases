DECLARE @idoc int
DECLARE @doc xml
SELECT @doc = c FROM OPENROWSET(BULK 'J:\SPDF\Bauman\DataBases\lab5\LargePackages.xml',
                                SINGLE_BLOB) AS TEMP(c)
EXEC sp_xml_preparedocument @idoc OUTPUT, @doc

SELECT *
FROM OPENXML (@idoc, '/LargePackages/LargePackages')
WITH (Content NCHAR(100) , Answer NCHAR(50), Source NCHAR(50))
EXEC sp_xml_removedocument @idoc
