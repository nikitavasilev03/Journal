USE JournalDB

SELECT *
FROM Timetable, Records
WHERE Timetable.record_id = Records.record_id and teacher_account_id = 3;