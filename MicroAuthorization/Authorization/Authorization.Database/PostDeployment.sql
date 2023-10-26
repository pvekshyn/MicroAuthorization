DECLARE @AdminUserId UNIQUEIDENTIFIER = '68D76B61-2532-4AA9-BC68-733E65D878B9'

DECLARE @AdminRoleId UNIQUEIDENTIFIER = '21C8DEA0-411F-4FA9-A95A-578D926F4AAD'

DECLARE @ManagePermission UNIQUEIDENTIFIER = '785027B3-FB55-4F92-860E-5EDF5B14BBF5'
DECLARE @ManageRole UNIQUEIDENTIFIER = '9eb8e349-87a3-4698-9380-44162ae88606'
DECLARE @Assign UNIQUEIDENTIFIER = 'dc879520-c6d1-4baa-acff-65785fdb4c9a'
DECLARE @ManageUser UNIQUEIDENTIFIER = '3707ee85-343d-4f2a-a856-1be689ea9cb4'

MERGE INTO [dbo].[User] AS [Target]
USING (VALUES 
	(@AdminUserId, 'Admin, User', 'admin@user.com')
	)
AS [Source] ([Id], [FullName], [Email]) ON [Target].[Id] = [Source].[Id] 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [FullName], [Email]) VALUES ([Id], [FullName], [Email]);

MERGE INTO [dbo].[Permission] AS [Target]
USING (VALUES 
	(@ManagePermission, 'Manage Permission'),
	(@ManageRole, 'Manage Role'),
	(@Assign, 'Assign'),
	(@ManageUser, 'Manage User')
	)
AS [Source] ([Id], [Name]) ON [Target].[Id] = [Source].[Id] 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name]) VALUES ([Id], [Name]);

MERGE INTO [dbo].[Role] AS [Target]
USING (VALUES 
	(@AdminRoleId, 'Admin')
	)
AS [Source] ([Id], [Name]) ON [Target].[Id] = [Source].[Id] 
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [Name]) VALUES ([Id], [Name]);

MERGE INTO [dbo].[RolePermission] AS [Target]
USING (
     SELECT @AdminRoleId, p.Id
     FROM [dbo].[Permission] p
    )
AS [Source] ([RoleId], [PermissionId]) ON [Target].[RoleId] = [Source].[RoleId] AND [Target].[PermissionId] = [Source].[PermissionId]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([RoleId], [Permissionid]) VALUES ([RoleId], [Permissionid]);

MERGE INTO [dbo].[Assignment] AS [Target]
USING (VALUES 
	(@AdminUserId, @AdminRoleId)
	)
AS [Source] ([UserId], [RoleId]) ON [Target].[UserId] = [Source].[UserId] AND [Target].[RoleId] = [Source].[RoleId]
WHEN NOT MATCHED BY TARGET THEN 
	INSERT ([Id], [UserId], [RoleId]) VALUES (NEWID(), [UserId], [RoleId]);
