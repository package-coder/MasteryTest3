CREATE PROCEDURE [dbo].[GetCartOrder] (@clientId INT)
AS BEGIN
    SELECT
        cart.Id,
        cart.crc,
        SUM(item.quantity) totalItems,
        SUM(product.price) totalAmount
    FROM  
        [Order] cart 
            JOIN OrderItem item ON cart.Id = item.orderId
            JOIN Product product ON item.productId = product.Id
    WHERE clientId = @clientId AND status = 'DRAFT'
    group by cart.Id, cart.crc
END