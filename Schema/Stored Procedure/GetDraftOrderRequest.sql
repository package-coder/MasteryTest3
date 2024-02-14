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
        orderItem.remark,
        UOM.Id,
        UOM.name,
        UOM.unit
    FROM dbo.[Order]
    LEFT JOIN OrderItem ON OrderItem.orderId = [Order].Id
    LEFT JOIN UOM ON OrderItem.uom = UOM.Id
    WHERE clientId = @clientId AND status = 'DRAFT'
END