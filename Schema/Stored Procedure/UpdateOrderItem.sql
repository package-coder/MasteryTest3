CREATE PROCEDURE UpdateOrderItem(
	@Id INT,
	@productId INT,
	@name VARCHAR(100),
	@uom INT,
	@quantity INT,
	@remark VARCHAR(100)
)
AS BEGIN

	IF(@ProductId = 0)
		BEGIN
			UPDATE OrderItem
			SET
				name = @name,
				quantity = @quantity,
				uom = @uom,
				remark = @remark
			WHERE
				Id = @Id
		END ELSE BEGIN
			Update OrderItem
			SET
				productId = @productId,
				name = (SELECT name FROM Product WHERE Id = @Id),
				quantity = @quantity,
				uom = @uom,
				remark = @remark
			WHERE 
				Id = @Id
		END
END