SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;

-- Seed rows (using DepartmentName to resolve DepartmentId)
IF OBJECT_ID('tempdb..#EmployeesSeed') IS NOT NULL DROP TABLE #EmployeesSeed;
CREATE TABLE #EmployeesSeed
(
    FirstName  NVARCHAR(100) NOT NULL,
    LastName   NVARCHAR(100) NOT NULL,
    Email      NVARCHAR(200) NOT NULL,
    DeptName   NVARCHAR(200) NOT NULL
);

INSERT INTO #EmployeesSeed (FirstName, LastName, Email, DeptName) VALUES
(N'Alex',     N'Nguyen',   N'alex.nguyen@cmanagement.local',       N'Operations'),
(N'Priya',    N'Patel',    N'priya.patel@cmanagement.local',       N'Logistics'),
(N'Marcus',   N'Lee',      N'marcus.lee@cmanagement.local',        N'IT'),
(N'Sofia',    N'Garcia',   N'sofia.garcia@cmanagement.local',      N'Finance'),
(N'Emma',     N'Wong',     N'emma.wong@cmanagement.local',         N'Human Resources'),
(N'Daniel',   N'Khan',     N'daniel.khan@cmanagement.local',       N'Operations'),
(N'Noah',     N'Bennett',  N'noah.bennett@cmanagement.local',      N'Logistics'),
(N'Isabella', N'Russo',    N'isabella.russo@cmanagement.local',    N'IT'),
(N'Liam',     N'Sousa',    N'liam.sousa@cmanagement.local',        N'Finance'),
(N'Ava',      N'Martinez', N'ava.martinez@cmanagement.local',      N'Operations');

-- Ensure AspNetUsers rows exist (UserName = Email)
INSERT INTO dbo.AspNetUsers
([Id],[UserName],[NormalizedUserName],[Email],[NormalizedEmail],
 [EmailConfirmed],[PasswordHash],[SecurityStamp],[ConcurrencyStamp],
 [PhoneNumber],[PhoneNumberConfirmed],[TwoFactorEnabled],
 [LockoutEnd],[LockoutEnabled],[AccessFailedCount])
SELECT
    NEWID(), s.Email, UPPER(s.Email), s.Email, UPPER(s.Email),
    0, NULL, CONVERT(nvarchar(36), NEWID()), CONVERT(nvarchar(36), NEWID()),
    NULL, 0, 0, NULL, 1, 0
FROM #EmployeesSeed s
WHERE NOT EXISTS (SELECT 1 FROM dbo.AspNetUsers u WHERE u.[UserName] = s.Email);

-- Insert Employees (no identity insert; key is Email uniqueness)
INSERT INTO dbo.Employees (FirstName, LastName, Email, DepartmentId, ApplicationUserId)
SELECT
    s.FirstName,
    s.LastName,
    s.Email,
    d.DepartmentId,
    u.Id
FROM #EmployeesSeed s
JOIN dbo.Departments d ON d.DepartmentName = s.DeptName
JOIN dbo.AspNetUsers u ON u.[UserName] = s.Email
WHERE NOT EXISTS (SELECT 1 FROM dbo.Employees e WHERE e.Email = s.Email);

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'Employees seed failed: ' + ERROR_MESSAGE();
END CATCH;

SELECT TOP 10 EmployeeId, FirstName, LastName, Email, DepartmentId, ApplicationUserId
FROM dbo.Employees ORDER BY EmployeeId;
