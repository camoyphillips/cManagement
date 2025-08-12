SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;


IF NOT EXISTS (SELECT 1 FROM dbo.Drivers)
BEGIN
    PRINT 'Aborting: no drivers in dbo.Drivers.';
    ROLLBACK;
    RETURN;
END

IF NOT EXISTS (SELECT 1 FROM dbo.Shipments)
BEGIN
    PRINT 'Aborting: no shipments in dbo.Shipments.';
    ROLLBACK;
    RETURN;
END

DECLARE @DriverCount INT = (SELECT COUNT(*) FROM dbo.Drivers);
DECLARE @BaseId INT = ISNULL((SELECT MAX(DriverShipmentId) FROM dbo.DriverShipments), 0);
DECLARE @StartDate DATETIME2 = '2025-08-10'; 

;WITH d AS (
    SELECT DriverId, ROW_NUMBER() OVER (ORDER BY DriverId) AS drn
    FROM dbo.Drivers
),
s AS (
    SELECT ShipmentId, ROW_NUMBER() OVER (ORDER BY ShipmentId) AS srn
    FROM dbo.Shipments
),
pairs AS (
    
    SELECT
        s.ShipmentId,
        d.DriverId,
        ROW_NUMBER() OVER (ORDER BY s.ShipmentId) AS rn
    FROM s
    JOIN d
      ON d.drn = ((s.srn - 1) % @DriverCount) + 1
    WHERE NOT EXISTS (SELECT 1 FROM dbo.DriverShipments ds WHERE ds.ShipmentId = s.ShipmentId)
)
INSERT INTO dbo.DriverShipments (DriverId, ShipmentId, DriverShipmentId, [Role], AssignedOn)
SELECT
    p.DriverId,
    p.ShipmentId,
    @BaseId + p.rn AS DriverShipmentId,
    N'Primary'     AS [Role],
    DATEADD(DAY, p.rn, @StartDate) AS AssignedOn
FROM pairs p;

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'Round-robin assignment failed: ' + ERROR_MESSAGE();
END CATCH;

-- Peek the latest assignments
SELECT TOP 25 DriverId, ShipmentId, DriverShipmentId, [Role], AssignedOn
FROM dbo.DriverShipments
ORDER BY DriverShipmentId DESC;
