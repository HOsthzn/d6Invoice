SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/11/30
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Product_Get](
	                                            @page INT = 0, @recPerPage INT =10
                                            )
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	--IF 0 = 1 SET FMTONLY OFF;

	SELECT Id, Description, Price
	FROM dbo.Products
	ORDER BY Id
	OFFSET (@page * @recPerPage) ROWS FETCH NEXT @recPerPage ROWS ONLY;
END;
