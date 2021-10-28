USE [MyDatabase]
GO

CREATE SCHEMA PhotoGallery;
GO

CREATE TABLE [PhotoGallery].[Photo](
    [PhotoId] [int] IDENTITY(1,1) NOT NULL,
    /*nvarchar when the sizes of the column data entries vary considerably*/
	[ImageLink] [nvarchar](300) NOT NULL,
    [Description] [nvarchar](20) NULL,
    [Timestamp] [datetime] NOT NULL,

	CONSTRAINT [PK_PhotoGallery] PRIMARY KEY CLUSTERED (
		[PhotoId] DESC
	)

) ON [PRIMARY]

GO