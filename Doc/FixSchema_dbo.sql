/* =====================================================================
   Fix duplicate tables created in the [mehdi] schema by the EF migration.
   Keeps everything in [dbo]. Run in SSMS against BudgetCareDB.

   What it does (all-or-nothing, inside one transaction):
     1. Stops if any [mehdi] table contains data (so nothing is lost).
     2. Creates dbo.NoteAttachment (the only genuinely new table).
     3. Creates dbo.__EFMigrationsHistory with every migration marked as applied,
        so EF never re-runs them.
     4. Drops all tables in the [mehdi] schema.
   Then (outside the transaction):
     5. Drops the empty [mehdi] schema.
     6. Sets the "mehdi" login's default schema to dbo.
   ===================================================================== */

USE [BudgetCareDB];
GO

/* ---------- STEP 0 (optional): look before changing anything ---------- */
SELECT s.name AS [schema], t.name AS [table], SUM(p.rows) AS [rows]
FROM sys.tables t
JOIN sys.schemas s    ON s.schema_id = t.schema_id
JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
WHERE s.name IN ('mehdi', 'dbo')
GROUP BY s.name, t.name
ORDER BY t.name, s.name;
GO

/* ---------- STEPS 1-4 ---------- */
SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- 1. Safety check: abort if any mehdi table (other than the migrations history) has rows.
IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s    ON s.schema_id = t.schema_id
    JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
    WHERE s.name = 'mehdi' AND t.name <> '__EFMigrationsHistory'
    GROUP BY t.name
    HAVING SUM(p.rows) > 0
)
BEGIN
    ROLLBACK TRANSACTION;
    THROW 50001, 'Some [mehdi] tables contain data. Nothing was changed. Check STEP 0 output and move that data to dbo first.', 1;
END;

-- 2. dbo.NoteAttachment
IF OBJECT_ID(N'[dbo].[NoteAttachment]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[NoteAttachment] (
        [attachment_id] int NOT NULL IDENTITY(1,1),
        [note_id]       int NOT NULL,
        [user_id]       int NOT NULL,
        [file_name]     nvarchar(255) NOT NULL,
        [file_path]     nvarchar(500) NOT NULL,
        [content_type]  nvarchar(150) NOT NULL,
        [file_size]     bigint NOT NULL,
        [created_at]    datetime NOT NULL,
        [updated_at]    datetime NULL,
        CONSTRAINT [PK_NoteAttachment] PRIMARY KEY ([attachment_id]),
        CONSTRAINT [FK_NoteAttachment_Note] FOREIGN KEY ([note_id])
            REFERENCES [dbo].[Notes] ([note_id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_NoteAttachment_note_id] ON [dbo].[NoteAttachment] ([note_id]);
END;

-- 3. dbo.__EFMigrationsHistory with all migrations marked as applied
IF OBJECT_ID(N'[dbo].[__EFMigrationsHistory]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[__EFMigrationsHistory] (
        [MigrationId]    nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32)  NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

INSERT INTO [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT v.MigrationId, N'10.0.0'
FROM (VALUES
    (N'20250315200742_MigrationName'),
    (N'20250317195637_DbChanges'),
    (N'20250420050503_NoteTablesAdded'),
    (N'20250518105253_bondsDBCHanges'),
    (N'20250519074300_AnalyzeTempTableAdded'),
    (N'20250519101556_BondnumberTypeChange'),
    (N'20250519105953_AddedRelationSHip'),
    (N'20250530065522_bondflowchange'),
    (N'20250530071413_bondflowchange2'),
    (N'20250530073552_bondflowchange21'),
    (N'20250610054102_Userbond'),
    (N'20261110023004_NoteAttachmentsAdded'),
    (N'20261110043456_UseDboSchema')
) AS v(MigrationId)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[__EFMigrationsHistory] h WHERE h.MigrationId = v.MigrationId);

-- 4. Drop every table in the [mehdi] schema (foreign keys first)
DECLARE @sql nvarchar(max) = N'';

SELECT @sql += N'ALTER TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name)
             + N' DROP CONSTRAINT ' + QUOTENAME(fk.name) + N';' + CHAR(10)
FROM sys.foreign_keys fk
JOIN sys.tables  t ON t.object_id = fk.parent_object_id
JOIN sys.schemas s ON s.schema_id = t.schema_id
WHERE s.name = 'mehdi';

SELECT @sql += N'DROP TABLE ' + QUOTENAME(s.name) + N'.' + QUOTENAME(t.name) + N';' + CHAR(10)
FROM sys.tables  t
JOIN sys.schemas s ON s.schema_id = t.schema_id
WHERE s.name = 'mehdi';

PRINT @sql;
EXEC sys.sp_executesql @sql;

COMMIT TRANSACTION;
PRINT 'Done: mehdi tables dropped, dbo.NoteAttachment + dbo.__EFMigrationsHistory ready.';
GO

/* ---------- STEP 5: drop the now-empty schema (skip if it errors) ---------- */
IF SCHEMA_ID('mehdi') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.objects WHERE schema_id = SCHEMA_ID('mehdi'))
    DROP SCHEMA [mehdi];
GO

/* ---------- STEP 6: make dbo the default schema for the "mehdi" login ----------
   Needs ALTER ANY USER permission (run as sa / db owner). The app no longer depends
   on this (it now always uses dbo), but it stops this from happening in other tools. */
IF EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'mehdi')
    ALTER USER [mehdi] WITH DEFAULT_SCHEMA = [dbo];
GO

/* ---------- Check ---------- */
SELECT s.name AS [schema], t.name AS [table]
FROM sys.tables t JOIN sys.schemas s ON s.schema_id = t.schema_id
ORDER BY s.name, t.name;
SELECT * FROM [dbo].[__EFMigrationsHistory] ORDER BY MigrationId;
