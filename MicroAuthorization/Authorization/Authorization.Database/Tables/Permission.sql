CREATE TABLE [dbo].[Permission]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[Name] nvarchar(100) NOT NULL,
	CONSTRAINT [UQ_Permission_Name] UNIQUE([Name])
)
