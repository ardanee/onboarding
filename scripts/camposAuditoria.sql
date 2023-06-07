ALTER TABLE OB.ObTRecurso
ADD activo BIT NOT NULL DEFAULT 1,
eliminado BIT NOT NULL DEFAULT 0,
usuarioCreacion INT NOT NULL,
fechaCreacion DATETIME NOT NULL,
usuarioModificacion INT NULL,
fechaModificacion DATETIME NULL
