CREATE PROCEDURE GetPaginatedResult(
	@offset INT,
	@next INT
)
AS BEGIN
	SELECT
		id,
		name,
		sku,
		[weight],
		price,
		size,
		color
	FROM
		Product
	WHERE
		discontinued = 0 AND dateDeleted = NULL
	ORDER BY
		name
	OFFSET @offset ROWS
	FETCH NEXT @next ROWS ONLY
END