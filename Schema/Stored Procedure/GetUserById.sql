CREATE PROCEDURE GetOrderItems (
	@Id INT
)
AS BEGIN
	SELECT
		name,
		quantity,
		UOM,
		remark
	FROM
		OrderItem
	WHERE
		Id = @Id
END