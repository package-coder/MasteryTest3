﻿CREATE PROCEDURE SaveOrder
(
    @clientId INT,
    @Id INT,
    @status VARCHAR(20) = 'DRAFT'
) 
AS BEGIN
    IF (@Id IS NULL) BEGIN 
        INSERT INTO [Order](clientId, totalItems, status)
        VALUES(@clientId, 0, @status)
        SELECT SCOPE_IDENTITY()
    END ELSE 
    BEGIN
        UPDATE [Order] 
        SET status = @status
        WHERE Id = @Id
    END
END