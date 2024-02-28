CREATE PROCEDURE GetAllApprovals(
	@orderId INT
)
AS
BEGIN
	SELECT 
	orderApproval.Id,
	[status],
	dateLogged,
	remark,
	appUser.Id,
	appUser.name
	FROM 
		OrderApprovalLog orderApproval
	JOIN
		AppUser appUser ON approverId = appUser.Id
	WHERE 
		OrderId = @orderId
END
