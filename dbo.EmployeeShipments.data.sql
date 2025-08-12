SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;

IF NOT EXISTS (SELECT 1 FROM dbo.Employees)
BEGIN
    PRINT 'No employees found; skipping EmployeeShipments.';
    ROLLBACK;
    RETURN;
END

IF NOT EXISTS (SELECT 1 FROM dbo.Shipments)
BEGIN
    PRINT 'No shipments found; skipping EmployeeShipments.';
    ROLLBACK;
    RETURN;
END

DECLARE @EmpCount INT = (SELECT COUNT(*) FROM dbo.Employees);
DECLARE @StartDate DATETIME2 = '2025-08-10';

;WITH e AS (
    SELECT EmployeeId, ROW_NUMBER() OVER (ORDER BY EmployeeId) AS ern
    FROM dbo.Employees
),
s AS (
    SELECT ShipmentId, ROW_NUMBER() OVER (ORDER BY ShipmentId) AS srn
    FROM dbo.Shipments
),
pairs AS (
    SELECT
        s.ShipmentId,
        e.EmployeeId,
        ROW_NUMBER() OVER (ORDER BY s.ShipmentId) AS rn
    FROM s
    JOIN e ON e.ern = ((s.srn - 1) % @EmpCount) + 1
)
INSERT INTO dbo.EmployeeShipments (EmployeeId, ShipmentId, [Role], [Status], CompletedAt, AssignedOn)
SELECT
    p.EmployeeId,
    p.ShipmentId,
    N'Coordinator' AS [Role],
    CASE WHEN (p.rn % 3) = 0 THEN 2  -- every 3rd completed
         WHEN (p.rn % 3) = 1 THEN 1  -- in progress
         ELSE 0                      -- planned
    END AS [Status],
    CASE WHEN (p.rn % 3) = 0 THEN DATEADD(DAY, p.rn + 2, @StartDate) ELSE NULL END AS CompletedAt,
    DATEADD(DAY, p.rn, @StartDate) AS AssignedOn
FROM pairs p
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.EmployeeShipments es
    WHERE es.EmployeeId = p.EmployeeId AND es.ShipmentId = p.ShipmentId
);

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'EmployeeShipments seed failed: ' + ERROR_MESSAGE();
END CATCH;

SELECT TOP 10 EmployeeId, ShipmentId, [Role], [Status], CompletedAt, AssignedOn
FROM dbo.EmployeeShipments
ORDER BY AssignedOn DESC;
