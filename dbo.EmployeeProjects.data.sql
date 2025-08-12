SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;

IF OBJECT_ID('tempdb..#EPSeed') IS NOT NULL DROP TABLE #EPSeed;
CREATE TABLE #EPSeed
(
    EmployeeEmail NVARCHAR(200) NOT NULL,
    ProjectName   NVARCHAR(200) NOT NULL
);

INSERT INTO #EPSeed (EmployeeEmail, ProjectName) VALUES
(N'alex.nguyen@cmanagement.local',    N'Onboarding Optimization'),
(N'alex.nguyen@cmanagement.local',    N'Analytics Dashboard'),
(N'priya.patel@cmanagement.local',    N'Fleet Upgrade'),
(N'marcus.lee@cmanagement.local',     N'Analytics Dashboard'),
(N'sofia.garcia@cmanagement.local',   N'Cross-border Compliance'),
(N'emma.wong@cmanagement.local',      N'Onboarding Optimization'),
(N'daniel.khan@cmanagement.local',    N'Warehouse WMS'),
(N'noah.bennett@cmanagement.local',   N'Fleet Upgrade'),
(N'isabella.russo@cmanagement.local', N'Analytics Dashboard'),
(N'liam.sousa@cmanagement.local',     N'Cross-border Compliance'),
(N'ava.martinez@cmanagement.local',   N'Warehouse WMS');

INSERT INTO dbo.EmployeeProjects (EmployeeId, ProjectId)
SELECT e.EmployeeId, p.ProjectId
FROM #EPSeed s
JOIN dbo.Employees e ON e.Email = s.EmployeeEmail
JOIN dbo.Projects  p ON p.ProjectName = s.ProjectName
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.EmployeeProjects x 
    WHERE x.EmployeeId = e.EmployeeId AND x.ProjectId = p.ProjectId
);

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'EmployeeProjects seed failed: ' + ERROR_MESSAGE();
END CATCH;

SELECT TOP 10 EmployeeId, ProjectId FROM dbo.EmployeeProjects ORDER BY EmployeeId, ProjectId;
