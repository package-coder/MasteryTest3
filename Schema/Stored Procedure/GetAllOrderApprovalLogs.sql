CREATE PROCEDURE GetAllOrderApprovalLogs(
    @approverId INT = NULL,
    @client INT = NULL
) AS BEGIN 
    SELECT 
        approvalLog.Id,
        approvalLog.dateLogged,
        approvalLog.remark,
        approvalLog.status,
        approvalLog.visibilityLevel,
        ord.Id,
        ord.dateOrdered,
        ord.totalItems,
        ord.status,
        appUser.Id,
        appUser.name
    FROM dbo.OrderApprovalLog approvalLog
    JOIN dbo.[Order] ord ON ord.Id = approvalLog.orderId
    JOIN AppUser appUser ON appUser.Id = ord.clientId
    WHERE 
        (@approverId IS NULL OR approverId = @approverId) AND
        (@client IS NULL OR ord.clientId = @client)
    ORDER BY dateLogged DESC 
END
go

