INSERT INTO Category (name)
VALUES
('FOOD'),('TECHNOLOGY'),('KITCHENWARE'),('TOILETRIES')

INSERT INTO UserRole (name, visibilityLevel)
VALUES
('requestor', 0),
('approver1', 1),
('approver2', 2)

INSERT INTO AppUser (name, email, [address], [role])
VALUES
('Peter Griffin', 'peter.griffin@example.com', 'Forda Street', 0),
('Glen Quagmire', 'glen.quagmire@example.com', 'Eisenhower Street', 0),
('Joe Swanson', 'joe.swanson@example.com', 'Thymes Street', 1),
('Cleveland Brown', 'cleveland.brown@example.com', 'Sesame Street', 2)

INSERT INTO UOM (name, unit)
VALUES
('PIECE', 'pc'),
('KILOGRAM', 'kg'),
('MILLILITER', 'ml'),
('GRAM', 'g'),
('CENTIMER', 'cm'),
('INCHES', 'in'),
('YARD', 'yd'),
('METER', 'm'),
('REAM', 'reams')

INSERT INTO Product (name, [description], categoryId, sku, size, color, [weight], price, photo, discontinued, uomId)
VALUES 
('Artisanal Olive Oil', 'High-quality, cold-pressed olive oil from artisanal farms.', 1, '1234567890123', 'S', 'Green', 1.5, 29.99, 'olive_oil.jpg', 0, 3),
('Smart Watch Pro', 'Stay connected and monitor your health with the latest smartwatch technology.', 2, '2345678901234', 'M', 'Black', 2.0, 99.99, 'smartwatch_pro.jpg', 0, 1),
('Copper Kitchen Utensil Set', 'Upgrade your kitchen with this stylish and durable copper utensil set.', 3, '3456789012345', 'L', 'Copper', 1.8, 49.99, 'copper_utensil_set.jpg', 0, 1),
('Organic Lavender Bath Salts', 'Transform your bath into a spa-like retreat with these soothing lavender bath salts.', 4, '4567890123456', 'S', 'Purple', 2.5, 19.99, 'lavender_bath_salts.jpg', 0, 4),
('Premium Coffee Beans Blend', 'Experience the rich flavors of a premium blend of handpicked coffee beans.', 1, '5678901234567', 'M', 'Brown', 1.2, 14.99, 'coffee_beans_blend.jpg', 0, 4),
('Convertible Laptop/Tablet Combo', 'Versatile 2-in-1 device that transforms from a laptop to a tablet for ultimate flexibility.', 2, '6789012345678', 'L', 'Silver', 1.0, 799.99, 'convertible_laptop_tablet.jpg', 0, 1),
('Stainless Steel Water Bottle', 'Stay hydrated on the go with this sleek and durable stainless steel water bottle.', 3, '7890123456789', 'S', 'Blue', 1.7, 24.99, 'water_bottle.jpg', 0, 3),
('Luxury Rose-scented Candle', 'Create a cozy atmosphere with the enchanting scent of a luxury rose-scented candle.', 4, '8901234567890', 'M', 'Pink', 2.3, 34.99, 'rose_scented_candle.jpg', 0, 1),
('Handwoven Throw Blanket', 'Add warmth and style to your home with a handwoven throw blanket.', 1, '9012345678901', 'L', 'Gray', 1.4, 39.99, 'throw_blanket.jpg', 0, 1),
('Wireless Gaming Mouse', 'Enhance your gaming experience with precision and speed using this wireless gaming mouse.', 2, '0123456789012', 'S', 'RGB', 2.2, 49.99, 'gaming_mouse.jpg', 0,1),
('Organic Apple Juice', 'Refreshing and healthy apple juice made from organic apples.', 1, '1234567890123', 'S', 'Red', 1.5, 19.99, 'apple_juice.jpg', 0, 4),
('Smartphone X Pro', 'Cutting-edge smartphone with advanced features and sleek design.', 2, '2345678901234', 'M', 'Blue', 2.0, 29.99, 'smartphone_x_pro.jpg', 0, 1),
('Chefs Stainless Steel Pot', 'Premium stainless steel pot for professional chefs, ideal for cooking delicious meals.', 3, '3456789012345', 'L', 'Silver', 1.8, 24.99, 'chef_pot.jpg', 0, 1),
('Luxury Lavender Soap', 'Indulge in the calming fragrance of luxury lavender soap for a spa-like experience.', 4, '4567890123456', 'S', 'Purple', 2.5, 39.99, 'lavender_soap.jpg', 0, 1),
('Gourmet Chocolate Bar', 'Delight your taste buds with a high-quality gourmet chocolate bar.', 1, '5678901234567', 'M', 'Brown', 1.2, 14.99, 'chocolate_bar.jpg', 0, 1),
('UltraSlim Laptop', 'Ultra-thin and lightweight laptop for on-the-go professionals and students.', 2, '6789012345678', 'L', 'Silver', 1.0, 9.99, 'ultraslim_laptop.jpg', 0, 1),
('Non-Stick Cooking Pan', 'Effortlessly prepare delicious meals with this non-stick cooking pan.', 3, '7890123456789', 'S', 'Black', 1.7, 22.99, 'cooking_pan.jpg', 0, 1),
('Luxury Shampoo and Conditioner Set', 'Pamper your hair with this luxurious shampoo and conditioner set.', 4, '8901234567890', 'M', 'White', 2.3, 34.99, 'shampoo_conditioner_set.jpg', 0,3),
('Handcrafted Ceramic Mug', 'Sip your favorite beverage in style with this handcrafted ceramic mug.', 1, '9012345678901', 'L', 'Blue', 1.4, 17.99, 'ceramic_mug.jpg', 0, 1),
('Wireless Noise-Canceling Headphones', 'Immerse yourself in music with these wireless noise-canceling headphones.', 2, '0123456789012', 'S', 'Black', 2.2, 27.99, 'headphones.jpg', 0, 1);