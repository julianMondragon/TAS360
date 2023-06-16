USE [ptstools_HelpDesk]
GO

/****** Object:  Table [dbo].[Pendiente]    Script Date: 12/06/2023 02:09:03 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[Pendiente]    
        Script Date: 12/06/2023 12:53:00 p. m.
		CreatedBy: Julian Mondragon Carter
******/
USE [ptstools_HelpDesk]
GO

ALTER TABLE [dbo].[Pendiente] add 	
	[id_status] [int] NULL,
	[is_deleted] [bit]  
	
ALTER TABLE [dbo].[Pendiente]  WITH CHECK ADD  CONSTRAINT [FK_Pendiente_Status] FOREIGN KEY([id_status])
REFERENCES [dbo].[Status] ([Status])
GO

