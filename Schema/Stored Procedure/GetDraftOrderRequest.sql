CREATE PROCEDURE GetDraftOrderRequest (@clientId INT)
AS BEGIN
    SELECT  
		[Order].Id,
		clientId,
		crc,
		status,
		totalItems,
		dateOrdered,
		datePrinted,
		orderItem.Id,
		orderItem.orderId,
		orderItem.name,
		orderItem.quantity,
		orderItem.unit,       
		orderItem.remark,
		Product.Id
	FROM dbo.[Order]
	JOIN OrderItem ON OrderItem.orderId = [Order].Id
	LEFT JOIN Product On OrderItem.productId = Product.Id
	WHERE clientId = 1 AND status = 'DRAFT' AND [Order].dateDeleted IS NULL
END;

