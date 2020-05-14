USE JournalDB
GO

if object_id('Timetable') is not null
	DROP TABLE Timetable;
if object_id('Journals') is not null
	DROP TABLE Journals;
if object_id('Records') is not null
	DROP TABLE Records;
if object_id('Students') is not null
	DROP TABLE Students;
if object_id('Teachers') is not null
	DROP TABLE Teachers;
if object_id('Subjects') is not null
	DROP TABLE Subjects;
if object_id('Accounts') is not null
	DROP TABLE Accounts;
GO

CREATE TABLE Accounts
(
    account_id NUMERIC(10, 0) NOT NULL,
    login_name NVARCHAR(255) UNIQUE NOT NULL,
    hpassword NVARCHAR(255) NOT NULL,
    account_type NVARCHAR(50) NOT NULL,
    date_create DATE NOT NULL,
    date_end DATE NULL,
	PRIMARY KEY(account_id)
)

CREATE TABLE Students (
	account_id NUMERIC(10, 0) NOT NULL,
	student_name NVARCHAR(255) NOT NULL,
	student_sname NVARCHAR(255) NOT NULL,
	student_lname NVARCHAR(255) NULL,
	student_group NUMERIC(4, 0) NOT NULL,
	PRIMARY KEY(account_id),
	FOREIGN KEY (account_id)  REFERENCES Accounts (account_id) ON DELETE CASCADE
)

CREATE TABLE Teachers(
	account_id NUMERIC(10, 0) NOT NULL,
	teacher_name NVARCHAR(255) NOT NULL,
	teacher_sname NVARCHAR(255) NOT NULL,
	teacher_lname NVARCHAR(255) NULL,
	PRIMARY KEY(account_id),
	FOREIGN KEY (account_id)  REFERENCES Accounts (account_id) ON DELETE CASCADE
)

CREATE TABLE Subjects(
	subject_id NUMERIC(5, 0) NOT NULL,
	subject_name NVARCHAR(255) NOT NULL,
	need_visits NUMERIC(5, 0) NULL,
	PRIMARY KEY(subject_id)
)

CREATE TABLE Records(
	record_id NUMERIC(5, 0) NOT NULL,
	student_account_id NUMERIC(10, 0) NOT NULL,
	subject_id NUMERIC(5, 0) NOT NULL,
	date_start DATE NOT NULL,
	date_end DATE NOT NULL,
	number_visits NUMERIC(5, 0) NOT NULL,
	is_passed BIT NOT NULL,
	PRIMARY KEY(record_id),
	FOREIGN KEY (student_account_id)  REFERENCES Students (account_id) ON DELETE CASCADE,
	FOREIGN KEY (subject_id)  REFERENCES Subjects (subject_id) ON DELETE CASCADE
)

CREATE TABLE Journals(
	jour_id NUMERIC(10, 0) NOT NULL,
	subject_id NUMERIC(5, 0) NOT NULL,
	teacher_account_id NUMERIC(10, 0) NOT NULL,
	student_account_id NUMERIC(10, 0) NOT NULL,
	visit_date DATE NULL,
	PRIMARY KEY(jour_id),
	FOREIGN KEY (teacher_account_id)  REFERENCES Teachers (account_id) ON DELETE NO ACTION,
	FOREIGN KEY (student_account_id)  REFERENCES Students (account_id) ON DELETE NO ACTION,
	FOREIGN KEY (subject_id)  REFERENCES Subjects (subject_id) ON DELETE CASCADE
)

CREATE TABLE Timetable(
	tt_id NUMERIC(5, 0) NOT NULL,
	record_id NUMERIC(5, 0) NOT NULL,
	teacher_account_id NUMERIC(10, 0) NOT NULL,
	tt_week_day NUMERIC(1, 0) NULL,
	tt_num_lesson NUMERIC(1, 0) NULL,
	PRIMARY KEY(tt_id),
	FOREIGN KEY (record_id)  REFERENCES Records (record_id) ON DELETE CASCADE,
	FOREIGN KEY (teacher_account_id)  REFERENCES Teachers (account_id) ON DELETE NO ACTION
)