/*****************************************************
		Object:  Table [dbo].[Pendiente]    
        Script Date: 12/06/2023 12:53:00 p. m.
		CreatedBy: Julian Mondragon Carter
		Sumary: se agregan propiedades pendientes al objeto pendiente
*********************************************************/
USE [ptstools_HelpDesk]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [dbo].[Pendiente] add 	
	[id_status] int NULL,
	[is_deleted] bit,
	[version_where_the_Pending_was_found] VARCHAR(250) NULL,
	[version_where_the_Pending_is_fixed] VARCHAR(250) NULL,
    [is_PAF] bit,
    [is_PAS] bit;

ALTER TABLE [dbo].[Pendiente]  WITH CHECK ADD  CONSTRAINT [FK_Pendiente_Status] FOREIGN KEY([id_status])
REFERENCES [dbo].[Status] ([Status])
GO

