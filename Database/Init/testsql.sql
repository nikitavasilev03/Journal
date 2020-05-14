USE JournalDB

DELETE FROM Journals
WHERE jour_id = 2;

UPDATE Journals
SET student_account_id = 1, subject_id = 1
WHERE jour_id = 3;

SELECT * FROM Records;
SELECT * FROM Journals;

SELECT *
FROM Accounts, Students
WHERE Accounts.account_id = Students.account_id;

SELECT *
FROM Accounts, Teachers
WHERE Accounts.account_id = Teachers.account_id;

INSERT INTO Journals
VALUES
(NEXT VALUE FOR SEQ_Journals, 1, 3, 1, '2020-04-15');

INSERT INTO Records
VALUES
(NEXT VALUE FOR SEQ_Records, 1, 1, GETDATE(), GETDATE(), 0, 0);


