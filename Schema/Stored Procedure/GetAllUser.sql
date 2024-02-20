CREATE PROCEDURE GetAllUsers
AS BEGIN
	SELECT
		a.Id,
		a.[name],
		a.email,
		a.[address],
		u.Id,
		u.name
	FROM 
		AppUser a
	JOIN
		UserRole u ON u.Id = a.role
END