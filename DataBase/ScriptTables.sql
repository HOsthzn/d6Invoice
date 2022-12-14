CREATE TABLE dbo.Client
(
	Id            INT IDENTITY
		CONSTRAINT Client_pk
			PRIMARY KEY,
	Name          VARCHAR(256) NOT NULL,
	CompanyName   VARCHAR(256),
	StreetAddress VARCHAR(256),
	CityStateZip  VARCHAR(MAX),
	Phone         NVARCHAR(256)
)
go

CREATE TABLE dbo.InvoiceHeader
(
	Id       INT IDENTITY
		CONSTRAINT InvoiceHeader_pk
			PRIMARY KEY,
	Date     DATETIME2 DEFAULT GETDATE( ) NOT NULL,
	Status   TINYINT   DEFAULT 0          NOT NULL,
	ClientId INT                          NOT NULL
		CONSTRAINT InvoiceHeader_Client_Id_fk
			REFERENCES dbo.Client
			ON DELETE CASCADE,
	DueDate  DATETIME2 DEFAULT GETDATE( ) NOT NULL,
	IsPaid   BIT       DEFAULT 0          NOT NULL
)
go

CREATE TRIGGER InvoiceHeader_UpdateOnlyActive
	ON InvoiceHeader
	FOR UPDATE , DELETE
	AS
BEGIN
	/*prevent updating invoices that are no longer in an active state
	,if an invoice is canceled or sent to a client, that invoice may no longer be changed/altered
	  (Active = 0, Canceled = 1 & Invoiced = 2 )*/
	IF (EXISTS( SELECT Id FROM deleted WHERE Status <> 0 ))
			BEGIN
				ROLLBACK
			END
END;
go

CREATE TABLE dbo.Products
(
	Id          INT IDENTITY
		CONSTRAINT Products_pk
			PRIMARY KEY,
	Description VARCHAR(256)             NOT NULL,
	Price       DECIMAL(18, 6) DEFAULT 0 NOT NULL
)
go

CREATE TABLE dbo.InvoiceDetails
(
	Id              INT IDENTITY
		CONSTRAINT InvoiceDetails_pk
			PRIMARY KEY,
	InvoiceHeaderId INT           NOT NULL
		CONSTRAINT InvoiceDetails_InvoiceHeader_Id_fk
			REFERENCES dbo.InvoiceHeader
			ON DELETE CASCADE,
	ProductId       INT           NOT NULL
		CONSTRAINT InvoiceDetails_Products_Id_fk
			REFERENCES dbo.Products,
	Quantity        INT DEFAULT 1 NOT NULL,
	Taxed           BIT DEFAULT 0 NOT NULL
)
go

CREATE TRIGGER InvoiceDetails_UpdateOnlyActive
		ON InvoiceDetails
		FOR INSERT, UPDATE , DELETE
		AS
	BEGIN
		/*prevent updating invoices that are no longer in an active state,
		  if an invoice is canceled or sent to a client, that invoice may no longer be changed/altered
	  (Active = 0, Canceled = 1 & Invoiced = 2 )*/

		IF (EXISTS(
			--we need to get the state of the invoice from the header
			--for this to work on both insert and Update we will union the insert and delete tables header id's
			--we will then join the header table to check the state
				SELECT T.InvoiceHeaderId
				FROM (
					     SELECT InvoiceHeaderId
					     FROM deleted
					     UNION
					     SELECT InvoiceHeaderId
					     FROM inserted
				     )                            AS T
					     INNER JOIN InvoiceHeader AS IH
					                ON T.InvoiceHeaderId = IH.Id
				WHERE IH.Status <> 0
			))
				BEGIN
					ROLLBACK
				END
	END;
go

