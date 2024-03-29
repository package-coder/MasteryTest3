CREATE PROCEDURE GetUserById (
	@userId INT
)
AS BEGIN
	SELECT 
		a.Id,
		a.name,
		a.email,
		a.[address],
		u.Id,
		u.name,
		u.visibilityLevel
	FROM 
		AppUser a
	JOIN
		UserRole u ON u.Id = a.role
	WHERE 
		a.Id = @userId
END