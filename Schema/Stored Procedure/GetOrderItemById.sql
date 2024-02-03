CREATE PROCEDURE GetOrderItemById (
	@Id INT
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
		OrderItem o
	LEFT JOIN
		UOM u ON u.Id = o.uom
	WHERE
		o.Id = @Id
END