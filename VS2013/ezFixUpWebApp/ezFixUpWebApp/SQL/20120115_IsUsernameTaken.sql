USE [dbezFixUp2012Jan10th]
GO

/****** Object:  StoredProcedure [dbo].[IsUsernameTaken]    Script Date: 01/17/2012 01:57:37 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[IsUsernameTaken]
(
	@Username	VARCHAR(20)
)
AS
	IF ((SELECT COUNT(*)  FROM Users WHERE @Username = u_username) > 0)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)

GO


