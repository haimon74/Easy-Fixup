USE [dbezFixUp2012Jan10th]
GO

/****** Object:  StoredProcedure [dbo].[AddToFriends]    Script Date: 01/16/2012 23:28:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateFacebookMutualFriends]
(
	@Viewer	varchar(20),
	@Viewed	varchar(20),
	@MutualFriends text
)
AS
	UPDATE Friends SET fb_mutualy_friends = @MutualFriends
	WHERE @Viewer = u_username AND @Viewed = f_username

GO


