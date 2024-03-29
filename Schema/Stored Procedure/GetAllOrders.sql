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
        (
            SELECT COUNT(*)
            FROM OrderItem orderItem
            WHERE orderItem.orderId = ord.Id
        ) AS 'totalItems',
        ord.dateOrdered,
        ord.dateAdded,
        ord.datePrinted,
        ord.visibilityLevel,
        appUser.Id,
        appUser.name,      
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
    JOIN AppUser appUser ON appUser.Id = ord.clientId
    FULL OUTER JOIN Product product ON orderItem.productId = product.Id	
	WHERE
	    (@clientId IS NULL OR ord.clientId = @clientId) AND
        (
            @status IS NULL OR
            (
                (@status = 'COMPLETED' AND ord.status IN ('APPROVED', 'DISAPPROVED')) OR
                (@status = 'REQUESTED' AND ord.status IN ('FOR_APPROVAL', 'APPROVED', 'DISAPPROVED')) OR
                (@status = ord.status)
            )
        ) AND
	    (@visibilityLevel IS NULL OR ord.visibilityLevel = @visibilityLevel) AND
	    ord.dateDeleted IS NULL
    ORDER BY dateOrdered DESC
END
go

