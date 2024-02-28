CREATE PROCEDURE [dbo].[SaveOrder]
(
    @clientId INT,
    @Id INT,
    @status VARCHAR(20) = 'DRAFT',
    @visibilityLevel INT = NULL,
	@attachment VARCHAR(MAX) = NULL,
	@crc INT = NULL
) 
AS BEGIN
    IF (@Id IS NULL) BEGIN 
        INSERT INTO [Order](clientId, totalItems, [status], attachment)
		OUTPUT INSERTED.Id
        VALUES(@clientId, 0, @status, @attachment)
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
			attachment = @attachment
            OUTPUT INSERTED.Id WHERE Id = @Id
        ELSE
            UPDATE [Order] SET [status] = @status, attachment = @attachment OUTPUT INSERTED.Id WHERE Id = @Id
    END
END

