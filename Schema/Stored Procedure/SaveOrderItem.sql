CREATE PROCEDURE SaveOrderItem
(
    @Id INT,
    @orderId INT,
    @productId INT,
    @quantity DECIMAL(7,2),
    @unit VARCHAR(20),
    @name VARCHAR(100),
    @remark VARCHAR(100)
) AS BEGIN
    
    IF (@Id IS NULL) BEGIN 
    INSERT INTO OrderItem (orderId, productId, [name], quantity, unit, remark)
        VALUES (
                   @orderId,
                   @productId,
                   @name,
                   @quantity,
                   @unit,
                   @remark
               )    
    END ELSE
    BEGIN 
       UPDATE [OrderItem] 
       SET 
           productId = @productId, 
           quantity = @quantity, 
           unit = @unit, 
           name = @name, 
           remark = @remark
       WHERE Id = @Id
    END
END;

