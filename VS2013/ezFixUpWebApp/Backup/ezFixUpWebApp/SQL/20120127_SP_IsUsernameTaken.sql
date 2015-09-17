USE [dbezFixUp2012Jan10th]
GO
/****** Object:  StoredProcedure [dbo].[IsUsernameTaken]    Script Date: 01/27/2012 23:44:43 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[IsUsernameTaken]
(
	@Username	VARCHAR(20)
)
AS
	IF ((SELECT COUNT(*)  FROM Users 
		 WHERE (@Username = u_username) OR 
		       (u_facebookid != null and @Username = CONVERT(varchar, u_facebookid))) 
		 > 0)
	
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)

