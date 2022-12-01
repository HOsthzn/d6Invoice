SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Copyright © 2022 Polysphere (Pty) Ltd
-- Author:		Hendrik Oosthuizen
-- Create date: 2022/11/30
-- =============================================

CREATE OR ALTER PROCEDURE [dbo].[Client_GetDetails](
	@Id INT
                                                   )
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	--IF 0 = 1 SET FMTONLY OFF;

	SELECT TOP (1) Id, Name, Address, Suburb, State, ZipCode FROM dbo.Client WHERE Id = @Id
END;
