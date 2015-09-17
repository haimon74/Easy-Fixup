
/****** Object:  StoredProcedure [dbo].[FetchMatchmakings]    Script Date: 08/08/2011 22:03:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchMatchmakings]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchMatchmakings]
GO

USE [LoveHitchDev15June2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchMatchmakings]    Script Date: 08/08/2011 22:03:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[FetchMatchmakings]
(
	@Id			INT = 0,
	@MatchmakerUsername	VARCHAR(20) = null,
	@ToFriendUsername	VARCHAR(20) = null,
	@WithFriendUsername	VARCHAR(20) = null,
	@Status		VARCHAR(20) = null,
	@ToFriendAck	BIT = null,
	@WithFriendAck	BIT = null	
)
AS
	SELECT TOP 100
		mm_id			AS Id,
		mm_matchmaker_username AS MatchmakerUsername,
		mm_to_friend_username AS ToFriendUsername,
		mm_with_friend_username AS WithFriendUsername,
		mm_status AS MatchStatus,
		mm_to_friend_ack AS ToFriendAck,
		mm_with_friend_ack AS WithFriendAck,
		mm_timestamp AS MatchDateTime
		
	FROM Matchmakings
	WHERE (@Id = 0 OR @Id = mm_id)
	AND (@MatchmakerUsername IS NULL OR @MatchmakerUsername = mm_matchmaker_username)
	AND (@ToFriendUsername IS NULL OR @ToFriendUsername = mm_to_friend_username)
	AND (@WithFriendUsername IS NULL OR @WithFriendUsername = mm_with_friend_username)
	AND (@Status IS NULL OR @Status = mm_status)
	AND (@ToFriendAck IS NULL OR mm_to_friend_ack = @ToFriendAck)
	AND (@WithFriendAck IS NULL OR mm_with_friend_ack = @WithFriendAck)
	ORDER BY mm_timestamp DESC


GO


