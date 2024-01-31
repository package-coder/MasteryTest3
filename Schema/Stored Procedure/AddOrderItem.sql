CREATE PROCEDURE [dbo].[AddOrderItems]
(
	@ClientId INT,
	@productId INT,
	@crc VARCHAR(250),
	@quantity DECIMAL(7,2),
	@uom INT,
	@name VARCHAR(100)
)
AS BEGIN
	DECLARE @order table(Id INT)

	IF EXISTS (SELECT * FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId) BEGIN
		IF(@productId = 0)
			BEGIN
				INSERT INTO OrderItem (orderId, name, quantity, uom)
				VALUES(
					(SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId),
					@name,
					@quantity,
					@uom
				)
			END ELSE BEGIN
				INSERT INTO OrderItem (orderId, productId, name, quantity, uom)
				VALUES(
					(SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId),
					@productId,
					(SELECT name FROM Product WHERE Id = @productId),
					@quantity,
					@uom
				)
		END
	END ELSE BEGIN
		INSERT INTO [Order](clientId, crc, totalItems)
			OUTPUT INSERTED.Id INTO @order
		VALUES(@clientId, @crc, 0)

		IF(@productId = 0) BEGIN
			INSERT INTO OrderItem (orderId, name, quantity, uom)
				VALUES(
					(SELECT Id FROM @order),
					@name,
					@quantity,
					@uom
				)

		END ELSE BEGIN
			INSERT INTO OrderItem (orderId, productId, name, quantity, uom)
				VALUES(
					(SELECT Id FROM @order),
					@productId,
					(SELECT name FROM Product WHERE Id = @productId),
					@quantity,
					@uom
				)
		END
	END

	UPDATE [Order]
	SET totalItems += 1
	WHERE Id = (SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId)
END