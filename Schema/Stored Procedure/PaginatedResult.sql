CREATE PROCEDURE GetPaginatedResult(
	@offset INT,
	@next INT
)
AS BEGIN
	SELECT
		p.Id,
		p.name,
		p.color,
		p.size
	FROM
		Product p
	LEFT JOIN
		Category c ON p.categoryId = c.Id
	WHERE
		discontinued != 0 AND dateDeleted = NULL
	ORDER BY
		p.name
	OFFSET @offset ROWS
	FETCH NEXT @next ROWS ONLY
END