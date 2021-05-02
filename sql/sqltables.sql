USE [SampleDB]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[individual](
	[id] [int] NOT NULL,
	[firstName] [varchar](100) NULL,
	[secondName] [varchar](100) NULL,
	[thirdName] [varchar](100) NULL,
	[listedOn] [date] NULL,
	[commentary] [text] NULL,
 CONSTRAINT [IX_individualTable] UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[entity](
	[id] [int] NOT NULL,
	[name] [varchar](100) NULL,
	[listedOn] [date] NULL,
	[commentary] [text] NULL,
 CONSTRAINT [IX_entityTable] UNIQUE NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[document](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[individualId] [int] NOT NULL,
	[documentType] [varchar](100) NULL,
	[documentType2] [varchar](100) NULL,
	[documentNumber] [nchar](100) NULL,
 CONSTRAINT [PK_individualDocument] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[document]  WITH CHECK ADD  CONSTRAINT [FK_individualDocument_individualTable] FOREIGN KEY([individualId])
REFERENCES [dbo].[individual] ([id])
GO

ALTER TABLE [dbo].[document] CHECK CONSTRAINT [FK_individualDocument_individualTable]
GO

CREATE TABLE [dbo].[entityAddress](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[entityId] [int] NOT NULL,
	[street] [varchar](500) NULL,
	[city] [varchar](100) NULL,
	[zipCode] [varchar](100) NULL,
	[country] [varchar](100) NULL,
 CONSTRAINT [PK_entityAddress] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[entityAddress]  WITH CHECK ADD  CONSTRAINT [FK_entityAddress_entityTable] FOREIGN KEY([entityId])
REFERENCES [dbo].[entity] ([id])
GO

ALTER TABLE [dbo].[entityAddress] CHECK CONSTRAINT [FK_entityAddress_entityTable]
GO

CREATE TABLE [dbo].[entityAlias](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[entityId] [int] NOT NULL,
	[alias] [varchar](500) NOT NULL,
 CONSTRAINT [PK_entityAlias] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[entityAlias]  WITH CHECK ADD  CONSTRAINT [FK_entityAlias_entityTable] FOREIGN KEY([entityId])
REFERENCES [dbo].[entity] ([id])
GO

ALTER TABLE [dbo].[entityAlias] CHECK CONSTRAINT [FK_entityAlias_entityTable]
GO

CREATE TABLE [dbo].[individualAlias](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[individualId] [int] NOT NULL,
	[alias] [varchar](500) NOT NULL,
 CONSTRAINT [PK_individualAlias] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[individualAlias]  WITH CHECK ADD  CONSTRAINT [FK_individualAlias_individualTable] FOREIGN KEY([individualId])
REFERENCES [dbo].[individual] ([id])
GO

ALTER TABLE [dbo].[individualAlias] CHECK CONSTRAINT [FK_individualAlias_individualTable]
GO

CREATE TABLE [dbo].[placeOfBirth](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[individualId] [int] NOT NULL,
	[city] [varchar](100) NULL,
	[country] [varchar](100) NULL,
 CONSTRAINT [PK_individualPlaceOfBirth] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[placeOfBirth]  WITH CHECK ADD  CONSTRAINT [FK_individualPlaceOfBirth_individualTable] FOREIGN KEY([individualId])
REFERENCES [dbo].[individual] ([id])
GO

ALTER TABLE [dbo].[placeOfBirth] CHECK CONSTRAINT [FK_individualPlaceOfBirth_individualTable]
GO



CREATE TABLE [dbo].[dateOfBirth](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[individualId] [int] NOT NULL,
	[dateOfBirth] [date] NOT NULL,
 CONSTRAINT [PK_individualTimeOfBirth] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[dateOfBirth]  WITH CHECK ADD  CONSTRAINT [FK_individualTimeOfBirth_individualTable] FOREIGN KEY([individualId])
REFERENCES [dbo].[individual] ([id])
GO

ALTER TABLE [dbo].[dateOfBirth] CHECK CONSTRAINT [FK_individualTimeOfBirth_individualTable]
GO

