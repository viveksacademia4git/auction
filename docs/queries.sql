
-- User Table
CREATE TABLE users (
	userId INT PRIMARY KEY AUTO_INCREMENT,
	userName VARCHAR(50),
	pwd TEXT,
	fullname VARCHAR(50),
	userType INT DEFAULT 3,
    deleteFlag TINYINT(1) DEFAULT 0
);


-- Auction Item
CREATE TABLE auction_item (
	itemId INT PRIMARY KEY AUTO_INCREMENT,
    itemName VARCHAR(100),
    itemType VARCHAR(50),
    additionalInfo TEXT,
    owner VARCHAR(50),
	quantity INT DEFAULT 1,
    initialPrice INT DEFAULT 0,
	sold TINYINT(1) DEFAULT 0,
    deleteFlag TINYINT(1) DEFAULT 0
);


-- Location
CREATE TABLE location (
	locationId INT PRIMARY KEY AUTO_INCREMENT,
    locationName VARCHAR(50),
    address VARCHAR(255),
    availability TEXT,
    place VARCHAR(50),
    zipcode VARCHAR(6),
	capacity INT DEFAULT 1,
    contactPerson VARCHAR(100),
    phone VARCHAR(20),
    email VARCHAR(100),
    deleteFlag TINYINT(1) DEFAULT 0
);

-- Auction Event
CREATE TABLE auction_event (
	eventId INT PRIMARY KEY AUTO_INCREMENT,
    eventName VARCHAR(100),
    eventTime TEXT,
    registrationFee INT DEFAULT 0,
    conducted TINYINT(1) DEFAULT 0,
    buyer VARCHAR(50) DEFAULT '-',
    auctionItemId INT,
    auctionLocationId INT,
    deleteFlag TINYINT(1) DEFAULT 0
);


