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
    DECLARE @orderId INT = (SELECT Id FROM [Order] WHERE status = 'DRAFT' AND clientId = @ClientId)

    IF (@orderId IS NOT NULL) BEGIN
        INSERT INTO OrderItem (orderId, productId, [name], quantity, uom, remark)
        VALUES (
                   @orderId,
                   @productId,
                   @name,
                   @quantity,
                   @uomId,
                   @remark
               )
    END
    ELSE BEGIN
        DECLARE @order table(Id INT)

        INSERT INTO [Order](clientId, totalItems)
        OUTPUT INSERTED.Id INTO @order
        VALUES(@clientId, 0)

        INSERT INTO OrderItem (orderId, productId, [name], quantity, uom, remark)
        VALUES(
                  (SELECT Id FROM @order),
                  @productId,
                  @name,
                  @quantity,
                  @uomId,
                  @remark
              )
    END
END;