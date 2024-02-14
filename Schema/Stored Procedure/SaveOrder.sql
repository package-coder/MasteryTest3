CREATE PROCEDURE [dbo].[SaveOrder]
(
    @clientId INT,
    @Id INT,
    @status VARCHAR(20) = 'DRAFT'
) 
AS BEGIN
    IF (@Id IS NULL) BEGIN 
        INSERT INTO [Order](clientId, totalItems, status)
		OUTPUT INSERTED.Id
        VALUES(@clientId, 0, @status)
    END ELSE 
    BEGIN
        UPDATE [Order] 
        SET [status] = @status
		OUTPUT INSERTED.Id
        WHERE Id = @Id
    END
END;

