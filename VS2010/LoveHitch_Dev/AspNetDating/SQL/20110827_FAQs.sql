USE [LoveHitch14thAugust2011]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChangeFAQOrder]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ChangeFAQOrder]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveFAQ]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveFAQ]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteFAQ]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteFAQ]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchFAQ]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchFAQ]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FAQs_q_active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FAQs] DROP CONSTRAINT [DF_FAQs_q_active]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FAQs_q_order]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FAQs] DROP CONSTRAINT [DF_FAQs_q_order]
END

GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  Table [dbo].[FAQs]    Script Date: 08/27/2011 16:43:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FAQs]') AND type in (N'U'))
DROP TABLE [dbo].[FAQs]
GO


USE [LoveHitch14thAugust2011]
GO

/****** Object:  Table [dbo].[FAQs]    Script Date: 08/27/2011 01:38:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[FAQs](
	[q_id] [int] IDENTITY(1,1) NOT NULL,
	[q_name] [nvarchar](50) NOT NULL,
	[q_active] [bit] NOT NULL,
	[q_order] [int] NOT NULL,
	[q_question] [varchar](250) NOT NULL,
	[q_answer] [varchar](2500) NOT NULL,
	[q_category_id] [int] NULL
 CONSTRAINT [PK_FAQs] PRIMARY KEY CLUSTERED 
(
	[q_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[FAQs] ADD  CONSTRAINT [DF_FAQs_q_active]  DEFAULT ((0)) FOR [q_active]
GO

ALTER TABLE [dbo].[FAQs] ADD  CONSTRAINT [DF_FAQs_q_order]  DEFAULT ((1)) FOR [q_order]
GO



/****** Object:  StoredProcedure [dbo].[SaveFAQ]    Script Date: 08/27/2011 02:12:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveFAQ]
(
	@Id			int = -1,
	@Name		nvarchar(50),
	@Active		bit = 1,
	@QuestionDisplay varchar(250) = null,
	@AnswerDisplay varchar(2500) = null,
	@CategoryId int = -1
)
AS

IF (@Id IS NULL)
BEGIN
	DECLARE @next_order int
	IF ((SELECT MAX(q_order) FROM FAQs) IS NULL) 
	BEGIN SET @next_order = 1 END
	ELSE BEGIN SET @next_order = 1+(SELECT MAX(q_order) FROM FAQs) END
	INSERT INTO FAQs
	(q_name, q_active, q_question, q_answer, q_order, q_category_id)
	VALUES
	(@Name, @Active, @QuestionDisplay,@AnswerDisplay, @next_order, @CategoryId)

	SELECT @@Identity
END
ELSE
	UPDATE FAQs
	SET q_name = @Name,
		q_active = @Active,
		q_question = @QuestionDisplay,
		q_answer = @AnswerDisplay,
		q_category_id = @CategoryId
	WHERE q_id = @Id
GO


/****** Object:  StoredProcedure [dbo].[DeleteFAQ]    Script Date: 08/27/2011 01:31:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteFAQ] 
	(
		@Id	int
	)
AS

DELETE FROM FAQs	WHERE	q_id = @Id

GO



/****** Object:  StoredProcedure [dbo].[FetchFAQ]    Script Date: 08/27/2011 01:36:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchFAQ] 
	(
		@Id	int,
		@Name nvarchar(50)
	)
AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_question as QuestionDisplay,
			q_answer as AnswerDisplay,
			q_category_id as CategoryId			
		FROM FAQs
			
		WHERE	(@Id = -1 OR q_id = @Id)
				AND (@Name IS NULL	OR q_name = @Name)

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchFAQs]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchFAQs]
GO

/****** Object:  StoredProcedure [dbo].[FetchFAQs]    Script Date: 08/27/2011 01:36:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchFAQs] 
	(
		@CategoryId	int
	)
AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_question as QuestionDisplay,
			q_answer as AnswerDisplay,
			q_category_id as CategoryId
			
		FROM FAQs
		WHERE q_category_id = @CategoryId
		
		ORDER BY
			q_order

GO



/****** Object:  StoredProcedure [dbo].[ChangeFAQOrder]    Script Date: 08/27/2011 02:06:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChangeFAQOrder] 
	(
		@QuestionId int,
		@CategoryId int,
		@Direction varchar(4)
	)
AS

DECLARE @order int
	SET @order = (SELECT q_order FROM FAQs WHERE q_id=@QuestionId)
	IF (@Direction = 'up')
		BEGIN
			DECLARE @previous_order int
			SET @previous_order = 
				(SELECT MAX(q_order) FROM FAQs WHERE q_order <= @order - 1 and q_category_id = @CategoryId)
			IF (@previous_order IS NOT NULL)
				BEGIN
					UPDATE FAQs
					SET q_order = @order
					WHERE q_order = @previous_order
					UPDATE FAQs
					SET q_order = @previous_order
					WHERE q_id = @QuestionId
				END
		END
	ELSE
	IF (@Direction = 'down')
		BEGIN
		DECLARE @next_order int
			SET @next_order = 
				(SELECT MIN(q_order) FROM FAQs WHERE q_order>= @order + 1)
			IF (@next_order IS NOT NULL)
				BEGIN
					UPDATE FAQs 
					SET q_order = @order
					WHERE q_order = @next_order
					UPDATE FAQs
					SET q_order = @next_order
					WHERE q_id = @QuestionId
				END
		END
GO








USE [LoveHitch14thAugust2011]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_FaqCategories_fc_active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[FaqCategories] DROP CONSTRAINT [DF_FaqCategories_fc_active]
END

GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  Table [dbo].[FaqCategories]    Script Date: 08/27/2011 20:33:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FaqCategories]') AND type in (N'U'))
DROP TABLE [dbo].[FaqCategories]
GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  Table [dbo].[FaqCategories]    Script Date: 08/27/2011 20:33:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FaqCategories](
	[fc_id] [int] IDENTITY(1,1) NOT NULL,
	[fc_name] [nvarchar](50) NULL,
	[fc_active] [bit] NOT NULL,
 CONSTRAINT [PK_FaqCategories] PRIMARY KEY CLUSTERED 
(
	[fc_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[FaqCategories] ADD  CONSTRAINT [DF_FaqCategories_fc_active]  DEFAULT ((1)) FOR [fc_active]
GO










USE [LoveHitch14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchFaqCategories]    Script Date: 08/27/2011 20:36:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchFaqCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchFaqCategories]
GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchFaqCategories]    Script Date: 08/27/2011 20:36:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[FetchFaqCategories]
	@ID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	fc_id AS[ID],
		--ecc_parentid AS ParentID,
		fc_name AS Title
FROM FaqCategories
WHERE	(@ID IS NULL OR fc_id = @ID)
		--AND (@ParentID IS NULL OR fc_parentid = @ParentID)
		AND (@Title IS NULL OR fc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN fc_name END

END

GO









USE [LoveHitch14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[DeleteFaqCategory]    Script Date: 08/27/2011 20:41:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteFaqCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteFaqCategory]
GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[DeleteFaqCategory]    Script Date: 08/27/2011 20:41:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[DeleteFaqCategory]
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM FAQs WHERE	q_category_id = @ID

DELETE FROM FAQsCategories WHERE fc_id = @ID

COMMIT



GO


USE [LoveHitch14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveFaqCategory]    Script Date: 08/27/2011 21:07:01 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveFaqCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveFaqCategory]
GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveFaqCategory]    Script Date: 08/27/2011 21:07:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SaveFaqCategory]
	@ID INT,
	@ParentID INT,
	@Active BIT,
	@Title NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @ID IS NULL
	BEGIN
		INSERT INTO FaqCategories (
		--fc_parentid, 
		fc_name, fc_active) VALUES (
		--@ParentID, 
		@Title, @Active)
		SELECT @@Identity
	END

ELSE
	UPDATE FaqCategories
	SET --fc_parentid = @ParentID,
		fc_name = @Title, fc_active = @Active
	WHERE fc_id = @ID
END


GO


/****** Object:  StoredProcedure [dbo].[SaveOrderFAQ]    Script Date: 08/28/2011 22:08:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveOrderFAQ]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveOrderFAQ]
GO


/****** Object:  StoredProcedure [dbo].[SaveOrderFAQ]    Script Date: 08/28/2011 22:08:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SaveOrderFAQ]
(
	@Id		 int = -1,
	@OrderId int = -1
)
AS

IF (@Id > 0)
BEGIN
	UPDATE FAQs SET q_order = @OrderId
	WHERE q_id = @Id
END

GO


