CREATE PROCEDURE DeleteOrder (
	@orderId INT
)
AS BEGIN
	DELETE FROM
		OrderItem
	WHERE
		orderId = @orderId

	DELETE FROM
		[Order]
	WHERE
		Id = @orderId
END