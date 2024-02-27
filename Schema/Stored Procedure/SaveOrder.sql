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
        INSERT INTO [Order](clientId, totalItems, [status])
		OUTPUT INSERTED.Id
        VALUES(@clientId, 0, @status)
    END ELSE 
    BEGIN 
        DECLARE @maxLevel INT; 
        SELECT @maxLevel = MAX(visibilityLevel) FROM UserRole;

        IF(@status = 'APPROVED' AND @visibilityLevel < @maxLevel)
            UPDATE [Order] SET [visibilityLevel] = (@visibilityLevel + 1) OUTPUT INSERTED.Id WHERE Id = @Id
        ELSE
            UPDATE [Order] SET 
			[status] = @status,
			attachment =  @attachment,
			crc = @crc
		OUTPUT INSERTED.Id WHERE Id = @Id
    END
END;
