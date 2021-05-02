CREATE TABLE [dbo].[Actors] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50)  NOT NULL,
    [CountryId] INT            NOT NULL,
    [BirthDate] DATETIME       NOT NULL,
    [Sexe]      INT            NOT NULL,
    [PhotoId]   NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Actors_ToCountries] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id])
);

CREATE TABLE [dbo].[Films] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Title]          NVARCHAR (MAX) NOT NULL,
    [Synopsis]       NVARCHAR (MAX) NOT NULL,
    [Author]         NVARCHAR (MAX) NOT NULL,
    [AudienceId]     INT            NOT NULL,
    [StyleId]        INT            NOT NULL,
    [Year]           INT            NOT NULL,
    [PosterId]       NVARCHAR (MAX) NULL,
    [RatingsAverage] FLOAT (53)     NOT NULL,
    [NbRatings]      INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Films_ToAudiences] FOREIGN KEY ([AudienceId]) REFERENCES [Audiences]([Id]),
	CONSTRAINT [FK_Films_ToStyles] FOREIGN KEY ([StyleId]) REFERENCES [Styles]([Id])
);

CREATE TABLE [dbo].[Users] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (50)  NOT NULL,
    [FullName] NVARCHAR (50)  NOT NULL,
    [Admin]    BIT            NOT NULL,
    [AvatarId] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Audiences] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (10) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Castings] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [FilmId]  INT NOT NULL,
    [ActorId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Castings_Actors] FOREIGN KEY ([ActorId]) REFERENCES [dbo].[Actors] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Castings_Films] FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Films] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Countries] (
    [Id]   INT IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Ratings] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [FilmId]  INT            NOT NULL,
    [UserId]  INT            NOT NULL,
    [Value]   INT            NOT NULL,
    [Comment] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ratings_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Ratings_Films] FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Films] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Styles] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

