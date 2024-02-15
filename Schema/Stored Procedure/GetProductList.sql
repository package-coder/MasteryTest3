CREATE PROCEDURE [dbo].[GetProductList]
AS BEGIN
	SELECT
		p.id,
		p.name,
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