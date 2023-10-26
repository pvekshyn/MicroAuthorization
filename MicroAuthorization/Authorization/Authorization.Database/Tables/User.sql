CREATE TABLE [dbo].[User]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[FullName] nvarchar(200) NOT NULL,
	[Email] nvarchar(200) NOT NULL
)
