CREATE PROCEDURE [dbo].[GetNonDraftOrders](
	@clientId INT
)
AS BEGIN
	SELECT 
		[order].Id,
		crc,
		COUNT(orderItem.orderId) totalItems,
		[status],
		dateOrdered
	FROM
		[Order] [order]
	LEFT JOIN 
		OrderItem orderItem ON [order].Id = orderItem.orderId
	WHERE
		clientId = @clientId AND STATUS != 'DRAFT' AND dateDeleted IS NULL
	GROUP BY
		[order].Id,
		crc,
		dateOrdered,
		[status]
END