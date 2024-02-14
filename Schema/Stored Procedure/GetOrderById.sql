CREATE PROCEDURE GetOrderById(
	@id INT
)
AS BEGIN
    SELECT
        ord.Id,
        ord.crc,
        ord.[status],
        ord.totalItems,
        ord.dateOrdered,
        ord.datePrinted,
        appUser.Id,
        appUser.name,
        appUser.email,
        appUser.[address],
        orderItem.Id,
        orderItem.orderId,
        orderItem.productId,
        orderItem.name,
        orderItem.quantity,
        orderItem.remark,
        uom.Id,
        uom.name,
        uom.unit
    FROM
        [Order] ord
            JOIN AppUser appUser ON appUser.id = ord.clientId
            JOIN OrderItem orderItem ON orderItem.orderId = ord.Id
            JOIN UOM uom ON orderItem.uom = uom.Id
    WHERE ord.Id = @id
END
