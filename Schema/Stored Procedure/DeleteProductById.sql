CREATE PROCEDURE DeleteProductById(
	@Id INT
)
AS BEGIN
	UPDATE Product
	SET 
		discontinued = 1,
		dateDeleted = GETDATE()
	WHERE
		Id = @Id
END