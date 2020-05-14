USE JournalDB
GO

if object_id('getSubjectById') is not null
  DROP TRIGGER  NewRecord;
GO

CREATE FUNCTION getSubjectById(@id NUMERIC)
RETURNS NVARCHAR
	DECLARE @record_id NUMERIC(5, 0);
AS
BEGIN
	SELECT 
END