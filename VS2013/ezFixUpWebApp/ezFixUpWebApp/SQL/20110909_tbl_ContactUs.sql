IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ContactUs_cu_reviewed]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ContactUs] DROP CONSTRAINT [DF_ContactUs_cu_reviewed]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_ContactUs_cu_datereported]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[ContactUs] DROP CONSTRAINT [DF_ContactUs_cu_datereported]
END

GO

/****** Object:  Table [dbo].[ContactUs]    Script Date: 09/10/2011 02:58:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContactUs]') AND type in (N'U'))
DROP TABLE [dbo].[ContactUs]
GO

/****** Object:  Table [dbo].[ContactUs]    Script Date: 09/10/2011 02:58:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ContactUs](
	[cu_id] [int] IDENTITY(1,1) NOT NULL,
	[cu_reportedby] [varchar](20) NOT NULL,
	[cu_type] [int] NOT NULL,
	[cu_report] [nvarchar](2000) NOT NULL,
	[cu_reporteduser] [varchar](20) NOT NULL,
	[cu_targetid] [int] NULL,
	[cu_reviewed] [bit] NOT NULL,
	[cu_datereported] [datetime] NOT NULL,
 CONSTRAINT [PK_ContactUs] PRIMARY KEY CLUSTERED 
(
	[cu_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ContactUs] ADD  CONSTRAINT [DF_ContactUs_cu_reviewed]  DEFAULT ((0)) FOR [cu_reviewed]
GO

ALTER TABLE [dbo].[ContactUs] ADD  CONSTRAINT [DF_ContactUs_cu_datereported]  DEFAULT (getdate()) FOR [cu_datereported]
GO





/****** Object:  StoredProcedure [dbo].[LoadContactUs]    Script Date: 09/10/2011 03:06:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LoadContactUs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[LoadContactUs]
GO

USE [ezFixUp14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[LoadContactUs]    Script Date: 09/10/2011 03:06:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[LoadContactUs]
(
	@ID int
)
AS
SELECT
	cu_id as [ID],
	cu_reportedby as ReportedBy,
	cu_reporteduser as ReportedUser,
	cu_type as [Type],
	cu_report as Report,
	cu_targetid as TargetId,
	cu_reviewed as Reviewed,
	cu_datereported as DateReported
FROM
	ContactUs
WHERE
	cu_id = @ID


GO




/****** Object:  StoredProcedure [dbo].[SaveContactUs]    Script Date: 09/10/2011 03:06:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveContactUs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveContactUs]
GO

USE [ezFixUp14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveContactUs]    Script Date: 09/10/2011 03:06:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SaveContactUs]
(
	@ID	INT,
	@ReportedBy	varchar(20),
	@ReportedUser varchar(250),
	@ReportType int,	
	@Report	ntext,
	@Reviewed bit,
	@ReportedItemID int,
	@DateReported datetime
)
AS
	IF (@ID IS NULL)
		INSERT INTO ContactUs
			(cu_reportedby, cu_reporteduser, cu_report, cu_type, cu_reviewed, cu_targetid, cu_datereported)
		VALUES
			(@ReportedBy, @ReportedUser, @Report, @ReportType, @Reviewed, @ReportedItemID, @DateReported)
	ELSE
		UPDATE ContactUs
		SET
			cu_reportedby = @ReportedBy,
			cu_reporteduser = @ReportedUser,
			cu_report = @Report,
			cu_type = @ReportType,
			cu_reviewed = @Reviewed,
			cu_targetid = @ReportedItemID,
			cu_datereported = @DateReported
		WHERE
			cu_id = @ID


GO



/****** Object:  StoredProcedure [dbo].[SearchContactUs]    Script Date: 09/10/2011 03:07:40 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SearchContactUs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SearchContactUs]
GO

USE [ezFixUp14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[SearchContactUs]    Script Date: 09/10/2011 03:07:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SearchContactUs]
(
	@ReportedBy	varchar(20),
	@ReportedUser varchar(250),
	@ReportType int,
	@ReportedId	int,
	@Reviewed bit,
	@FromDate datetime,
	@ToDate datetime,
	@SortColumn	varchar(50)
)
AS
SELECT
	cu_id as [ID]
FROM
	ContactUs
WHERE
	(@ReportedBy IS NULL OR @ReportedBy = cu_reportedby) AND
	(@ReportedUser IS NULL OR @ReportedUser = cu_reporteduser) AND
	((@FromDate IS NULL AND cu_datereported > '1/1/2000') OR cu_datereported > @FromDate) AND
	((@ToDate IS NULL AND cu_datereported < '1/1/2100') OR cu_datereported < @ToDate) AND
	(@ReportType IS NULL OR @ReportType = cu_type) AND
	(@ReportedId IS NULL OR @ReportedId = cu_targetid) AND
	(@Reviewed IS NULL OR @Reviewed = cu_reviewed)
ORDER BY
	CASE @SortColumn
		WHEN 'ReportedBy' THEN cu_reportedby
		WHEN 'ReportedUser' THEN cu_reporteduser
	END,
	CASE @SortColumn
		WHEN 'DateReported' THEN cu_datereported
	END,
	CASE @SortColumn
		WHEN 'ReportType' THEN cu_type
	END


GO


