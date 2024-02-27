CREATE PROCEDURE [dbo].[SaveOrderApprovalLog]
(
    @approverId INT,
    @orderId INT,
    @status VARCHAR(20),
    @remark VARCHAR(250),
    @visibilityLevel INT
)
AS BEGIN
    INSERT INTO OrderApprovalLog(approverId, orderId, status, remark, dateLogged, visibilityLevel)
    OUTPUT INSERTED.Id
    VALUES(@approverId, @orderId, @status, @remark, GETDATE(), @visibilityLevel)
END;

