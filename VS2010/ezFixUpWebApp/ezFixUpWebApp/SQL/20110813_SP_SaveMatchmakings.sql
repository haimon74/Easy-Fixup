IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveMatchmakings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveMatchmakings]
GO

/****** Object:  StoredProcedure [dbo].[SaveMatchmakings]    Script Date: 08/13/2011 18:10:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SaveMatchmakings]
(
	@ID int=null,
	@MatchmakerUsername nvarchar (20),
	@MatchmakingStatus int = NULL,
	@MatchToUsername nvarchar (20),
	@MatchWithUsername nvarchar (20),
	@MatchToAck bit,
	@MatchWithAck bit,
	@Date datetime
)
 AS
	IF (@ID IS NULL)
		INSERT INTO Matchmakings (
			mm_matchmaker_username, 
			mm_status, 
			mm_timestamp,
			mm_to_friend_username,
			mm_to_friend_ack, 
			mm_with_friend_username, 
			mm_with_friend_ack)
		VALUES
			(@MatchmakerUsername, @MatchmakingStatus, @Date,
			@MatchToUsername, @MatchToAck, @MatchWithUsername, @MatchWithAck)
	ELSE
		UPDATE Matchmakings
		SET
			mm_matchmaker_username = @MatchmakerUsername, 
			mm_status = @MatchmakingStatus, 
			mm_timestamp = @Date,
			mm_to_friend_username = @MatchToUsername,
			mm_to_friend_ack = @MatchToAck, 
			mm_with_friend_username = @MatchWithUsername, 
			mm_with_friend_ack = @MatchWithAck
		WHERE
			mm_id = @ID

GO


