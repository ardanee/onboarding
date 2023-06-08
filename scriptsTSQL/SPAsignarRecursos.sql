USE [dbAppILU]
GO

/****** Object:  StoredProcedure [OB].[ObPAsignarRecursos]    Script Date: 7/06/2023 11:51:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [OB].[ObPAsignarRecursos]
--DECLARE 
@PDPI VARCHAR(25),
@PNombre VARCHAR(50),
@PPuestoId INT = 767,
@PUsuarioOpera INT = 24
AS
BEGIN

	--Declara y asigna valores que se insertarán en el encabezado de la asignación de recursos.
	DECLARE 
	@VPlantillaId INT,  
	@VPlantillaNombre VARCHAR(50)

	(
		SELECT 
		@VPlantillaId = par.id, 
		@VPlantillaNombre = par.nombre
		FROM OB.ObTPlantillaAsignacionRecurso par
		WHERE par.puestoId IS NOT NULL 
		AND par.activo = 1
		AND par.eliminado  = 0
		AND par.puestoId = @PPuestoId
	)

	BEGIN TRY
		BEGIN TRANSACTION

		--Crea encabezado de la asignación de recursos
		INSERT OB.ObTAsignacionRecurso (DPI,fechaCreacion, usuarioCreacion,activo, eliminado) VALUES (@PDPI,GETDATE(),@PUsuarioOpera,1,0 )

		--Obtiene Id del último registro insertado
		 DECLARE @VAsignacionId INT =  @@IDENTITY
		--Lista de elementos que se insertarán en el detalle de la asignación
		INSERT OB.ObTAsignacionRecursoDetalle(AsignacionRecursoId,recursoId,detalle)
		SELECT @VAsignacionId, r.id, r.nombre
		FROM OB.ObTRecurso r
		WHERE r.plantillaAsignacionRecursoId = @VPlantillaId
		AND r.activo = 1
		AND r.eliminado = 0
			
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH	
		ROLLBACK TRANSACTION 
		DECLARE @ErrorMessage NVARCHAR(4000);  
		DECLARE @ErrorSeverity INT;  
		DECLARE @ErrorState INT;  
  
		SELECT   
			@ErrorMessage = ERROR_MESSAGE(),  
			@ErrorSeverity = ERROR_SEVERITY(),  
			@ErrorState = ERROR_STATE();  
  
			RAISERROR (@ErrorMessage, -- Message text.  
				   @ErrorSeverity, -- Severity.  
				   @ErrorState -- State.  
				   );  
	END CATCH 	

	
END
GO


