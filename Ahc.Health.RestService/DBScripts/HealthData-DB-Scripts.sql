
USE [HealthData]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT TOP 1 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'HealthDataAccess')
Begin
	CREATE TABLE [dbo].[HealthDataAccess](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](100) NOT NULL,
		[Secret] [nvarchar](255) NOT NULL,
	 CONSTRAINT [PK_HealthDataAccess] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UQ_HealthDataAccess_Description] UNIQUE NONCLUSTERED 
	(
		[Description] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO


IF NOT EXISTS (SELECT TOP 1 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'HealthDataAccess')
Begin
	CREATE TABLE [dbo].[HealthDataAccess](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[Description] [nvarchar](100) NOT NULL,
		[Secret] [nvarchar](255) NOT NULL,
	 CONSTRAINT [PK_HealthDataAccess] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 CONSTRAINT [UQ_HealthDataAccess_Description] UNIQUE NONCLUSTERED 
	(
		[Description] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END
GO


/****** Object:  Table [dbo].[Users]    Script Date: 8/1/2018 3:37:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT TOP 1 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'User')
Begin
CREATE TABLE [dbo].[User](
	[Id] Int identity(1,1) NOT NULL,
	[Title] nvarchar(20) NULL,
	[Name] nvarchar(50) NULL,
	[Gender] varchar(15) NULL,
	[Age] INT null,
	[HealthId] nvarchar(500) null,
	[TokenKey] nvarchar(100) null,
	[ContactNumber] nvarchar(50) NULL,
	[Email] nvarchar(254) NULL,
	[Active] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO
--Drop table [User]
--Drop table HealthDataCollected
IF NOT EXISTS (SELECT TOP 1 1 FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'HealthDataCollected')
Begin
CREATE TABLE [dbo].[HealthDataCollected](
	[Id] INT identity(1,1) NOT NULL FOREIGN KEY (UserId) REFERENCES [User](Id),
	[UserId] INT NULL,
	[Parameter] [nvarchar](200) NULL,
	[UOM] [nvarchar](100) NULL,
	[Reading] [nvarchar](MAX) NULL,
	[Predictive[status]] nvarchar(200) null,
	[ReadingTime] datetime NULL,
	[IsMonitored] bit NULL,
	[ActionToBeTaken] [nvarchar](50) NULL,
	[IsActionTaken] bit NULL,
	[ActionTaken] [nvarchar](50) NULL

 CONSTRAINT [PK_HealthDataCollected] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

END

GO


IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'uspGetRegisteredUssers')
DROP PROCEDURE uspGetRegisteredUssers
GO
CREATE PROCEDURE uspGetRegisteredUssers
	
AS   
   SET NOCOUNT ON; 
    
   SELECT 
   [Id]
           ,[Email]
           ,[Title]
           ,[Name]
           ,[Gender]
		   ,[Age]
           ,[HealthId]
           ,[TokenKey]
           ,[ContactNumber]
           ,[Active]
           ,[CreatedDate]
		   FROM
		   [User] WHERE Active = 1
GO  

IF EXISTS (SELECT 1 FROM sys.objects WHERE type = 'P' AND name = 'uspAddHealthData')
DROP PROCEDURE uspAddHealthData
GO
CREATE PROCEDURE uspAddHealthData
	@Id INT,
	@UserId INT,
	@Parameter nvarchar(200),
	@UOM nvarchar(100),
	@Reading nvarchar(200),
	@ReadingTime datetime = null
AS   
    SET NOCOUNT ON;  
    IF not exists(SELECT 1 from HealthDataCollected WHERE ID = @Id)
	Begin
		INSERT INTO HealthDataCollected(UserId,	Parameter, UOM, Reading, ReadingTime)
		Values(@UserId, @Parameter, @UOM,@Reading, @ReadingTime)
	 ENd
GO  






SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT TOP 1 * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'HealthRules')
Begin
	CREATE TABLE [dbo].[HealthRules](
	[SlNo] [tinyint] IDENTITY(1,1) NOT NULL,
	[Parameter] [nvarchar](100) NOT NULL,
	[Age_Lower] [tinyint] NOT NULL,
	[Age_Upper] [tinyint] NOT NULL,
	[Range_Lower] [tinyint] NOT NULL,
	[Range_Upper] [tinyint] NOT NULL,
	[Occurence] [tinyint] NULL,
	[Time_Window] [tinyint] NULL,
	[Action] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_HealthRules] PRIMARY KEY CLUSTERED 
(
	[SlNo] ASC,
	[Parameter] ASC,
	[Age_Lower] ASC,
	[Age_Upper] ASC,
	[Range_Lower] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

End
GO



Select *from HealthDataCollected

Select *from [dbo].[HealthRules]
