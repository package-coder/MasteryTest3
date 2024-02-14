CREATE PROCEDURE DeleteOrderItem (
    @Id INT
) AS BEGIN 
    DELETE FROM OrderItem
    WHERE Id = @Id;
END;

