USE [ezFixUp7DB]
GO
/****** Object:  Table [dbo].[ProfileQuestions]    Script Date: 01/11/2011 23:25:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileQuestions](
	[pq_id] [int] IDENTITY(1,1) NOT NULL,
	[pt_id] [int] NOT NULL,
	[pq_name] [nvarchar](250) NOT NULL,
	[pq_altname] [nvarchar](250) NOT NULL,
	[pq_description] [nvarchar](4000) NOT NULL,
	[pq_hint] [nvarchar](250) NOT NULL,
	[pq_edit_style] [int] NOT NULL,
	[pq_show_style] [int] NOT NULL,
	[pq_search_style] [int] NOT NULL,
	[pq_required] [bit] NOT NULL,
	[pq_priority] [int] NOT NULL,
	[pq_requires_approval] [bit] NOT NULL,
	[pq_visible_male] [bit] NOT NULL,
	[pq_visible_female] [bit] NOT NULL,
	[pq_visible_couple] [bit] NOT NULL,
	[pq_match_field] [int] NULL,
	[pq_viewpaidonly] [bit] NOT NULL,
	[pq_editpaidonly] [bit] NOT NULL,
	[pq_parent_question_id] [int] NULL,
	[pq_parent_question_choices] [nvarchar](3000) NULL,
 CONSTRAINT [PK_ProfileFields] PRIMARY KEY CLUSTERED 
(
	[pq_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProfileQuestions]  WITH NOCHECK ADD  CONSTRAINT [FK_ProfileFields_ProfileCategories] FOREIGN KEY([pt_id])
REFERENCES [dbo].[ProfileTopics] ([pt_id])
GO
ALTER TABLE [dbo].[ProfileQuestions] CHECK CONSTRAINT [FK_ProfileFields_ProfileCategories]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_name2]  DEFAULT ('') FOR [pq_altname]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileFields_pf_type]  DEFAULT ((0)) FOR [pq_edit_style]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_show_style]  DEFAULT ((0)) FOR [pq_show_style]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_search_style]  DEFAULT ((0)) FOR [pq_search_style]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileFields_pf_required]  DEFAULT ((0)) FOR [pq_required]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileFields_pf_priority]  DEFAULT ((1)) FOR [pq_priority]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_requires_approval]  DEFAULT ((0)) FOR [pq_requires_approval]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_visible_male]  DEFAULT ((1)) FOR [pq_visible_male]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_visible_female]  DEFAULT ((1)) FOR [pq_visible_female]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_visible_couple]  DEFAULT ((1)) FOR [pq_visible_couple]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_paidonly]  DEFAULT ((0)) FOR [pq_viewpaidonly]
GO
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_editpaidonly]  DEFAULT ((0)) FOR [pq_editpaidonly]
GO
