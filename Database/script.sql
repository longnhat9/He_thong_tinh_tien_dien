USE [BTL_tien_dien]
GO
/****** Object:  Table [dbo].[hoadon]    Script Date: 07/05/2023 15:00:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[hoadon](
	[mahoadon] [nvarchar](50) NULL,
	[macongto] [nvarchar](10) NULL,
	[makh] [nvarchar](50) NULL,
	[tenkh] [nvarchar](50) NULL,
	[thang] [nvarchar](50) NULL,
	[chisocu] [nvarchar](50) NULL,
	[chisomoi] [nvarchar](50) NULL,
	[thanhtien] [nvarchar](50) NULL,
	[status_thanhtoan] [nvarchar](20) NULL,
	[email] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tablecustomer]    Script Date: 07/05/2023 15:00:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tablecustomer](
	[makh] [nvarchar](50) NOT NULL,
	[tenkh] [nvarchar](50) NOT NULL,
	[sodt] [nvarchar](10) NOT NULL,
	[email] [nvarchar](50) NOT NULL,
	[diachi] [nvarchar](200) NOT NULL,
	[macongto] [nvarchar](10) NOT NULL,
 CONSTRAINT [pk_makh] PRIMARY KEY CLUSTERED 
(
	[makh] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[sodt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tablelogin_admin]    Script Date: 07/05/2023 15:00:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tablelogin_admin](
	[username] [nvarchar](50) NULL,
	[password] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tablelogin_customer]    Script Date: 07/05/2023 15:00:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tablelogin_customer](
	[user_customer] [nvarchar](50) NOT NULL,
	[pass_customer] [nvarchar](50) NOT NULL
) ON [PRIMARY]
GO
