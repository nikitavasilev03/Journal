USE JournalDB
GO

if object_id('NewRecord') is not null
  DROP TRIGGER  NewRecord;
GO

CREATE TRIGGER NewRecord
ON Records
INSTEAD OF INSERT
AS
	DECLARE @record_id NUMERIC(5, 0);
	DECLARE @student_account_id NUMERIC(10, 0);
	DECLARE @subject_id NUMERIC(5, 0);
	DECLARE @date_start DATE;
	DECLARE @date_end DATE;
	DECLARE @number_visits NUMERIC(5, 0);
	DECLARE @is_passed BIT;

	DECLARE cur_ins CURSOR FOR
		SELECT * 
		FROM inserted;
	DECLARE @count int
BEGIN
	--
	OPEN cur_ins;
	FETCH NEXT FROM cur_ins 
	INTO @record_id, 
		 @student_account_id,
		 @subject_id,
		 @date_start,
		 @date_end,
		 @number_visits,
		 @is_passed;

	WHILE @@FETCH_STATUS = 0 BEGIN
		
		SELECT @count = COUNT(*) 
		FROM Records
		WHERE student_account_id = @subject_id and subject_id = @student_account_id;

		IF (@count = 0) BEGIN
			INSERT INTO Records
			VALUES
			(@record_id, @student_account_id, @subject_id, @date_start, @date_end, @number_visits, @is_passed)
		END
		ELSE
		BEGIN
			PRINT 'ERROR! Record already created. INFO (' +
				STR(@record_id) + ', ' +
				STR(@student_account_id) + ', ' +
				STR(@subject_id) + ', ' +
				CONVERT(VARCHAR, @date_start) + ', ' +
				CONVERT(VARCHAR, @date_end) + ', ' +
				STR(@number_visits) + ', ' +
				STR(@is_passed) + ')';
		END;

		FETCH NEXT FROM cur_ins 
		INTO @record_id, 
			 @student_account_id,
			 @subject_id,
		     @date_start,
			 @date_end,
			 @number_visits,
			 @is_passed;

	END;
	CLOSE cur_ins;
	DEALLOCATE cur_ins;

END;

--GO

--CREATE TRIGGER ChangeRecord
--ON Records
--INSTEAD OF UPDATE
--AS
--	DECLARE @record_id NUMERIC(5, 0);
--	DECLARE @student_account_id NUMERIC(10, 0);
--	DECLARE @subject_id NUMERIC(5, 0);
--	DECLARE @date_start DATE;
--	DECLARE @date_end DATE;
--	DECLARE @number_visits NUMERIC(5, 0);
--	DECLARE @is_passed BIT;

--	DECLARE cur_ins CURSOR FOR
--		SELECT * 
--		FROM inserted;
--	DECLARE @count int
--BEGIN
--	--
--	OPEN cur_ins;
--	FETCH NEXT FROM cur_ins 
--	INTO @record_id, 
--		 @student_account_id,
--		 @subject_id,
--		 @date_start,
--		 @date_end,
--		 @number_visits,
--		 @is_passed;

--	WHILE @@FETCH_STATUS = 0 BEGIN
		
--		SELECT @count = COUNT(*) 
--		FROM Records
--		WHERE student_account_id = @subject_id and subject_id = @student_account_id;

--		IF (@count = 0) BEGIN
--			INSERT INTO Records
--			VALUES
--			(@record_id, @student_account_id, @subject_id, @date_start, @date_end, @number_visits, @is_passed)
--		END
--		ELSE
--		BEGIN
--			PRINT 'ERROR! Record already created. INFO (' +
--				STR(@record_id) + ', ' +
--				STR(@student_account_id) + ', ' +
--				STR(@subject_id) + ', ' +
--				CONVERT(VARCHAR, @date_start) + ', ' +
--				CONVERT(VARCHAR, @date_end) + ', ' +
--				STR(@number_visits) + ', ' +
--				STR(@is_passed) + ')';
--		END;

--		FETCH NEXT FROM cur_ins 
--		INTO @record_id, 
--			 @student_account_id,
--			 @subject_id,
--		     @date_start,
--			 @date_end,
--			 @number_visits,
--			 @is_passed;

--	END;
--	CLOSE cur_ins;
--	DEALLOCATE cur_ins;

--END;