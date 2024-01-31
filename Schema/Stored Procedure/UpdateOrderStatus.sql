CREATE PROCEDURE UpdateOrderStatus(
	@Id INT,
	@status VARCHAR(20)
)
AS BEGIN
	UPDATE [Order]
	SET 
		status = @status
	WHERE
		Id = @Id
END