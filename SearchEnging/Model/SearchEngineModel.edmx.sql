
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/23/2016 19:33:58
-- Generated from EDMX file: C:\Users\New Tech\documents\visual studio 2010\Projects\SearchEnging\SearchEnging\Model\SearchEngineModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [search];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_WordDocumentWordRelationship]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentWordRelationshipSet] DROP CONSTRAINT [FK_WordDocumentWordRelationship];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentDocumentWordRelationship]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentWordRelationshipSet] DROP CONSTRAINT [FK_DocumentDocumentWordRelationship];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[DocumentSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentSet];
GO
IF OBJECT_ID(N'[dbo].[WordSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WordSet];
GO
IF OBJECT_ID(N'[dbo].[DocumentWordRelationshipSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentWordRelationshipSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DocumentSet'
CREATE TABLE [dbo].[DocumentSet] (
    [DocumentId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Uri] nvarchar(max)  NOT NULL,
    [DateIndexed] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'WordSet'
CREATE TABLE [dbo].[WordSet] (
    [Value] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'DocumentWordRelationshipSet'
CREATE TABLE [dbo].[DocumentWordRelationshipSet] (
    [WordValue] nvarchar(200)  NOT NULL,
    [DocumentId] int  NOT NULL,
    [Frequency] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [DocumentId] in table 'DocumentSet'
ALTER TABLE [dbo].[DocumentSet]
ADD CONSTRAINT [PK_DocumentSet]
    PRIMARY KEY CLUSTERED ([DocumentId] ASC);
GO

-- Creating primary key on [Value] in table 'WordSet'
ALTER TABLE [dbo].[WordSet]
ADD CONSTRAINT [PK_WordSet]
    PRIMARY KEY CLUSTERED ([Value] ASC);
GO

-- Creating primary key on [WordValue], [DocumentId] in table 'DocumentWordRelationshipSet'
ALTER TABLE [dbo].[DocumentWordRelationshipSet]
ADD CONSTRAINT [PK_DocumentWordRelationshipSet]
    PRIMARY KEY CLUSTERED ([WordValue], [DocumentId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [WordValue] in table 'DocumentWordRelationshipSet'
ALTER TABLE [dbo].[DocumentWordRelationshipSet]
ADD CONSTRAINT [FK_WordDocumentWordRelationship]
    FOREIGN KEY ([WordValue])
    REFERENCES [dbo].[WordSet]
        ([Value])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [DocumentId] in table 'DocumentWordRelationshipSet'
ALTER TABLE [dbo].[DocumentWordRelationshipSet]
ADD CONSTRAINT [FK_DocumentDocumentWordRelationship]
    FOREIGN KEY ([DocumentId])
    REFERENCES [dbo].[DocumentSet]
        ([DocumentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentDocumentWordRelationship'
CREATE INDEX [IX_FK_DocumentDocumentWordRelationship]
ON [dbo].[DocumentWordRelationshipSet]
    ([DocumentId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------