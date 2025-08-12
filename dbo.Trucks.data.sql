SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;


DECLARE @Seed TABLE
(
    Model NVARCHAR(MAX) NOT NULL,
    Mileage INT NOT NULL,
    LastMaintenanceDate DATETIME2 NOT NULL,
    TruckImagePath NVARCHAR(MAX) NULL
);

INSERT INTO @Seed (Model, Mileage, LastMaintenanceDate, TruckImagePath) VALUES
(N'Volvo VNL 760',         125000, '2025-06-15', NULL),
(N'Freightliner Cascadia',  98000, '2025-05-20', NULL),
(N'Kenworth T680',         142500, '2025-04-30', NULL),
(N'Peterbilt 579',         201000, '2025-03-25', NULL),
(N'Mack Anthem',            67500, '2025-07-10', NULL),
(N'Volvo VNR 300',          45000, '2025-08-01', NULL),
(N'Freightliner M2 106',   112000, '2025-02-18', NULL),
(N'Hino 338',              131000, '2025-01-12', NULL);

;WITH s AS (
    SELECT *, ROW_NUMBER() OVER (ORDER BY Model) AS rn
    FROM @Seed
),
d AS (
    SELECT DriverId, ROW_NUMBER() OVER (ORDER BY DriverId) AS rn
    FROM dbo.Drivers
)
INSERT INTO dbo.Trucks (Model, Mileage, LastMaintenanceDate, TruckImagePath, AssignedDriverId)
SELECT 
    s.Model,
    s.Mileage,
    s.LastMaintenanceDate,
    s.TruckImagePath,
    d.DriverId   
FROM s
LEFT JOIN d ON d.rn = s.rn
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Trucks t WHERE t.Model = s.Model
);

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'Truck seed failed: ' + ERROR_MESSAGE();
END CATCH;

-- Peek
SELECT TOP 10 TruckId, Model, Mileage, LastMaintenanceDate, TruckImagePath, AssignedDriverId
FROM dbo.Trucks
ORDER BY TruckId;
