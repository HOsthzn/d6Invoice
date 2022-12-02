SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/12/02
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Product_AddUpdate](
	                                                   @Id NVARCHAR(128), @Description VARCHAR(256), @Price VARCHAR(256)
                                                   )
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	--IF 0 = 1 SET FMTONLY OFF;
	BEGIN TRANSACTION
		BEGIN TRY
			MERGE dbo.Products AS P
			USING (
				      SELECT @Id, @Description, @Price
			      ) AS source( iid, Des, Pri )
			ON (P.Id = iid)
			WHEN MATCHED THEN
				UPDATE
				SET P.Description = Des
				  , P.Price       = Pri
			WHEN NOT MATCHED THEN
				INSERT ( Description, Price )
				VALUES ( Des, Pri );


			IF (@Id IS NOT NULL)
					BEGIN
						SELECT TOP (1)
							Id
							 , Description
							 , Price
						FROM dbo.Products
						WHERE Id = @Id
					END
				ELSE
					SELECT TOP (1)
						Id
						 , Description
						 , Price
					FROM dbo.Products
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
