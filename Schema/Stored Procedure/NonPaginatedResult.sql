CREATE PROCEDURE NonPaginatedResult
AS BEGIN
	SELECT
		p.id,
		p.name,
		p.sku,
		p.[weight],
		p.price,
		p.size,
		p.color
	FROM
		Product p
	WHERE
		discontinued = 0 AND dateDeleted IS NULL
	ORDER BY
		p.name
END