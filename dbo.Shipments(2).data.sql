SET XACT_ABORT ON;
BEGIN TRY
BEGIN TRAN;


DECLARE @DefaultTruckId INT;
SELECT TOP 1 @DefaultTruckId = TruckId FROM dbo.Trucks ORDER BY TruckId;

IF @DefaultTruckId IS NULL
BEGIN
    PRINT 'Aborting: No trucks found in dbo.Trucks. Seed Trucks before seeding Shipments.';
    ROLLBACK TRAN;
    RETURN;
END

-- Seed rows
DECLARE @Shipments TABLE
(
    ShipmentId  INT           NOT NULL,
    Origin      NVARCHAR(200) NOT NULL,
    Destination NVARCHAR(200) NOT NULL,
    Distance    INT           NOT NULL,
    [Status]    NVARCHAR(100) NOT NULL
);

INSERT INTO @Shipments (ShipmentId, Origin, Destination, Distance, [Status]) VALUES
(1001, N'Toronto, ON',      N'Montreal, QC',     541,  N'Delivered'),
(1002, N'Toronto, ON',      N'Ottawa, ON',       450,  N'Delivered'),
(1003, N'Mississauga, ON',  N'Quebec City, QC',  800,  N'InTransit'),
(1004, N'Brampton, ON',     N'Windsor, ON',      360,  N'InTransit'),
(1005, N'Hamilton, ON',     N'London, ON',       128,  N'Delivered'),
(1006, N'Toronto, ON',      N'Vancouver, BC',   4370,  N'Scheduled'),
(1007, N'Calgary, AB',      N'Edmonton, AB',     300,  N'Delivered'),
(1008, N'Winnipeg, MB',     N'Regina, SK',       575,  N'InTransit'),
(1009, N'Halifax, NS',      N'Moncton, NB',      260,  N'Scheduled'),
(1010, N'Quebec City, QC',  N'Trois-Rivières, QC',130, N'Scheduled'),
(1011, N'Toronto, ON',      N'New York, NY',     800,  N'Delivered'),
(1012, N'Buffalo, NY',      N'Toronto, ON',      160,  N'Delivered'),
(1013, N'Detroit, MI',      N'London, ON',       200,  N'InTransit'),
(1014, N'Chicago, IL',      N'Mississauga, ON',  860,  N'Scheduled'),
(1015, N'Ottawa, ON',       N'Thunder Bay, ON', 1500,  N'InTransit'),
(1016, N'Kitchener, ON',    N'Kingston, ON',     300,  N'Delivered'),
(1017, N'Montreal, QC',     N'Toronto, ON',      541,  N'InTransit'),
(1018, N'Vancouver, BC',    N'Calgary, AB',      970,  N'Scheduled'),
(1019, N'Edmonton, AB',     N'Saskatoon, SK',    520,  N'Delivered'),
(1020, N'Regina, SK',       N'Winnipeg, MB',     575,  N'InTransit');


DECLARE @IsIdentity BIT = 0;
IF COLUMNPROPERTY(OBJECT_ID('dbo.Shipments'), 'ShipmentId', 'IsIdentity') = 1
    SET @IsIdentity = 1;

IF (@IsIdentity = 1) SET IDENTITY_INSERT dbo.Shipments ON;

INSERT INTO dbo.Shipments (ShipmentId, Origin, Destination, Distance, [Status], TruckId)
SELECT s.ShipmentId, s.Origin, s.Destination, s.Distance, s.[Status], @DefaultTruckId
FROM @Shipments s
WHERE NOT EXISTS (SELECT 1 FROM dbo.Shipments x WHERE x.ShipmentId = s.ShipmentId);

IF (@IsIdentity = 1) SET IDENTITY_INSERT dbo.Shipments OFF;

COMMIT;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK;
    PRINT 'Shipments seed failed: ' + ERROR_MESSAGE();
END CATCH;

-- Peek
SELECT TOP 5 ShipmentId, Origin, Destination, Distance, [Status], TruckId
FROM dbo.Shipments
ORDER BY ShipmentId;
