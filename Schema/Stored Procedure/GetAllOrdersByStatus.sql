CREATE PROCEDURE GetAllOrdersByStatus(
    @status VARCHAR(20),
    @clientId INT
) AS BEGIN
    SELECT
        ord.Id,
        ord.clientId,
        ord.crc,
        ord.status,
        ord.dateOrdered,
        ord.datePrinted,
        orderItem.Id,
        orderItem.orderId,
        orderItem.productId,
        orderItem.name,
        orderItem.quantity,
        orderItem.unit,
        orderItem.remark
    FROM dbo.[Order] ord
             JOIN OrderItem ON OrderItem.orderId = ord.Id
    WHERE clientId = @clientId AND status = @status AND dateDeleted IS NULL
END;

