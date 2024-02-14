CREATE PROCEDURE SaveOrderItem
(
    @Id INT,
    @orderId INT,
    @productId INT,
    @quantity DECIMAL(7,2),
    @uomId INT,
    @name VARCHAR(100),
    @remark VARCHAR(100)
) AS BEGIN
    
    IF (@Id IS NULL) BEGIN 
    INSERT INTO OrderItem (orderId, productId, [name], quantity, uom, remark)
        VALUES (
                   @orderId,
                   @productId,
                   @name,
                   @quantity,
                   @uomId,
                   @remark
               )    
    END ELSE
    BEGIN 
       UPDATE [OrderItem] 
       SET 
           productId = @productId, 
           quantity = @quantity, 
           uom = @uomId, 
           name = @name, 
           remark = @remark
       WHERE Id = @Id
    END
END