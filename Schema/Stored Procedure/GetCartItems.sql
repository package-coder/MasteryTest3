CREATE PROCEDURE [dbo].[GetCartItems](
	@clientId INT
)
AS BEGIN
	SELECT 
		o.Id,
		o.orderId,
		o.name,
		o.quantity,
		o.remark,
		u.Id,
		u.unit,
		p.Id,
		p.photo,
		p.price
	FROM
		orderItem o
	LEFT JOIN
		Product p ON p.Id = o.productId
	LEFT JOIN
		UOM u ON u.Id = o.uom
	WHERE
		orderId = (SELECT Id FROM [Order] WHERE clientId = @clientId AND status = 'DRAFT')
END