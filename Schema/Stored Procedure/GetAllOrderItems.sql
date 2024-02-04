CREATE PROCEDURE GetAllOrderItems (
	@orderId INT
)
AS BEGIN
	SELECT 
		o.Id,
		o.name,
		quantity,
		remark,
		u.Id,
		u.unit
	FROM
		[OrderItem] o
	LEFT JOIN
		UOM u ON o.uom = u.Id
	WHERE
		orderId = @orderId
END