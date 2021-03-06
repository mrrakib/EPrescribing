
--EXEC [dbo].[GetTodayAppointmentList] 2
CREATE PROCEDURE [dbo].[GetTodayAppointmentList]
(
	@doctorId int=0
)
AS
BEGIN	
	SELECT TOP 10 p.PatientID, p.[Name] AS PatientName, p.Age, p.MobileNo, d.[Name] AS DoctorName, p.NextAppointmentDate FROM Patients p 
	JOIN Doctors d ON p.DoctorId=d.Id
	WHERE CAST(p.NextAppointmentDate AS DATE)=CAST(GETDATE() AS DATE)
	AND p.DoctorId=IIF(@doctorId=0, p.DoctorId, @doctorId)
END
