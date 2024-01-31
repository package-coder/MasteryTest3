CREATE PROCEDURE GetProductById(
	@Id INT
)AS BEGIN
	SELECT
		p.Id,
		p.name,
		p.[description],
		sku,
		size,
		color,
		[weight],
		price,
		photo,
		discontinued,
		dateAdded,
		dateModified,
		dateDeleted,
		c.Id,
		c.name
	FROM
		Product p 
	LEFT JOIN 
		Category c ON c.Id = p.categoryId
	WHERE 
		p.Id = @Id
END

