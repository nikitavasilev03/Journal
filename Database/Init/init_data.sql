USE JournalDB
GO

DELETE FROM Timetable;
DELETE FROM Journals;
DELETE FROM Records;
DELETE FROM Students; 
DELETE FROM Subjects;
DELETE FROM Teachers;
DELETE FROM Accounts;
GO

if object_id('SEQ_Accounts') is not null
	DROP SEQUENCE SEQ_Accounts;
if object_id('SEQ_Subjects') is not null
	DROP SEQUENCE SEQ_Subjects;
if object_id('SEQ_Records') is not null
	DROP SEQUENCE SEQ_Records;
if object_id('SEQ_Timetable') is not null
	DROP SEQUENCE SEQ_Timetable; 
if object_id('SEQ_Journals') is not null
	DROP SEQUENCE SEQ_Journals;
GO

CREATE SEQUENCE SEQ_Accounts
    START WITH 1  
    INCREMENT BY 1;
CREATE SEQUENCE SEQ_Subjects
    START WITH 1  
    INCREMENT BY 1;  
CREATE SEQUENCE SEQ_Records
    START WITH 1  
    INCREMENT BY 1;  
CREATE SEQUENCE SEQ_Timetable
    START WITH 1  
    INCREMENT BY 1;  
CREATE SEQUENCE SEQ_Journals
    START WITH 1  
    INCREMENT BY 1;   

INSERT INTO Accounts 
VALUES 
(NEXT VALUE FOR SEQ_Accounts, 'vna.17', '111111', 'Ученик', GETDATE(), NULL),
(NEXT VALUE FOR SEQ_Accounts, 'asv.17', '111111', 'Ученик', GETDATE(), NULL),
(NEXT VALUE FOR SEQ_Accounts, 'sinicin', '111111', 'Преподаватель', GETDATE(), NULL);

INSERT INTO Students 
VALUES  
(1, 'Никита', 'Васильев', 'Александрович', 3254),
(2, 'Степан', 'Аракельянц', 'Владимирович', 3254);

INSERT INTO Teachers 
VALUES
(3, 'Андрей', 'Синицин', 'Александрович');

INSERT INTO Subjects 
VALUES
(NEXT VALUE FOR SEQ_Subjects, 'Баскетбол', 25),
(NEXT VALUE FOR SEQ_Subjects, 'Волейбол', 25);

INSERT INTO Records
VALUES
(NEXT VALUE FOR SEQ_Records, 1, 1, GETDATE(), GETDATE(), 0, 0),
(NEXT VALUE FOR SEQ_Records, 2, 2, GETDATE(), GETDATE(), 0, 0);

INSERT INTO Timetable
VALUES
(NEXT VALUE FOR SEQ_Timetable, 1, 3, 3, 5),
(NEXT VALUE FOR SEQ_Timetable, 2, 3, 4, 3);

INSERT INTO Journals
VALUES
(NEXT VALUE FOR SEQ_Journals, 1, 3, 1, '2020-04-8'),
(NEXT VALUE FOR SEQ_Journals, 2, 3, 2, '2020-04-2'),
(NEXT VALUE FOR SEQ_Journals, 2, 3, 2, '2020-04-9'),
(NEXT VALUE FOR SEQ_Journals, 2, 3, 2, '2020-04-16');