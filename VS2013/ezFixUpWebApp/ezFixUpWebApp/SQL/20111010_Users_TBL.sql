--use ezFixUp20111011

ALTER TABLE [dbo].[Users] 
DROP CONSTRAINT [DF_Users_u_country]
GO

ALTER TABLE [dbo].[Users]
DROP COLUMN u_country
GO

ALTER TABLE [dbo].[Users]
ADD u_country nvarchar(20)
GO

ALTER TABLE [dbo].[Users] 
ADD  CONSTRAINT [DF_Users_u_country]  DEFAULT ('') FOR [u_country]

UPDATE [dbo].[Users]
   SET [u_country] = N'�����'
      ,[u_state] = N'�� ����'
      ,[u_city] = N'�� ���� - ���'
GO


