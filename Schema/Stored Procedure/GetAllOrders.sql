CREATE PROCEDURE GetAllOrders(
	@clientId INT = NULL,
    @status VARCHAR(20) = NULL,
    @visibilityLevel INT = NULL
)
AS BEGIN
    SELECT
        ord.Id,
        ord.clientId,
        ord.crc,
        ord.status,
        COUNT(ord.Id) as totalItems,
        ord.dateOrdered,
        ord.datePrinted,
        orderItem.Id,
        orderItem.orderId,
        orderItem.name,
        orderItem.quantity,
        orderItem.unit,
        orderItem.remark,
        product.Id,
        product.name
	FROM [Order] ord
    JOIN OrderItem orderItem ON ord.Id = orderItem.orderId
    FULL OUTER JOIN Product product ON orderItem.productId = product.Id	
	WHERE
	    (@clientId IS NULL OR ord.clientId = @clientId) AND
	    (@status IS NULL OR ord.status = @status) AND
	    (@visibilityLevel IS NULL OR ord.visibilityLevel = @visibilityLevel) AND
	    ord.dateDeleted IS NULL
    GROUP BY ord.Id,
             ord.clientId,
             ord.crc,
             ord.status,
             ord.dateOrdered,
             ord.datePrinted,
             orderItem.Id,
             orderItem.orderId,
             orderItem.name,
             orderItem.quantity,
             orderItem.unit,
             orderItem.remark,
             product.Id,
             product.name
END;

