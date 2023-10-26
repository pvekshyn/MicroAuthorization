﻿CREATE TABLE [dbo].[Assignment]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[RoleId] UNIQUEIDENTIFIER NOT NULL,
	[UserId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_Assignment_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [FK_Assignment_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]) ON DELETE CASCADE,
	CONSTRAINT [UQ_RoleId_UserId] UNIQUE([RoleId], [UserId])
)
