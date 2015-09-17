USE [dbezFixUp2012Jan10th]
GO

/****** Object:  StoredProcedure [dbo].[MutualFriendsSearch]    Script Date: 01/16/2012 23:21:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchFacebookMutualFriends]
	(
		@Viewer varchar(20),
		@Viewed varchar(20)
	)
AS
	SELECT fb_mutualy_friends as FacebookMutualFriends FROM Friends
		WHERE @Viewer = u_username AND
			(f_username = @Viewed)
GO


