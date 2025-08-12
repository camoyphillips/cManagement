SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;

-- Idempotent insert by DepartmentName
INSERT INTO dbo.Departments (DepartmentName)
SELECT v.DepartmentName
FROM (VALUES
    (N'Operations'),
    (N'Logistics'),
    (N'Human Resources'),
    (N'Finance'),
    (N'IT')
) AS v(DepartmentName)
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Departments d WHERE d.DepartmentName = v.DepartmentName
);

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'Departments seed failed: ' + ERROR_MESSAGE();
END CATCH;

SELECT TOP 10 DepartmentId, DepartmentName FROM dbo.Departments ORDER BY DepartmentId;
