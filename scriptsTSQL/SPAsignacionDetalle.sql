use dbAppILU
GO

ALTER PROCEDURE OB.ObPAsignacionDetalle
@PAsignacionId INT 
,@PActivo BIT 
AS
SELECT ard.id, ard.recursoId, ard.detalle, ard.numeroGestionEntrega, eEntrega.Nombre estadoGestionEntrega, ard.numeroGestionDevolucion, eDevolucion.Nombre estadoGestionDevolucion
FROM OB.ObTAsignacionRecursoDetalle ard
LEFT JOIN AV.AITAviso aEntrega ON ard.numeroGestionEntrega = aEntrega.CodigoAviso
LEFT JOIN AV.AITEstado eEntrega ON aEntrega.CodigoEstado = eEntrega.Codigo
LEFT JOIN AV.AITAviso aDevolucion ON ard.numeroGestionDevolucion = aDevolucion.CodigoAviso
LEFT JOIN AV.AITEstado eDevolucion ON aDevolucion.CodigoEstado = eDevolucion.Codigo
WHERE ard.activo = @PActivo
AND NOT ard.eliminado =  @PActivo
AND ard.AsignacionRecursoId = @PAsignacionId

