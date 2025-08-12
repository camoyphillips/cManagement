SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;

-- Idempotent insert by ProjectName
INSERT INTO dbo.Projects (ProjectName, [Description])
SELECT v.ProjectName, v.[Description]
FROM (VALUES
    (N'Onboarding Optimization', N'Streamline new-employee onboarding across departments.'),
    (N'Fleet Upgrade',           N'Plan and execute phased refresh of the truck fleet.'),
    (N'Warehouse WMS',           N'Implement a warehouse management system.'),
    (N'Cross-border Compliance', N'Improve US/CA customs workflow and documentation.'),
    (N'Analytics Dashboard',     N'Company-wide operational KPI dashboards.')
) AS v(ProjectName, [Description])
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Projects p WHERE p.ProjectName = v.ProjectName
);

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'Projects seed failed: ' + ERROR_MESSAGE();
END CATCH;

SELECT TOP 10 ProjectId, ProjectName, [Description] FROM dbo.Projects ORDER BY ProjectId;
