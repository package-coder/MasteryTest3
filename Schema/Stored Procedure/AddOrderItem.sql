CREATE PROCEDURE [dbo].[AddOrderItems]
(
	@clientId INT,
	@productId INT,
	@quantity DECIMAL(7,2),
	@uomId INT,
	@name VARCHAR(100),
	@remark VARCHAR(100)
)
AS BEGIN
	DECLARE @order table(Id INT)

	IF EXISTS (SELECT * FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId) BEGIN
		IF(@productId = 0)
			BEGIN
				INSERT INTO OrderItem (orderId, name, quantity, uom, remark)
				VALUES(
					(SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId),
					@name,
					@quantity,
					@uomId,
					@remark
				)
			END ELSE BEGIN
				INSERT INTO OrderItem (orderId, productId, name, quantity, uom, remark)
				VALUES(
					(SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId),
					@productId,
					(SELECT name FROM Product WHERE Id = @productId),
					@quantity,
					(SELECT uomId FROM Product WHERE Id = @productId),
					@remark
				)
		END
	END ELSE BEGIN
		INSERT INTO [Order](clientId, totalItems)
			OUTPUT INSERTED.Id INTO @order
		VALUES(@clientId, 0)

		IF(@productId = 0) BEGIN
			INSERT INTO OrderItem (orderId, name, quantity, uom, remark)
				VALUES(
					(SELECT Id FROM @order),
					@name,
					@quantity,
					@uomId,
					@remark
				)

		END ELSE BEGIN
			INSERT INTO OrderItem (orderId, productId, name, quantity, uom, remark)
				VALUES(
					(SELECT Id FROM @order),
					@productId,
					(SELECT name FROM Product WHERE Id = @productId),
					@quantity,
					(SELECT uomId FROM Product WHERE Id = @productId),
					@remark
				)
		END
	END

	UPDATE [Order]
	SET totalItems += 1
	WHERE Id = (SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId)
END