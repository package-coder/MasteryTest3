CREATE PROCEDURE [dbo].[UpdateOrderItem](
	@Id INT,
	@name VARCHAR(100),
	@uomId INT,
	@quantity INT,
	@remark VARCHAR(100)
)
AS BEGIN
	UPDATE 
		OrderItem
	SET
		name = @name,
		quantity = @quantity,
		remark = @remark,
		uom = @uomId
	WHERE
		Id = @Id
END