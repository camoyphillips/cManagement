

SET XACT_ABORT ON;
BEGIN TRAN;


DECLARE @DriversSeed TABLE (
    DriverId       INT            NOT NULL,
    [Name]         NVARCHAR(100)  NOT NULL,
    LicenseNumber  NVARCHAR(50)   NOT NULL,
    ContactNumber  NVARCHAR(50)   NULL,
    Email          NVARCHAR(256)  NOT NULL
);

INSERT INTO @DriversSeed (DriverId, [Name], LicenseNumber, ContactNumber, Email)
VALUES
    (1,  N'Christine',        N'ABC',     NULL, N'christine@example.com'),
    (2,  N'Sean',             N'DEF',     NULL, N'sean@example.com'),
    (26, N'Christine',        N'ABC123',  NULL, N'christine2@example.com'),
    (27, N'Sean',             N'DEF456',  NULL, N'sean2@example.com'),
    (28, N'Camoy',            N'GHI789',  NULL, N'camoy@example.com'),
    (29, N'Khani',            N'JKL012',  NULL, N'khani@example.com'),
    (30, N'Phillips',         N'MNO345',  NULL, N'phillips@example.com'),
    (31, N'Gary',             N'PQR678',  NULL, N'gary@example.com'),
    (32, N'Lance',            N'STU901',  NULL, N'lance@example.com'),
    (33, N'Caranetia',        N'VWX234',  NULL, N'caranetia@example.com'),
    (34, N'Doreen',           N'YZA567',  NULL, N'doreen@example.com'),
    (35, N'Shamoy',           N'BCD890',  NULL, N'shamoy@example.com'),
    (36, N'Khadoy',           N'CDE321',  NULL, N'khadoy@example.com'),
    (37, N'Sophia',           N'FGH654',  NULL, N'sophia@example.com'),
    (38, N'Marvinique',       N'IJK987',  NULL, N'marvinique@example.com'),
    (39, N'Emma',             N'LMN210',  NULL, N'emma@example.com'),
    (40, N'Jacob',            N'OPQ543',  NULL, N'jacob@example.com'),
    (41, N'Isabella',         N'RST876',  NULL, N'isabella@example.com'),
    (42, N'William',          N'UVW109',  NULL, N'william@example.com'),
    (43, N'Olivia',           N'XYZ432',  NULL, N'olivia@example.com'),
    (44, N'James',            N'ABC765',  NULL, N'james@example.com'),
    (45, N'Ava',              N'DEF098',  NULL, N'ava@example.com'),
    (46, N'Benjamin',         N'GHI321',  NULL, N'benjamin@example.com'),
    (47, N'Mia',              N'JKL654',  NULL, N'mia@example.com'),
    (48, N'Logan',            N'MNO987',  NULL, N'logan@example.com'),
    (49, N'Charlotte',        N'PQR210',  NULL, N'charlotte@example.com'),

    (50, N'John Doe',         N'L12345',  NULL, N'john.doe@example.com'),
    (51, N'Jane Smith',       N'L12346',  NULL, N'jane.smith@example.com'),
    (52, N'Robert Johnson',   N'L12347',  NULL, N'robert.johnson@example.com'),
    (53, N'Emily Davis',      N'L12348',  NULL, N'emily.davis@example.com'),
    (54, N'Michael Brown',    N'L12349',  NULL, N'michael.brown@example.com'),
    (55, N'Jessica Miller',   N'L12350',  NULL, N'jessica.miller@example.com'),
    (56, N'David Wilson',     N'L12351',  NULL, N'david.wilson@example.com'),
    (57, N'Sarah Moore',      N'L12352',  NULL, N'sarah.moore@example.com'),
    (58, N'Chris Taylor',     N'L12353',  NULL, N'chris.taylor@example.com'),
    (59, N'Laura Anderson',   N'L12354',  NULL, N'laura.anderson@example.com'),
    (60, N'James Thomas',     N'L12355',  NULL, N'james.thomas@example.com'),
    (61, N'Mary Jackson',     N'L12356',  NULL, N'mary.jackson@example.com'),
    (62, N'Daniel White',     N'L12357',  NULL, N'daniel.white@example.com'),
    (63, N'Olivia Harris',    N'L12358',  NULL, N'olivia.harris@example.com'),
    (64, N'William Martin',   N'L12359',  NULL, N'william.martin@example.com'),
    (65, N'Sophia Thompson',  N'L12360',  NULL, N'sophia.thompson@example.com'),
    (66, N'Joseph Garcia',    N'L12361',  NULL, N'joseph.garcia@example.com'),
    (67, N'Ava Martinez',     N'L12362',  NULL, N'ava.martinez@example.com'),
    (68, N'Matthew Robinson', N'L12363',  NULL, N'matthew.robinson@example.com'),
    (69, N'Isabella Clark',   N'L12364',  NULL, N'isabella.clark@example.com'),
    (70, N'Andrew Rodriguez', N'L12365',  NULL, N'andrew.rodriguez@example.com'),
    (71, N'Mia Lewis',        N'L12366',  NULL, N'mia.lewis@example.com'),
    (72, N'Ethan Lee',        N'L12367',  NULL, N'ethan.lee@example.com'),
    (73, N'Charlotte Walker', N'L12368',  NULL, N'charlotte.walker@example.com'),

    -- Duplicates with unique emails
    (74, N'John Doe',         N'L12345',  NULL, N'john.doe2@example.com'),
    (75, N'Jane Smith',       N'L12346',  NULL, N'jane.smith2@example.com'),
    (76, N'Robert Johnson',   N'L12347',  NULL, N'robert.johnson2@example.com'),
    (77, N'Emily Davis',      N'L12348',  NULL, N'emily.davis2@example.com'),
    (78, N'Michael Brown',    N'L12349',  NULL, N'michael.brown2@example.com'),
    (79, N'Jessica Miller',   N'L12350',  NULL, N'jessica.miller2@example.com'),
    (80, N'David Wilson',     N'L12351',  NULL, N'david.wilson2@example.com'),
    (81, N'Sarah Moore',      N'L12352',  NULL, N'sarah.moore2@example.com'),
    (82, N'Chris Taylor',     N'L12353',  NULL, N'chris.taylor2@example.com'),
    (83, N'Laura Anderson',   N'L12354',  NULL, N'laura.anderson2@example.com'),
    (84, N'James Thomas',     N'L12355',  NULL, N'james.thomas2@example.com'),
    (85, N'Mary Jackson',     N'L12356',  NULL, N'mary.jackson2@example.com'),
    (86, N'Daniel White',     N'L12357',  NULL, N'daniel.white2@example.com'),
    (87, N'Olivia Harris',    N'L12358',  NULL, N'olivia.harris2@example.com'),
    (88, N'William Martin',   N'L12359',  NULL, N'william.martin2@example.com'),
    (89, N'Sophia Thompson',  N'L12360',  NULL, N'sophia.thompson2@example.com'),
    (90, N'Joseph Garcia',    N'L12361',  NULL, N'joseph.garcia2@example.com'),
    (91, N'Ava Martinez',     N'L12362',  NULL, N'ava.martinez2@example.com'),
    (92, N'Matthew Robinson', N'L12363',  NULL, N'matthew.robinson2@example.com'),
    (93, N'Isabella Clark',   N'L12364',  NULL, N'isabella.clark2@example.com'),
    (94, N'Andrew Rodriguez', N'L12365',  NULL, N'andrew.rodriguez2@example.com'),
    (95, N'Mia Lewis',        N'L12366',  NULL, N'mia.lewis2@example.com'),
    (96, N'Ethan Lee',        N'L12367',  NULL, N'ethan.lee2@example.com'),
    (97, N'Charlotte Walker', N'L12368',  NULL, N'charlotte.walker2@example.com');


INSERT INTO [dbo].[AspNetUsers] (
    [Id],
    [UserName],
    [NormalizedUserName],
    [Email],
    [NormalizedEmail],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [ConcurrencyStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEnd],
    [LockoutEnabled],
    [AccessFailedCount]
)
SELECT
    NEWID()                                  AS [Id],
    s.Email                                  AS [UserName],
    UPPER(s.Email)                           AS [NormalizedUserName],
    s.Email                                  AS [Email],
    UPPER(s.Email)                           AS [NormalizedEmail],
    0                                        AS [EmailConfirmed],
    NULL                                     AS [PasswordHash],      
    CONVERT(NVARCHAR(36), NEWID())           AS [SecurityStamp],
    CONVERT(NVARCHAR(36), NEWID())           AS [ConcurrencyStamp],
    NULL                                     AS [PhoneNumber],
    0                                        AS [PhoneNumberConfirmed],
    0                                        AS [TwoFactorEnabled],
    NULL                                     AS [LockoutEnd],
    1                                        AS [LockoutEnabled],
    0                                        AS [AccessFailedCount]
FROM @DriversSeed s
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[AspNetUsers] u WHERE u.[UserName] = s.Email
);


SET IDENTITY_INSERT [dbo].[Drivers] ON;

INSERT INTO [dbo].[Drivers] (
    [DriverId],
    [Name],
    [LicenseNumber],
    [ContactNumber],
    [ApplicationUserId]
)
SELECT
    s.DriverId,
    s.[Name],
    s.LicenseNumber,
    s.ContactNumber,
    u.[Id] AS ApplicationUserId
FROM @DriversSeed s
JOIN [dbo].[AspNetUsers] u
    ON u.[UserName] = s.Email
WHERE NOT EXISTS (
    SELECT 1 FROM [dbo].[Drivers] d WHERE d.[DriverId] = s.DriverId
);

SET IDENTITY_INSERT [dbo].[Drivers] OFF;


COMMIT;

SELECT COUNT(*) AS InsertedUsers
FROM [dbo].[AspNetUsers] u
WHERE u.[UserName] IN (SELECT Email FROM @DriversSeed);

SELECT COUNT(*) AS InsertedDrivers
FROM [dbo].[Drivers] d
WHERE d.[DriverId] IN (SELECT DriverId FROM @DriversSeed);
