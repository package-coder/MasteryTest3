CREATE PROCEDURE GetDraftOrderWithItems (@clientId INT)
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
--     JOIN (
--         SELECT 
--             OrderItem.Id, 
--             OrderItem.orderId, 
--             OrderItem.productId, 
--             OrderItem.name, 
--             OrderItem.quantity, 
--             OrderItem.remark, 
--             Unit.Id, 
--             Unit.name, 
--             Unit.unit
--         FROM OrderItem JOIN dbo.UOM Unit on OrderItem.uom = Unit.Id           
--     ) orderItem ON orderId = [Order].Id
    JOIN OrderItem ON OrderItem.orderId = [Order].Id
    JOIN UOM ON OrderItem.uom = UOM.Id
    WHERE clientId = @clientId AND status = 'DRAFT'
END