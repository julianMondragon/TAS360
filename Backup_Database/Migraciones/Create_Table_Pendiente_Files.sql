USE [ptstools_HelpDesk]
GO

/*****************************************************
		Object:  Table [dbo].[Pendiente_Files]
        Script Date: 18/06/2023 10:07:00 p. m.
		CreatedBy: Julian Mondragon Carter
		Sumary: se agrega la tabla que almacenara la 
		relacion entre los archivos y el pendiente.

*********************************************************/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pendiente_Files](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_Pendiente] [int] NULL,
	[id_File] [int] NULL,
 CONSTRAINT [PK_Pendiente_Files] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Pendiente_Files]  WITH CHECK ADD  CONSTRAINT [FK_Pendiente_Files_Files] FOREIGN KEY([id_File])
REFERENCES [dbo].[Files] ([id])
GO

ALTER TABLE [dbo].[Pendiente_Files] CHECK CONSTRAINT [FK_Pendiente_Files_Files]
GO

ALTER TABLE [dbo].[Pendiente_Files]  WITH CHECK ADD  CONSTRAINT [FK_Pendiente_Files_Pendiente] FOREIGN KEY([id_Pendiente])
REFERENCES [dbo].[Pendiente] ([id])
GO

ALTER TABLE [dbo].[Pendiente_Files] CHECK CONSTRAINT [FK_Pendiente_Files_Pendiente]
GO


