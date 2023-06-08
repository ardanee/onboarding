USE dbAppILU
GO

CREATE PROCEDURE OB.ObPAnaliticaActivosFilas
AS
BEGIN
	SELECT estado.Nombre nombre, COUNT(1) cantidad	
	FROM OB.ObTAsignacionRecursoDetalle detalle 
	INNER JOIN Ob.ObTAsignacionRecurso encabezado ON detalle.AsignacionRecursoId = encabezado.id
	INNER JOIN AV.AITAviso aviso ON aviso.CodigoAviso = detalle.numeroGestionEntrega
	INNER JOIN AV.AITEstado estado ON aviso.CodigoEstado = estado.Codigo
	WHERE encabezado.activo = 1
	AND encabezado.eliminado = 0
	AND detalle.activo = 1
	AND detalle.eliminado = 0
	GROUP BY estado.Nombre
END

GO

CREATE PROCEDURE OB.ObPAnaliticaInactivosFilas
AS
BEGIN
	SELECT estado.Nombre nombre, COUNT(1) cantidad	
	FROM OB.ObTAsignacionRecursoDetalle detalle 
	INNER JOIN Ob.ObTAsignacionRecurso encabezado ON detalle.AsignacionRecursoId = encabezado.id
	INNER JOIN AV.AITAviso aviso ON aviso.CodigoAviso = detalle.numeroGestionDevolucion
	INNER JOIN AV.AITEstado estado ON aviso.CodigoEstado = estado.Codigo
	WHERE encabezado.activo = 1
	AND encabezado.eliminado = 0
	AND detalle.activo = 0
	AND detalle.eliminado = 1
	GROUP BY estado.Nombre
END


