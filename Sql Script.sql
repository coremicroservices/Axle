USE [axle]
GO
/****** Object:  Table [dbo].[buyersuppliermapping]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[buyersuppliermapping](
	[id] [varchar](32) NOT NULL,
	[shipmentId] [varchar](32) NULL,
	[supplierId] [varchar](32) NULL,
	[createdOn] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FcmDeviceTokens]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FcmDeviceTokens](
	[Id] [varchar](32) NOT NULL,
	[UserId] [varchar](32) NOT NULL,
	[DeviceToken] [nvarchar](max) NOT NULL,
	[Platform] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Files]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Files](
	[Id] [varchar](32) NOT NULL,
	[Type] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Size] [bigint] NOT NULL,
	[Fingerprint] [nvarchar](255) NULL,
	[ParentId] [nvarchar](50) NOT NULL,
	[CreationDate] [datetime] NULL,
	[Owner] [nvarchar](50) NULL,
	[folderPath] [varchar](250) NULL,
	[nodeId] [varchar](250) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[shipmentdetail]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[shipmentdetail](
	[Id] [varchar](32) NOT NULL,
	[ShipmentId] [varchar](32) NULL,
	[ProductCode] [nvarchar](100) NOT NULL,
	[NumberOfBoxes] [int] NOT NULL,
	[Length] [decimal](18, 2) NULL,
	[Breadth] [decimal](18, 2) NULL,
	[Height] [decimal](18, 2) NULL,
	[TotalWeight] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShipmentFiles]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipmentFiles](
	[Id] [varchar](32) NOT NULL,
	[OriginalFileName] [nvarchar](255) NOT NULL,
	[StoredFileName] [nvarchar](255) NOT NULL,
	[FileExtension] [nvarchar](20) NULL,
	[FileSizeKB] [decimal](18, 2) NULL,
	[ContentType] [nvarchar](100) NULL,
	[FilePath] [nvarchar](500) NOT NULL,
	[UploadedBy] [nvarchar](100) NULL,
	[UploadedOn] [datetime2](7) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[fileItemId] [varchar](32) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shipments]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shipments](
	[Id] [varchar](32) NOT NULL,
	[SourcePincode] [nvarchar](6) NOT NULL,
	[SourceAddress] [nvarchar](1000) NOT NULL,
	[DestinationPincode] [nvarchar](6) NOT NULL,
	[DestinationAddress] [nvarchar](1000) NOT NULL,
	[InvoiceNumber] [nvarchar](100) NULL,
	[InvoiceValue] [decimal](18, 2) NULL,
	[CreatedBy] [nvarchar](100) NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[UpdatedOn] [datetime2](7) NULL,
	[IsActive] [bit] NOT NULL,
	[ShipmentFileId] [varchar](32) NOT NULL,
	[BookingDate] [datetime] NULL,
	[BookingId] [varchar](50) NULL,
 CONSTRAINT [PK__Shipment__3214EC07BCFA226B] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[SupplierId] [varchar](32) NOT NULL,
	[CompanyName] [nvarchar](100) NOT NULL,
	[OwnerName] [nvarchar](100) NOT NULL,
	[ContactNumber] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[TruckCount] [int] NOT NULL,
	[TruckTypes] [nvarchar](200) NULL,
	[BaseLocation] [nvarchar](100) NULL,
	[ServiceRegions] [nvarchar](200) NULL,
	[TruckImagePath] [nvarchar](200) NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/16/2025 12:20:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [varchar](32) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PasswordHash] [nvarchar](512) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[FcmDeviceTokenId] [varchar](5000) NULL,
	[contactnumber] [varchar](11) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[buyersuppliermapping] ADD  DEFAULT (getdate()) FOR [createdOn]
GO
ALTER TABLE [dbo].[FcmDeviceTokens] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Files] ADD  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[ShipmentFiles] ADD  DEFAULT (sysutcdatetime()) FOR [UploadedOn]
GO
ALTER TABLE [dbo].[ShipmentFiles] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Shipments] ADD  CONSTRAINT [DF__Shipments__Creat__412EB0B6]  DEFAULT (sysutcdatetime()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Shipments] ADD  CONSTRAINT [DF__Shipments__IsAct__4222D4EF]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Suppliers] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
