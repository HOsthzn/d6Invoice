SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/11/30
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Product_GetDetails](
	@Id INT
                                                   )
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	--IF 0 = 1 SET FMTONLY OFF;

	SELECT TOP (1) Id, Description, Price FROM dbo.Products WHERE Id = @Id
END;
