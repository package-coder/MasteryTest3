CREATE PROCEDURE UpdateOrderStatus(
	@clientId INT,
	@status VARCHAR(20),
    @crc VARCHAR(30)
)
AS BEGIN
	DECLARE @ordeId INT = (SELECT Id FROM [order] WHERE clientId = @clientId AND status = 'DRAFT')
	DECLARE @totalItems INT = (SELECT totalItems FROM [order] WHERE clientId = @clientId AND status = 'DRAFT')

	UPDATE [Order]
	SET 
		status = @status,
		crc = @crc + @totalItems
	WHERE
		Id = @ordeId
END