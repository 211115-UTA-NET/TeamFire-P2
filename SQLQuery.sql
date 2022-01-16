CREATE SCHEMA poke;
GO

CREATE TABLE poke.Users (
    userID INT NOT NULL IDENTITY PRIMARY KEY,
    userName NVARCHAR(255) NOT NULL UNIQUE,
	password NVARCHAR(255) NOT NULL,
	email NVARCHAR(255) NOT NULL,
);


CREATE TABLE poke.Dex(
    pokeID INT NOT NULL PRIMARY KEY,
    pokemon NVARCHAR(255) NOT NULL,
);

CREATE TABLE poke.Cards(
    cardID INT NOT NULL IDENTITY PRIMARY KEY,
    userID INT NOT NULL,
	pokeID INT NOT NULL,
    is_tradeable BIT DEFAULT 0,
);

CREATE TABLE poke.CompletedTrades (
    tradeID INT NOT NULL IDENTITY PRIMARY KEY,
    offeredBy INT NOT NULL,
    redeemedBy INT NOT NULL,
    Timestamp DATETIMEOFFSET NOT NULL DEFAULT (SYSDATETIMEOFFSET()),
);

CREATE TABLE poke.TradeDetail(
	ID INT NOT NULL IDENTITY PRIMARY KEY,
	tradeID INT NOT NULL,
	cardID INT NOT NULL,
	userID INT NOT NULL
)

ALTER TABLE poke.Cards ADD CONSTRAINT FK_User_ID 
    FOREIGN KEY (userID) REFERENCES poke.Users(userID);

ALTER TABLE poke.Cards ADD CONSTRAINT FK_Poke_ID 
    FOREIGN KEY (pokeID) REFERENCES poke.Dex(pokeID);

ALTER TABLE poke.CompletedTrades ADD CONSTRAINT FK_offeredBy_User_ID 
    FOREIGN KEY (offeredBy) REFERENCES poke.Users(userID);

ALTER TABLE poke.CompletedTrades ADD CONSTRAINT FK_redeemedBy_User_ID 
    FOREIGN KEY (redeemedBy) REFERENCES poke.Users(userID);

ALTER TABLE poke.TradeDetail ADD CONSTRAINT FK_trade_ID 
    FOREIGN KEY (tradeID) REFERENCES poke.CompletedTrades(tradeID);

ALTER TABLE poke.TradeDetail ADD CONSTRAINT FK_card_ID 
    FOREIGN KEY (cardID) REFERENCES poke.Cards(cardID);