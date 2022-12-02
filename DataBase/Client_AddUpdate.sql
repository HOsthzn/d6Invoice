SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/12/02
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Client_AddUpdate](
	                                                  @Id     NVARCHAR(128), @Name VARCHAR(256), @Address VARCHAR(256),
	                                                  @Suburb VARCHAR(256), @State VARCHAR(256), @ZipCode VARCHAR(10)
                                                  )
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	--IF 0 = 1 SET FMTONLY OFF;
	BEGIN TRANSACTION
		BEGIN TRY
			MERGE dbo.Client AS C
			USING (
				      SELECT @Id, @Name, @Address, @Suburb, @State, @ZipCode
			      ) AS source( iid, Nam, Adr, Sub, Sta, ZCode )
			ON (C.Id = iid)
			WHEN MATCHED THEN
				UPDATE
				SET C.Name    = Nam
				  , C.Address = Adr
				  , C.Suburb  = Sub
				  , C.State   = Sta
				  , C.ZipCode = ZCode
			WHEN NOT MATCHED THEN
				INSERT ( Name, Address, Suburb, State, ZipCode )
				VALUES ( Nam, Adr, Sub, Sta, ZCode );


			IF (@Id IS NOT NULL)
					BEGIN
						SELECT TOP (1)
							Id
							 , Name
							 , Address
							 , Suburb
							 , State
							 , ZipCode
						FROM dbo.Client
						WHERE Id = @Id
					END
				ELSE
					SELECT TOP (1)
						Id
						 , Name
						 , Address
						 , Suburb
						 , State
						 , ZipCode
					FROM dbo.Client
					WHERE Id = SCOPE_IDENTITY( )

			COMMIT TRANSACTION;
		END TRY
		BEGIN CATCH
			DECLARE
				@error NVARCHAR(MAX)
				,@message NVARCHAR(MAX)
				,@xstate NVARCHAR(MAX);

			SELECT @error = ERROR_NUMBER( )
				 , @message = 'Client_AddUpdate :' + ERROR_MESSAGE( )
				 , @xstate = XACT_STATE( );

			ROLLBACK TRANSACTION;
			RAISERROR (@message, 16, 1);
		END CATCH;
END;
