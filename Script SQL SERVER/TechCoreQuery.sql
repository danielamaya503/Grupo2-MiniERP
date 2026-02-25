CREATE DATABASE TechCore
GO

USE TechCore
GO

-----------------------------------------------------
-- TABLA ROL
-----------------------------------------------------
CREATE TABLE rol(
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombreRol VARCHAR(100) NOT NULL,
    habilitado BIT DEFAULT 1
)
GO

-----------------------------------------------------
-- TABLA USERS
-----------------------------------------------------
CREATE TABLE users(
    id INT IDENTITY(1,1) PRIMARY KEY,
    code VARCHAR(10) NOT NULL,
    nombre VARCHAR(200) NOT NULL,
    username VARCHAR(100) NOT NULL UNIQUE,
    pwd VARCHAR(MAX) NOT NULL,
    phone VARCHAR(15),
    idrol INT NOT NULL,
    email VARCHAR(200),
    created_date DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (idrol) REFERENCES rol(id)
)
GO

-----------------------------------------------------
-- CLIENTES
-----------------------------------------------------
CREATE TABLE clientes(
    codclien VARCHAR(50) PRIMARY KEY,
    nombre VARCHAR(200) NOT NULL,
    telefono VARCHAR(15),
    email VARCHAR(200),
    direccion VARCHAR(300),
    estado BIT DEFAULT 1,
    created_date DATETIME DEFAULT GETDATE()
)
GO

-----------------------------------------------------
-- TABLA CATEGORIA
-----------------------------------------------------
CREATE TABLE categoria(
    codCategoria INT IDENTITY(1,1) PRIMARY KEY,
    codigo VARCHAR(20) NOT NULL UNIQUE,
    nombre VARCHAR(150) NOT NULL,
    descripcion VARCHAR(300),
    estado BIT DEFAULT 1,
    created_date DATETIME DEFAULT GETDATE()
)
GO

-----------------------------------------------------
-- TABLA PROVEEDORES
-----------------------------------------------------
CREATE TABLE proveedores
(
    codprovee VARCHAR(50) PRIMARY KEY,
    nombre VARCHAR(200) NOT NULL,
    telefono VARCHAR(15),
    email VARCHAR(200),
    direccion VARCHAR(300),
    estado INT DEFAULT 1,
    created_date DATETIME DEFAULT GETDATE()
)
GO


-----------------------------------------------------
-- PRODUCTOS
-----------------------------------------------------
CREATE TABLE productos(
    codprod VARCHAR(50) PRIMARY KEY,
    codCategoria INT NULL,
    descripcion VARCHAR(500),
    precioCompra DECIMAL(18,2) NOT NULL,
    precioVenta DECIMAL(18,2) NOT NULL,
    stock INT DEFAULT 0,
    stockMinimo INT DEFAULT 5,
    estado BIT DEFAULT 1,
    created_date DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_productos_categoria
    FOREIGN KEY (codCategoria) REFERENCES categoria(codCategoria)
)
GO

-----------------------------------------------------
-- VENTAS
-----------------------------------------------------
CREATE TABLE ventas(
    norden VARCHAR(50) PRIMARY KEY,
    ordenN INT NOT NULL,
    codclien VARCHAR(50) NOT NULL,
    codvend INT NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),

    subtotal DECIMAL(18,2) NOT NULL,
    iva DECIMAL(18,2) NOT NULL,
    total DECIMAL(18,2) NOT NULL,

    tipoPago VARCHAR(20) DEFAULT 'CONTADO', -- CONTADO / CREDITO
    meses INT NULL,
    tasaInteres DECIMAL(5,2) DEFAULT 0,
    saldo DECIMAL(18,2) NOT NULL,

    nula BIT DEFAULT 0, -- 0 = válida, 1 = anulada
    estado BIT DEFAULT 1,

    FOREIGN KEY (codclien) REFERENCES clientes(codclien),
    FOREIGN KEY (codvend) REFERENCES users(id)
)
GO

-----------------------------------------------------
-- VENTAS DETALLE (CON CASCADE CORRECTO)
-----------------------------------------------------
CREATE TABLE ventasDetalle(
    id INT IDENTITY(1,1) PRIMARY KEY,
    norden VARCHAR(50) NOT NULL,
    codprod VARCHAR(50) NOT NULL,
    cantidad INT NOT NULL,
    pventa DECIMAL(18,2) NOT NULL,
    subtotal DECIMAL(18,2) NOT NULL,

    FOREIGN KEY (norden) 
        REFERENCES ventas(norden)
        ON DELETE CASCADE,   

    FOREIGN KEY (codprod) 
        REFERENCES productos(codprod)
)
GO

-----------------------------------------------------
-- TABLA COMPRAS
-----------------------------------------------------
CREATE TABLE compras
(
    norden VARCHAR(50) PRIMARY KEY,
    ordenN INT NOT NULL,
    codprov VARCHAR(50) NOT NULL,
    codusu INT NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),
    subtotal DECIMAL(18,2) NOT NULL,
    iva DECIMAL(18,2) NOT NULL,
    total DECIMAL(18,2) NOT NULL,
    estado INT DEFAULT 1,

    FOREIGN KEY (codprov) REFERENCES proveedores(codprovee),
    FOREIGN KEY (codusu) REFERENCES users(id)
)
GO




-----------------------------------------------------
-- TABLA COMPRAS DETALLE (CON CASCADE)
-----------------------------------------------------
CREATE TABLE comprasDetalle
(
    id INT IDENTITY(1,1) PRIMARY KEY,
    norden VARCHAR(50) NOT NULL,
    codprod VARCHAR(50) NOT NULL,
    cantidad INT NOT NULL,
    precio DECIMAL(18,2) NOT NULL,
    subtotal DECIMAL(18,2) NOT NULL,

    FOREIGN KEY (norden) 
        REFERENCES compras(norden)
        ON DELETE CASCADE,

    FOREIGN KEY (codprod) REFERENCES productos(codprod)
)
GO



-----------------------------------------------------
-- PLAN DE PAGOS
-----------------------------------------------------
CREATE TABLE planPagos(
    id INT IDENTITY(1,1) PRIMARY KEY,
    norden VARCHAR(50) NOT NULL,
    numeroCuota INT NOT NULL,
    fechaVencimiento DATE NOT NULL,
    montoCuota DECIMAL(18,2) NOT NULL,
    pagada BIT DEFAULT 0,
    FOREIGN KEY (norden) REFERENCES ventas(norden) ON DELETE CASCADE
)
GO

-----------------------------------------------------
-- ABONOS
-----------------------------------------------------
CREATE TABLE abonosVentas(
    id INT IDENTITY(1,1) PRIMARY KEY,
    norden VARCHAR(50) NOT NULL,
    fecha DATETIME DEFAULT GETDATE(),
    monto DECIMAL(18,2) NOT NULL,
    numeroCuota INT NOT NULL,
    FOREIGN KEY (norden) REFERENCES ventas(norden) ON DELETE CASCADE
)
GO


-----------------------------------------------------
-- INDICES: ROL
-----------------------------------------------------
CREATE INDEX IDX_rol_habilitado ON rol(habilitado)
GO

-----------------------------------------------------
-- INDICES: USERS
-----------------------------------------------------
CREATE UNIQUE INDEX IDX_users_code ON users(code)
GO
CREATE INDEX IDX_users_idrol ON users(idrol)
GO
CREATE INDEX IDX_users_email ON users(email)
GO

-----------------------------------------------------
-- INDICES: CATEGORIA
-----------------------------------------------------
CREATE UNIQUE INDEX IDX_categoria_codigo ON categoria(codigo)
GO
CREATE INDEX IDX_categoria_nombre ON categoria(nombre)
GO
CREATE INDEX IDX_categoria_estado ON categoria(estado)
GO

-----------------------------------------------------
-- INDICES: CLIENTES
-----------------------------------------------------
CREATE INDEX IDX_clientes_nombre ON clientes(nombre)
GO
CREATE INDEX IDX_clientes_estado ON clientes(estado)
GO
CREATE INDEX IDX_clientes_email ON clientes(email)
GO

-----------------------------------------------------
-- INDICES: PROVEEDORES
-----------------------------------------------------
CREATE INDEX IDX_proveedores_nombre ON proveedores(nombre)
GO
CREATE INDEX IDX_proveedores_estado ON proveedores(estado)
GO

-----------------------------------------------------
-- INDICES: PRODUCTOS
-----------------------------------------------------
CREATE INDEX IDX_productos_descripcion ON productos(descripcion)
GO
CREATE INDEX IDX_productos_idcategoria ON productos(codCategoria)
GO
CREATE INDEX IDX_productos_estado ON productos(estado)
GO
-- Útil para alertas de reabastecimiento
CREATE INDEX IDX_productos_stock ON productos(stock, stockMinimo)
GO

-----------------------------------------------------
-- INDICES: VENTAS
-----------------------------------------------------
CREATE INDEX IDX_ventas_codclien ON ventas(codclien)
GO
CREATE INDEX IDX_ventas_codvend ON ventas(codvend)
GO
CREATE INDEX IDX_ventas_fecha ON ventas(fecha)
GO
CREATE INDEX IDX_ventas_tipoPago ON ventas(tipoPago)
GO
-- Filtrado de ventas activas/anuladas (muy frecuente en las views)
CREATE INDEX IDX_ventas_nula ON ventas(nula) WHERE nula = 0
GO

-----------------------------------------------------
-- INDICES: VENTAS DETALLE
-----------------------------------------------------
CREATE INDEX IDX_ventasDetalle_norden ON ventasDetalle(norden)
GO
CREATE INDEX IDX_ventasDetalle_codprod ON ventasDetalle(codprod)
GO

-----------------------------------------------------
-- INDICES: COMPRAS
-----------------------------------------------------
CREATE INDEX IDX_compras_codprov ON compras(codprov)
GO
CREATE INDEX IDX_compras_codusu ON compras(codusu)
GO
CREATE INDEX IDX_compras_fecha ON compras(fecha)
GO
CREATE INDEX IDX_compras_estado ON compras(estado)
GO

-----------------------------------------------------
-- INDICES: COMPRAS DETALLE
-----------------------------------------------------
CREATE INDEX IDX_comprasDetalle_norden ON comprasDetalle(norden)
GO
CREATE INDEX IDX_comprasDetalle_codprod ON comprasDetalle(codprod)
GO

-----------------------------------------------------
-- INDICES: PLAN DE PAGOS
-----------------------------------------------------
CREATE INDEX IDX_planPagos_norden ON planPagos(norden)
GO
CREATE INDEX IDX_planPagos_fechaVencimiento ON planPagos(fechaVencimiento)
GO
-- Filtrado de cuotas pendientes (usado en vw_CuotasVencidas y vw_CuotasPorVencer)
CREATE INDEX IDX_planPagos_pagada ON planPagos(pagada) WHERE pagada = 0
GO

-----------------------------------------------------
-- INDICES: ABONOS
-----------------------------------------------------
CREATE INDEX IDX_abonosVentas_norden ON abonosVentas(norden)
GO
CREATE INDEX IDX_abonosVentas_fecha ON abonosVentas(fecha)
GO



-----------------------------------------------------
-- TRIGGER DISMINUIR STOCK SOLO SI NO ESTA ANULADA
-----------------------------------------------------
CREATE TRIGGER TR_DisminuirStock
ON ventasDetalle
AFTER INSERT
AS
BEGIN
    UPDATE p
    SET p.stock = p.stock - i.cantidad
    FROM productos p
    INNER JOIN inserted i ON p.codprod = i.codprod
    INNER JOIN ventas v ON v.norden = i.norden
    WHERE v.nula = 0
END
GO

-----------------------------------------------------
-- TRIGGER ACTUALIZAR SALDO Y MARCAR CUOTA PAGADA
-----------------------------------------------------
CREATE TRIGGER TR_ActualizarSaldo
ON abonosVentas
AFTER INSERT
AS
BEGIN
    UPDATE v
    SET v.saldo = v.saldo - i.monto
    FROM ventas v
    INNER JOIN inserted i ON v.norden = i.norden

    UPDATE pp
    SET pagada = 1
    FROM planPagos pp
    INNER JOIN inserted i 
        ON pp.norden = i.norden 
        AND pp.numeroCuota = i.numeroCuota
END
GO



-----------------------------------------------------
-- VIEW CUOTAS VENCIDAS (NO FACTURAS ANULADAS)
-----------------------------------------------------
CREATE VIEW vw_CuotasVencidas
AS
SELECT 
    v.norden,
    c.nombre AS cliente,
    pp.numeroCuota,
    pp.fechaVencimiento,
    pp.montoCuota,
    DATEDIFF(DAY, pp.fechaVencimiento, GETDATE()) AS diasAtraso,
    (pp.montoCuota * 0.02) * DATEDIFF(DAY, pp.fechaVencimiento, GETDATE()) AS moraCalculada
FROM planPagos pp
INNER JOIN ventas v ON pp.norden = v.norden
INNER JOIN clientes c ON v.codclien = c.codclien
WHERE pp.pagada = 0
AND pp.fechaVencimiento < GETDATE()
AND v.nula = 0
GO

-----------------------------------------------------
-- VIEW CUOTAS POR VENCER
-----------------------------------------------------
CREATE VIEW vw_CuotasPorVencer
AS
SELECT 
    v.norden,
    c.nombre AS cliente,
    pp.numeroCuota,
    pp.fechaVencimiento,
    pp.montoCuota
FROM planPagos pp
INNER JOIN ventas v ON pp.norden = v.norden
INNER JOIN clientes c ON v.codclien = c.codclien
WHERE pp.pagada = 0
AND pp.fechaVencimiento >= GETDATE()
AND v.nula = 0
GO

-----------------------------------------------------
-- VIEW ESTADO DE CUENTA 
-----------------------------------------------------
CREATE VIEW vw_EstadoCuenta
AS
SELECT 
    v.norden,
    c.nombre AS cliente,
    v.total,
    v.saldo,
    v.meses,
    SUM(ISNULL(a.monto,0)) AS totalAbonado
FROM ventas v
INNER JOIN clientes c ON v.codclien = c.codclien
LEFT JOIN abonosVentas a ON v.norden = a.norden
WHERE v.tipoPago = 'CREDITO'
AND v.nula = 0
GROUP BY v.norden, c.nombre, v.total, v.saldo, v.meses
GO