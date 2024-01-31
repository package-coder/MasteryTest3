CREATE PROCEDURE GetAllOrders(
	@clientId INT
)
AS BEGIN
		SELECT *
	FROM
		[Order]
	WHERE
		clientId = @clientId AND STATUS != 'DRAFT'
END