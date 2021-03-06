SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UsernameSearchUsers]
(
	@Username	VARCHAR(22),
	@HasAnswer	BIT
)
AS
	SELECT
		u_username as Username
	FROM
		Users
	WHERE
		(u_username like @Username OR u_name like @Username)
		AND (@HasAnswer IS NULL OR EXISTS ( 
               SELECT pa_value FROM ProfileAnswers 
                    WHERE Users.u_username = ProfileAnswers.u_username))
		AND u_profilevisible = 1
		AND u_active = 1
		AND u_deleted = 0
		AND u_face_control_approved = 1
	ORDER BY u_lastonline DESC
