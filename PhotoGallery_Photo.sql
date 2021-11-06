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


/*Insert some initial data*/
SET IDENTITY_INSERT [PhotoGallery].[Photo] ON 
GO

INSERT [PhotoGallery].[Photo] ([PhotoId], [ImageLink], [Description], [Timestamp]) 
VALUES (4, N'https://static.euronews.com/articles/stories/05/85/91/80/1440x810_cmsv2_db0d87d0-0896-5d43-b056-468d484edcc4-5859180.jpg',
        N'Old town in Tallinn', CAST(N'2021-10-14T11:29:46.000' AS DateTime))
GO

INSERT [PhotoGallery].[Photo] ([PhotoId], [ImageLink], [Description], [Timestamp]) 
VALUES (3, N'https://www.usnews.com/dims4/USNEWS/298201f/2147483647/thumbnail/640x420/quality/85/?url=http%3A%2F%2Fmedia.beam.usnews.com%2F76%2F01%2Fbe89becd4c94a7f803eef2aac78e%2F180205-editorial.bc.estonia.eresidency_main.jpg', 
		N'Tallinn''s New Town', CAST(N'2021-10-13T11:29:46.000' AS DateTime))
GO

INSERT [PhotoGallery].[Photo] ([PhotoId], [ImageLink], [Description], [Timestamp]) 
VALUES (2, N'https://bnn-news.com/wp-content/uploads/2019/08/1KT26AUG19I3.jpg', 
        N'#Tartu2024', CAST(N'2021-10-12T11:29:46.000' AS DateTime))
GO

INSERT [PhotoGallery].[Photo] ([PhotoId], [ImageLink], [Description], [Timestamp]) 
VALUES (1, N'https://media-cdn.tripadvisor.com/media/photo-s/07/25/39/7d/estonian-experience.jpg', 
        N'Winter Time!', CAST(N'2021-10-11T11:29:46.000' AS DateTime))
GO

SET IDENTITY_INSERT [PhotoGallery].[Photo] OFF
GO