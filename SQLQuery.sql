CREATE SCHEMA poke;
GO

CREATE TABLE poke.Users (
    userID INT NOT NULL IDENTITY PRIMARY KEY,
    userName NVARCHAR(255) NOT NULL UNIQUE,
	password NVARCHAR(255) NOT NULL,
	email NVARCHAR(255) NOT NULL,
);


CREATE TABLE poke.Dex(
    pokeID INT NOT NULL IDENTITY PRIMARY KEY,
    pokemon NVARCHAR(255) NOT NULL,
);

drop table poke.Dex

CREATE TABLE poke.Cards(
    cardID INT NOT NULL IDENTITY PRIMARY KEY,
    userID INT NOT NULL,
	pokeID INT NOT NULL,
    trading BIT DEFAULT NOT NULL 0,
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

Alter table poke.Cards drop CONSTRAINT FK_Poke_ID

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

CREATE TABLE poke.TradeRequest(
	requestID INT NOT NULL IDENTITY PRIMARY KEY,
	cardID INT NOT NULL,
	userID INT NOT NULL,
	offerCardID INT NOT NULL
)

ALTER TABLE poke.TradeRequest ADD CONSTRAINT TR_FK_card_ID 
    FOREIGN KEY (cardID) REFERENCES poke.Cards(cardID);

ALTER TABLE poke.TradeRequest ADD CONSTRAINT TR_FK_user_ID 
    FOREIGN KEY (userID) REFERENCES poke.Users(userID);

ALTER TABLE poke.TradeRequest ADD CONSTRAINT TR_FK_offer_card_ID 
    FOREIGN KEY (offerCardID) REFERENCES poke.Cards(cardID);

Alter table poke.TradeRequest ADD status NVARCHAR(255) NOT NULL;
Alter table poke.TradeRequest ADD Timestamp DATETIMEOFFSET NOT NULL DEFAULT (SYSDATETIMEOFFSET())
Alter table poke.TradeRequest ADD targetUserID INT NOT NULL

select * from poke.Users;
SELECT * FROM poke.Cards;

select * from poke.TradeRequest;

SELECT * FROM poke.CompletedTrades;
select * from poke.TradeDetail;
select * from poke.Dex;


delete from poke.TradeRequest where requestID =21
delete from poke.TradeDetail where tradeID =31
delete from poke.CompletedTrades where tradeID =31


select * from poke.Cards where cardID=1 AND trading=1

-- user receive the trade requests from other user
select requestID, tr.cardID, tr.userID, offerCardID, c.pokeID, dex.pokemon, status, Timestamp
from poke.TradeRequest as tr 
join poke.Cards as c on tr.offerCardID = c.cardID 
join poke.Cards as owner on tr.cardID = owner.cardID
join poke.Dex as dex on c.pokeID = dex.pokeID
where tr.targetUserID=5

-- user send the trade requests to card owner
select requestID, tr.cardID, tr.userID, offerCardID, owner.pokeID, dex.pokemon, status, Timestamp
from poke.TradeRequest as tr
join poke.Cards as owner on tr.cardID = owner.cardID
join poke.Dex as dex on owner.pokeID = dex.pokeID
where tr.userID=4

SELECT CASE
          WHEN EXISTS (
              SELECT 1
              FROM [poke].[Cards] AS [c]
              WHERE ([c].[cardID] = 5) AND ([c].[trading] = 1)) THEN CAST(1 AS bit)
          ELSE CAST(0 AS bit)
END

UPDATE poke.Cards 
SET userID = 3
WHERE cardID = 1;
 UPDATE poke.Cards 
SET trading = 0 
WHERE cardID = 1;

insert poke.Cards values (3,2,1);
SELECT Max(tradeID) AS tradeID FROM poke.CompletedTrades where offeredBy =1 AND redeemedBy=3

select poke.TradeDetail.tradeID, p.pokemon as cardID, o.userName
                    from poke.TradeDetail
                    join poke.Dex p on poke.TradeDetail.cardID = p.pokeID
                    join poke.Users o on  poke.TradeDetail.userID = o.userID
                    where tradeID = 1;


select poke.CompletedTrades.tradeID, o.userName as offeredBy, o.userID, r.userName as redeemedBy, r.userID
from poke.CompletedTrades
join poke.Users o on  poke.CompletedTrades.offeredBy = o.userID
join poke.Users r on  poke.CompletedTrades.redeemedBy = r.userID
where (o.userName = 'test' or r.userName = 'test' );

