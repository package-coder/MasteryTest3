DROP TABLE IF EXISTS OrderItem;
DROP TABLE IF EXISTS [Order];
DROP TABLE IF EXISTS Product;
DROP TABLE IF EXISTS UOM;
DROP TABLE IF EXISTS AppUser;
DROP TABLE IF EXISTS Category;

CREATE TABLE Category(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	name VARCHAR(100) NOT NULL
);

CREATE TABLE AppUser(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	name VARCHAR(100) NOT NULL,
	email VARCHAR(100) NOT NULL,
	[address] VARCHAR(100) NOT NULL
);

CREATE TABLE UOM (
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
	name VARCHAR(100) NOT NULL,
	unit CHAR(10) NOT NULL
);
 
CREATE TABLE Product (
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	name VARCHAR(100) NOT NULL,
	[description] VARCHAR(MAX) NOT NULL,
	categoryId INT FOREIGN KEY REFERENCES Category(Id),
	uomId INT FOREIGN KEY REFERENCES UOM(Id),
	sku CHAR(13) NOT NULL,
	size CHAR(1) NOT NULL,
	color VARCHAR(100) NOT NULL,
	[weight] DECIMAL (7,2) DEFAULT(0.0),
	price DECIMAL(10,2) DEFAULT(0.0),
	photo VARCHAR(MAX),
	discontinued BIT DEFAULT(0),
	dateAdded DATETIME DEFAULT CURRENT_TIMESTAMP,
	dateModified DATETIME,
	dateDeleted DATETIME
);

CREATE TABLE [Order](
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	clientId INT FOREIGN KEY REFERENCES AppUser(Id),
	crc VARCHAR(250) NULL,
	[status] VARCHAR(20) DEFAULT('DRAFT'), --APPROVED, DISAPPROVED, FOR_APPROVAL, DRAFT
	totalItems INT NOT NULL,
	dateOrdered DATETIME DEFAULT CURRENT_TIMESTAMP,
	datePrinted DATETIME,
);

CREATE TABLE OrderItem(
	[Id] INT PRIMARY KEY IDENTITY(1,1),
	orderId INT FOREIGN KEY REFERENCES [Order](Id),
	productId INT FOREIGN KEY REFERENCES Product(Id),
	name VARCHAR(100) NOT NULL,
	quantity INT NOT NULL,
	uom INT FOREIGN KEY REFERENCES UOM(Id),
	remark VARCHAR(100),
);