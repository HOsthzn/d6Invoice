SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Copyright © 2022 Polysphere (Pty) Ltd
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/12/01
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Client_GetPageCount](
	@recPerPage INT = 10
                                                     )
--WITH ENCRYPTION

AS
BEGIN
	SET NOCOUNT ON;
	--IF 0 = 1 SET FMTONLY OFF;

	-- for the CEILING function to work we need to work with DECIMAL instead of int
	DECLARE  @recPerPageDec DECIMAL(18,6) = cast(@recPerPage as DECIMAL(18,6));
	SELECT  CEILING(COUNT( Id ) / @recPerPageDec) as Count  FROM dbo.Client;
END;

