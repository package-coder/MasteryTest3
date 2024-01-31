CREATE PROCEDURE UpdateProduct
(
    @id				INT,
    @name			VARCHAR(100),
    @description	VARCHAR(MAX),
    @categoryId		INT,
    @sku			CHAR(13),
    @size			CHAR(3),
    @color			CHAR(100),
    @weight			DECIMAL(7, 2),
    @price			DECIMAL(10, 2),
    @photo			VARCHAR(MAX)
) 
AS BEGIN
	UPDATE Product
	SET
		[name] = @name,
		[description] = @description,
		sku = @sku,
		size = @size,
		color = @color,
		categoryId = @categoryId,
		[weight] = @weight,
		price = @price,
		photo = @photo,
		dateModified = GETDATE()
	WHERE 
		Id = @id
END;