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
        orderItem.productId,
        orderItem.name,
        orderItem.quantity,
        orderItem.unit,       
        orderItem.remark
    FROM dbo.[Order]
    JOIN OrderItem ON OrderItem.orderId = [Order].Id
    WHERE clientId = @clientId AND status = 'DRAFT'
END;

