CREATE PROCEDURE [dbo].[NonPaginatedResult]
AS BEGIN
	SELECT
		p.id,
		p.name,
		p.sku,
		p.[weight],
		p.price,
		p.size,
		p.color,
		u.Id,
		u.unit
	FROM
		Product p
	LEFT JOIN
		UOM u ON p.uomId = u.Id
	WHERE
		discontinued = 0 AND dateDeleted IS NULL
		
	ORDER BY
		p.name
END