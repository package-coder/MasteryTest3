CREATE PROCEDURE GetOrderById(
	@id INT,
    @clientId INT = NULL
)
AS BEGIN
    SELECT
        ord.Id,
        ord.crc,
        ord.[status],
        (
            SELECT COUNT(*)
            FROM OrderItem orderItem
            WHERE orderItem.orderId = ord.Id
        ) AS 'totalItems',
		ord.attachment,
        ord.dateOrdered,
        ord.datePrinted,
        appUser.Id,
        appUser.name,
        orderItem.Id,
        orderItem.orderId,
        orderItem.name,
        orderItem.unit,
        orderItem.quantity,
        orderItem.remark,
        product.Id,
        product.name
    FROM
        [Order] ord
            JOIN OrderItem orderItem ON orderItem.orderId = ord.Id
            JOIN AppUser appUser ON appUser.Id = ord.clientId
            FULL OUTER JOIN Product product ON orderItem.productId = product.Id
    WHERE ord.Id = @id AND (@clientId IS NULL OR ord.clientId = @clientId)
END;



