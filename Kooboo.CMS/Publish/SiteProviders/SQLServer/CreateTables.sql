/****** Object:  Table [dbo].[Pages]    Script Date: 02/22/2012 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pages]') AND TYPE IN (N'U'))
BEGIN
CREATE TABLE [dbo].[Pages](
	[SiteName] [nvarchar](128) NOT NULL,
	[FullName] [nvarchar](128) NOT NULL,
	[ParentPage] [nvarchar](128) NULL,
	[IsDefault] [bit] NOT NULL,
	[ObjectXml] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_Pages] PRIMARY KEY CLUSTERED 
(
	[SiteName] ASC,
	[FullName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[PageDrafts]    Script Date: 02/22/2012 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PageDrafts]') AND TYPE IN (N'U'))
BEGIN
CREATE TABLE [dbo].[PageDrafts](
	[SiteName] [nvarchar](128) NOT NULL,
	[FullName] [nvarchar](128) NOT NULL,
	[ParentPage] [nvarchar](128) NULL,
	[IsDefault] [bit] NOT NULL,
	[ObjectXml] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_PageDrafts] PRIMARY KEY CLUSTERED 
(
	[SiteName] ASC,
	[FullName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Labels]    Script Date: 02/22/2012 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Labels]') AND TYPE IN (N'U'))
BEGIN
CREATE TABLE [dbo].[Labels](
	[SiteName] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Category] [nvarchar](128) NOT NULL,
	[VALUE] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_Labels] PRIMARY KEY CLUSTERED 
(
	[SiteName] ASC,
	[Name] ASC,
	[Category] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LabelCategories]    Script Date: 02/22/2012 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LabelCategories]') AND TYPE IN (N'U'))
BEGIN
CREATE TABLE [dbo].[LabelCategories](
	[SiteName] [nvarchar](128) NOT NULL,
	[CategoryName] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_LabelCategories] PRIMARY KEY CLUSTERED 
(
	[SiteName] ASC,
	[CategoryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[HtmlBlocks]    Script Date: 02/22/2012 10:28:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HtmlBlocks]') AND TYPE IN (N'U'))
BEGIN
CREATE TABLE [dbo].[HtmlBlocks](
	[SiteName] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Body] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_HtmlBlocks] PRIMARY KEY CLUSTERED 
(
	[SiteName] ASC,
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
 
/****** Object:  Table [dbo].[SiteUsers]    Script Date: 05/15/2012 14:04:33 ******/
SET ANSI_NULLS ON
GO
 
SET QUOTED_IDENTIFIER ON
GO
 
CREATE TABLE [dbo].[SiteUsers](
	[SiteName] [nvarchar](128) NOT NULL,
	[UserName] [nvarchar](128) NOT NULL,	
	[ObjectXml] [nvarchar](MAX) NULL,
 CONSTRAINT [PK_SiteUsers] PRIMARY KEY CLUSTERED 
(
	[SiteName] ASC,
	[UserName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
 
GO
 