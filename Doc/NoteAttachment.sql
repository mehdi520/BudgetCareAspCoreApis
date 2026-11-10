-- Same as migration 20261110023004_NoteAttachmentsAdded.
-- Use this if you apply schema changes by hand instead of "dotnet ef database update".
CREATE TABLE [NoteAttachment] (
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
    CONSTRAINT [FK_NoteAttachment_Note] FOREIGN KEY ([note_id]) REFERENCES [Notes] ([note_id]) ON DELETE CASCADE
);
CREATE INDEX [IX_NoteAttachment_note_id] ON [NoteAttachment] ([note_id]);

-- Record the migration so EF doesn't try to run it again.
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261110023004_NoteAttachmentsAdded', N'10.0.0');
