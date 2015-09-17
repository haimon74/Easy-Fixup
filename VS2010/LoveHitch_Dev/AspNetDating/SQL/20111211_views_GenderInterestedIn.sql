
/****** Object:  View [dbo].[View_FemaleInterestedInFemaleSingles]    Script Date: 12/10/2011 14:53:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_FemaleInterestedInFemaleSingles]
AS
SELECT     TOP (100) PERCENT dbo.Users.u_username, dbo.Users.u_name, dbo.Users.u_gender, dbo.Users.u_birthdate, dbo.Users.u_usersince, dbo.Users.u_state, 
                      dbo.Users.u_city, dbo.Users.u_lastonline, dbo.Users.u_interested_in, dbo.Users.u_score, dbo.Users.u_credits, dbo.Users.u_status_text, dbo.Users.u_facebookid, 
                      dbo.Users.u_country, dbo.Users.u_profileviews, dbo.Users.u_myspaceid, dbo.Users.u_paid_member, dbo.Users.u_email, dbo.Photos.p_primary, 
                      dbo.Photos.p_approved, dbo.Photos.p_private, dbo.Photos.p_explicit, dbo.Photos.p_image, dbo.Photos.p_id, dbo.Photos.p_name, dbo.Users.u_signup_ip
FROM         dbo.Users LEFT OUTER JOIN
                      dbo.Photos ON dbo.Users.u_username = dbo.Photos.u_username
WHERE     (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 2) AND (dbo.Users.u_interested_in = 2) AND 
                      (dbo.Photos.p_image IS NULL) OR
                      (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 2) AND (dbo.Users.u_interested_in = 2) AND 
                      (dbo.Photos.p_private = 0) AND (dbo.Photos.p_explicit = 0) AND (dbo.Photos.p_primary = 1)
ORDER BY dbo.Users.u_lastonline DESC

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[26] 4[41] 2[18] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 588
               Right = 339
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "Photos"
            Begin Extent = 
               Top = 31
               Left = 538
               Bottom = 313
               Right = 725
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 26
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3450
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_FemaleInterestedInFemaleSingles'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_FemaleInterestedInFemaleSingles'
GO


/****** Object:  View [dbo].[View_FemaleInterestedInMaleSingles]    Script Date: 12/10/2011 14:56:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_FemaleInterestedInMaleSingles]
AS
SELECT     TOP (100) PERCENT dbo.Users.u_username, dbo.Users.u_name, dbo.Users.u_gender, dbo.Users.u_birthdate, dbo.Users.u_usersince, dbo.Users.u_state, 
                      dbo.Users.u_city, dbo.Users.u_lastonline, dbo.Users.u_interested_in, dbo.Users.u_score, dbo.Users.u_credits, dbo.Users.u_status_text, dbo.Users.u_facebookid, 
                      dbo.Users.u_country, dbo.Users.u_profileviews, dbo.Users.u_myspaceid, dbo.Users.u_paid_member, dbo.Users.u_email, dbo.Photos.p_primary, 
                      dbo.Photos.p_approved, dbo.Photos.p_private, dbo.Photos.p_explicit, dbo.Photos.p_image, dbo.Photos.p_id, dbo.Photos.p_name, dbo.Users.u_signup_ip
FROM         dbo.Users LEFT OUTER JOIN
                      dbo.Photos ON dbo.Users.u_username = dbo.Photos.u_username
WHERE     (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 2) AND (dbo.Users.u_interested_in = 1) AND 
                      (dbo.Photos.p_image IS NULL) OR
                      (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 2) AND (dbo.Users.u_interested_in = 1) AND 
                      (dbo.Photos.p_private = 0) AND (dbo.Photos.p_explicit = 0) AND (dbo.Photos.p_primary = 1)
ORDER BY dbo.Users.u_lastonline DESC

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[42] 4[23] 2[21] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 588
               Right = 339
            End
            DisplayFlags = 280
            TopColumn = 14
         End
         Begin Table = "Photos"
            Begin Extent = 
               Top = 6
               Left = 377
               Bottom = 290
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 26
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_FemaleInterestedInMaleSingles'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_FemaleInterestedInMaleSingles'
GO


/****** Object:  View [dbo].[View_MaleInterestedInFemaleSingles]    Script Date: 12/10/2011 14:56:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_MaleInterestedInFemaleSingles]
AS
SELECT     TOP (100) PERCENT dbo.Users.u_username, dbo.Users.u_name, dbo.Users.u_gender, dbo.Users.u_birthdate, dbo.Users.u_usersince, dbo.Users.u_state, 
                      dbo.Users.u_city, dbo.Users.u_lastonline, dbo.Users.u_interested_in, dbo.Users.u_score, dbo.Users.u_credits, dbo.Users.u_status_text, dbo.Users.u_facebookid, 
                      dbo.Users.u_country, dbo.Users.u_profileviews, dbo.Users.u_myspaceid, dbo.Users.u_paid_member, dbo.Users.u_email, dbo.Photos.p_primary, 
                      dbo.Photos.p_approved, dbo.Photos.p_private, dbo.Photos.p_explicit, dbo.Photos.p_image, dbo.Photos.p_id, dbo.Photos.p_name, dbo.Users.u_signup_ip
FROM         dbo.Users LEFT OUTER JOIN
                      dbo.Photos ON dbo.Users.u_username = dbo.Photos.u_username
WHERE     (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 1) AND (dbo.Users.u_interested_in = 2) AND 
                      (dbo.Photos.p_image IS NULL) OR
                      (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 1) AND (dbo.Users.u_interested_in = 2) AND 
                      (dbo.Photos.p_private = 0) AND (dbo.Photos.p_explicit = 0) AND (dbo.Photos.p_primary = 1)
ORDER BY dbo.Users.u_lastonline DESC

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[20] 4[46] 2[9] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 588
               Right = 339
            End
            DisplayFlags = 280
            TopColumn = 15
         End
         Begin Table = "Photos"
            Begin Extent = 
               Top = 6
               Left = 377
               Bottom = 114
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_MaleInterestedInFemaleSingles'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_MaleInterestedInFemaleSingles'
GO


/****** Object:  View [dbo].[View_MaleInterestedInMaleSingles]    Script Date: 12/10/2011 14:57:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_MaleInterestedInMaleSingles]
AS
SELECT     TOP (100) PERCENT dbo.Users.u_username, dbo.Users.u_name, dbo.Users.u_gender, dbo.Users.u_birthdate, dbo.Users.u_usersince, dbo.Users.u_state, 
                      dbo.Users.u_city, dbo.Users.u_lastonline, dbo.Users.u_interested_in, dbo.Users.u_score, dbo.Users.u_credits, dbo.Users.u_status_text, dbo.Users.u_facebookid, 
                      dbo.Users.u_country, dbo.Users.u_profileviews, dbo.Users.u_myspaceid, dbo.Users.u_paid_member, dbo.Users.u_email, dbo.Photos.p_primary, 
                      dbo.Photos.p_approved, dbo.Photos.p_private, dbo.Photos.p_explicit, dbo.Photos.p_image, dbo.Photos.p_id, dbo.Photos.p_name, dbo.Users.u_signup_ip
FROM         dbo.Users LEFT OUTER JOIN
                      dbo.Photos ON dbo.Users.u_username = dbo.Photos.u_username
WHERE     (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 1) AND (dbo.Users.u_interested_in = 1) AND 
                      (dbo.Photos.p_image IS NULL) OR
                      (dbo.Users.u_profilevisible = 1) AND (dbo.Users.u_active = 1) AND (dbo.Users.u_deleted = 0) AND (dbo.Users.u_gender = 1) AND (dbo.Users.u_interested_in = 1) AND 
                      (dbo.Photos.p_private = 0) AND (dbo.Photos.p_explicit = 0) AND (dbo.Photos.p_primary = 1)
ORDER BY dbo.Users.u_lastonline DESC

GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[23] 4[39] 2[35] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = -96
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 588
               Right = 339
            End
            DisplayFlags = 280
            TopColumn = 10
         End
         Begin Table = "Photos"
            Begin Extent = 
               Top = 6
               Left = 377
               Bottom = 114
               Right = 564
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_MaleInterestedInMaleSingles'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_MaleInterestedInMaleSingles'
GO


