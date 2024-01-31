CREATE PROCEDURE DeleteOrderItem(
	@Id INT
)
AS BEGIN
	DECLARE @OrderId INT = (SELECT orderId FROM OrderItem WHERE Id = @Id)

	DELETE FROM 
		OrderItem
	WHERE
		Id = @Id

	UPDATE [Order]
	SET totalItems -= 1
	WHERE
		Id = @OrderId

	IF((SELECT totalItems FROM [Order] WHERE Id = @OrderId) = 0)
		BEGIN
			DELETE FROM
				[Order]
			WHERE
				Id = @OrderId
		END
END