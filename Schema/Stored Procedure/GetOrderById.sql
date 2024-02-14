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
        orderItem.remark
    FROM
        [Order] ord
            JOIN AppUser appUser ON appUser.id = ord.clientId
            JOIN OrderItem orderItem ON orderItem.orderId = ord.Id
    WHERE ord.Id = @id
END;

