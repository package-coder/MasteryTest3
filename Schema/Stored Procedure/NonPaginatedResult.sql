CREATE PROCEDURE NonPaginatedResult
AS BEGIN
	SELECT
		p.id,
		p.name,
		p.size,
		p.color
	FROM
		Product p
	LEFT JOIN
		Category c ON p.categoryId = c.Id
	WHERE
		discontinued != 0 AND dateDeleted = NULL
	ORDER BY
		p.name
END