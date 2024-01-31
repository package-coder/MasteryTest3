CREATE PROCEDURE GetOrderById(
	@Id INT
)
AS BEGIN
	SELECT 
		o.Id,
		crc,
		[status],
		totalItems,
		dateOrdered,
		datePrinted,
		a.id,
		a.name,
		a.email,
		a.[address]
	FROM
		[Order] o
	LEFT JOIN
		AppUser a ON a.id = o.clientId
	WHERE 
		o.Id = @Id
END
