CREATE PROCEDURE [dbo].[SaveOrder]
(
    @clientId INT,
    @Id INT,
    @status VARCHAR(20) = 'DRAFT',
    @visibilityLevel INT = NULL,
	@attachment VARCHAR(MAX) = NULL,
	@crc INT NULL,
	@totalItems INT
) 
AS BEGIN
    IF (@Id IS NULL) BEGIN 
        INSERT INTO [Order](clientId, totalItems, [status], attachment)
		OUTPUT INSERTED.Id
        VALUES(@clientId, @totalItems, @status, @attachment)
    END ELSE 
    BEGIN 
        DECLARE @maxLevel INT; 
        SELECT @maxLevel = MAX(visibilityLevel) FROM UserRole;

        IF(@status = 'APPROVED' AND @visibilityLevel < @maxLevel)
            UPDATE [Order] SET [visibilityLevel] = (@visibilityLevel + 1) OUTPUT INSERTED.Id WHERE Id = @Id
        ELSE IF(@status = 'FOR_APPROVAL')
            UPDATE [Order] 
            SET 
			[status] = @status, dateOrdered = GETDATE(),
			crc = @crc,
			attachment = @attachment,
			totalItems = @totalItems
            OUTPUT INSERTED.Id WHERE Id = @Id
        ELSE
            UPDATE [Order] SET [status] = @status, attachment = @attachment, totalItems = @totalItems OUTPUT INSERTED.Id WHERE Id = @Id
    END
END

