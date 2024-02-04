CREATE PROCEDURE [dbo].[GetCartItems](
	@clientId INT
)
AS BEGIN
	SELECT
	    item.id, 
	    item.name,
        item.quantity, 
	    item.remark,
	    product.Id,
        product.price,
        product.photo,
        product.sku,
	    unit.id, 
	    unit.name, 
	    unit.unit
	FROM
		OrderItem item 
		    JOIN UOM unit ON unit.Id = item.uom
	        FULL OUTER JOIN Product product ON product.Id = item.productId
	WHERE
		orderId = (SELECT Id FROM [Order] WHERE clientId = @clientId AND status = 'DRAFT')
END