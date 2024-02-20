CREATE PROCEDURE GetUserById (
	@userId INT
)
AS BEGIN
	SELECT 
		Id,
		name,
		[address],
		email
	FROM
		AppUser
	WHERE
		Id = @userId
END