CREATE TABLE [dbo].[RolePermission]
(
	[RoleId] UNIQUEIDENTIFIER NOT NULL,
	[PermissionId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT PK_RolePermission PRIMARY KEY ([RoleId], [PermissionId]),
	CONSTRAINT [FK_RolePermission_Permission_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permission] ([Id]),
    CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE
)
