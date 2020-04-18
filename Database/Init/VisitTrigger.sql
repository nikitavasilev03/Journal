USE JournalDB
GO

if object_id('Visit') is not null
  DROP TRIGGER  Visit;
if object_id('Visit_Not_Update') is not null
  DROP TRIGGER  Visit;
GO

CREATE TRIGGER Visit
ON Journals
AFTER INSERT, DELETE
AS
	DECLARE @subject_id NUMERIC(5, 0);
	DECLARE @student_account_id NUMERIC(10, 0);
	DECLARE cur_del CURSOR FOR
		SELECT subject_id, student_account_id 
		FROM deleted;
	DECLARE cur_ins CURSOR FOR
		SELECT subject_id, student_account_id 
		FROM inserted;
BEGIN
	--
	OPEN cur_del;
	FETCH NEXT FROM cur_del 
	INTO @subject_id,
		 @student_account_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE Records
		SET Records.number_visits = Records.number_visits - 1
		WHERE Records.subject_id = @subject_id and Records.student_account_id = @student_account_id;

		FETCH NEXT FROM cur_del 
		INTO @subject_id,
			 @student_account_id;

	END;
	CLOSE cur_del;
	DEALLOCATE cur_del;
	
	--
	OPEN cur_ins;
	FETCH NEXT FROM cur_ins 
	INTO @subject_id,
		 @student_account_id;
	WHILE @@FETCH_STATUS = 0 BEGIN

		UPDATE Records
		SET Records.number_visits = Records.number_visits + 1
		WHERE Records.subject_id = @subject_id and Records.student_account_id = @student_account_id;

		FETCH NEXT FROM cur_ins 
		INTO @subject_id,
			 @student_account_id;

	END;
	CLOSE cur_ins;
	DEALLOCATE cur_ins;

END;

GO

CREATE TRIGGER Visit_Not_Update
ON Journals
INSTEAD OF UPDATE
AS
BEGIN
	RAISERROR ('ERROR', -- Message text.  
               16, -- Severity.  
               1 -- State.  
               );  
END;
