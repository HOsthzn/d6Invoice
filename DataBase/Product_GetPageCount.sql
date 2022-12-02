SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/12/01
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Product_GetPageCount](
	@recPerPage INT = 10
                                                     )
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	-- 	IF 0 = 1 SET FMTONLY OFF;

	-- for the CEILING function to work we need to work with DECIMAL instead of int
	DECLARE @recPerPageDec DECIMAL(18, 6) = CAST( @recPerPage AS DECIMAL(18, 6) );
	SELECT cast(CEILING( COUNT( Id ) / @recPerPageDec ) as INT) AS Count FROM dbo.Products;
END;