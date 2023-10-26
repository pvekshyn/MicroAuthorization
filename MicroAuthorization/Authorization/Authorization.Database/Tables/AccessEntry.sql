CREATE VIEW [dbo].[AccessEntry]
AS 
SELECT a.UserId, rp.PermissionId
FROM [dbo].[Assignment] AS a INNER JOIN
    [dbo].[RolePermission] AS rp ON a.RoleId = rp.RoleId;
