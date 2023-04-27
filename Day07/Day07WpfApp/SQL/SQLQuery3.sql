CREATE TABLE [dbo].[FavoriteMovieItem](
	[Id] [int] NOT NULL PRIMARY KEY,
	[Title] [nvarchar](300) NOT NULL,
	[Original_Title] [nvarchar](300) NOT NULL,
	[Release_Date] [char](10) NOT NULL,
	[Original_Language] [varchar](10) NOT NULL,
	[Adult] [bit] NULL,
	[Popularity] [float] NOT NULL,
	[Vote_Average] [float] NOT NULL,
	[Poster_Path] [varchar](300) NULL,
	[Overview] [ntext] NULL,
	[Reg_Date] [datetime] NOT NULL,
 CONSTRAINT [PK_FavoriteMovieItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


