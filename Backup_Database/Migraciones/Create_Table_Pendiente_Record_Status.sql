/****** Object:  Table [dbo].[Ticket_Record_Status]    
        Script Date: 12/06/2023 12:53:00 p. m.
		CreatedBy: Julian Mondragon Carter
******/

USE [ptstools_HelpDesk]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pendiente_Record_Status](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_Pendiente] [int] NULL,
	[id_Status] [int] NULL,
	[CreatedAt] [datetime] NULL,
	[UpdateAt] [datetime] NULL,
 CONSTRAINT [PK_Pendiente_Record_Status] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pendiente_Record_Status]  WITH CHECK ADD  CONSTRAINT [FK_Pendiente_Record_Status_Status] FOREIGN KEY([id_Status])
REFERENCES [dbo].[Status] ([Status])
GO

ALTER TABLE [dbo].[Pendiente_Record_Status] CHECK CONSTRAINT [FK_Pendiente_Record_Status_Status]
GO

ALTER TABLE [dbo].[Pendiente_Record_Status]  WITH CHECK ADD  CONSTRAINT [FK_Pendiente_Record_Status_Pendiente] FOREIGN KEY([id_Pendiente])
REFERENCES [dbo].[Pendiente] ([id])
GO

ALTER TABLE [dbo].[Pendiente_Record_Status] CHECK CONSTRAINT [FK_Pendiente_Record_Status_Pendiente]
GO


