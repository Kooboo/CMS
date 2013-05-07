/****** Object:  Table [dbo].[Connect_User]    Script Date: 05/17/2011 13:59:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Connect_User](
	[Name] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[FirstName] [nvarchar](20) NULL,
	[MiddleName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NULL,
	[Gender] [smallint] NOT NULL,
	[Birthday] [datetime] NULL,
	[Country] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Address] [nvarchar](150) NULL,
	[Postcode] [nvarchar](30) NULL,
	[Telphone] [varchar](50) NULL,
	[Mobile] [varchar](50) NULL,
	[CreateDate] [datetime] NOT NULL,
	[FailedPasswordAnswerAttemptCount] [int] NOT NULL,
	[FailedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[FailedPasswordAttemptCount] [int] NOT NULL,
	[FailedPasswordAttemptWindowStart] [datetime] NULL,
	[IsApproved] [bit] NOT NULL,
	[IsLockedOut] [bit] NOT NULL,
	[LastLockoutDate] [datetime] NULL,
	[LastLoginDate] [datetime] NULL,
	[LastPasswordChangedDate] [datetime] NULL,
	[Password] [nvarchar](128) NULL,
	[PasswordAnswer] [nvarchar](128) NULL,
	[PasswordQuestion] [nvarchar](256) NULL,
	[PasswordSalt] [nvarchar](128) NULL,
	[CustomerId] [char](36) NULL,
	[Comment] [nvarchar](2000) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_Gender]  DEFAULT ((0)) FOR [Gender]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_FailedPasswordAnswerAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAnswerAttemptCount]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_FailedPasswordAnswerAttemptWindowStart]  DEFAULT (getdate()) FOR [FailedPasswordAnswerAttemptWindowStart]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_FailedPasswordAttemptCount]  DEFAULT ((0)) FOR [FailedPasswordAttemptCount]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_FailedPasswordAttemptWindowStart]  DEFAULT (getdate()) FOR [FailedPasswordAttemptWindowStart]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_IsApproved]  DEFAULT ((1)) FOR [IsApproved]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_IsLockedOut]  DEFAULT ((0)) FOR [IsLockedOut]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_LastLockoutDate]  DEFAULT (getdate()) FOR [LastLockoutDate]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_LastLoginDate]  DEFAULT (getdate()) FOR [LastLoginDate]
GO

ALTER TABLE [dbo].[Connect_User] ADD  CONSTRAINT [DF_User_LastPasswordChangedDate]  DEFAULT (getdate()) FOR [LastPasswordChangedDate]
GO


