CREATE PROCEDURE GetUserById (
	@userId INT
)
AS BEGIN
	SELECT *
	FROM
		AppUser
	WHERE
		Id = @userId
END;

