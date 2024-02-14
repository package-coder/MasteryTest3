CREATE PROCEDURE DeleteDraftOrderRequest(@Id INT)
AS BEGIN 
    UPDATE [Order] 
    SET dateDeleted = GETDATE()
    WHERE Id = @Id
END;

