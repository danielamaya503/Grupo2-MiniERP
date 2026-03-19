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

IF NOT EXISTS (SELECT 1 FROM rol WHERE nombreRol = 'Administrador')
    INSERT INTO rol (nombreRol) VALUES ('Administrador');
go
IF NOT EXISTS (SELECT 1 FROM rol WHERE nombreRol = 'Vendedor')
    INSERT INTO rol (nombreRol) VALUES ('Vendedor');
go
IF NOT EXISTS (SELECT 1 FROM rol WHERE nombreRol = 'Bodega')
    INSERT INTO rol (nombreRol) VALUES ('Bodega');
go
IF NOT EXISTS (SELECT 1 FROM rol WHERE nombreRol = 'Contador')
    INSERT INTO rol (nombreRol) VALUES ('Contador');
go

select * from rol
go

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

INSERT INTO users (code, nombre, username, pwd, phone, idrol, email)
VALUES (
    'USR-001',
    'Administrador TechCore',
    'admin',
    '$2a$11$S14bynqUvQmnTymkQHe9eOzbDJTWhYetiDbrScRYVPz8OSwSoIWvG',
    '0000-0000',
    (SELECT id FROM rol WHERE nombreRol = 'Administrador'),
    'admin@techcore.com'
);
go

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

-- TRIGGER: DISMINUIR STOCK AL VENDER
CREATE OR ALTER TRIGGER TR_DisminuirStock
ON ventasDetalle
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE p
        SET p.stock = p.stock - i.cantidad
        FROM productos p
        INNER JOIN inserted  i ON p.codprod  = i.codprod
        INNER JOIN ventas    v ON v.norden   = i.norden
        WHERE v.nula = 0;  -- Solo si la venta no está anulada

    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSev   INT            = ERROR_SEVERITY();
        DECLARE @ErrorState INT            = ERROR_STATE();
        RAISERROR(@ErrorMsg, @ErrorSev, @ErrorState);
        ROLLBACK TRANSACTION;
    END CATCH
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

-- TRIGGER: AUMENTAR STOCK AL REGISTRAR UNA COMPRA
CREATE OR ALTER TRIGGER TR_AumentarStock_Compra
ON comprasDetalle
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE p
        SET p.stock = p.stock + i.cantidad
        FROM productos p
        INNER JOIN inserted i ON p.codprod = i.codprod;

    END TRY
    BEGIN CATCH
        DECLARE @ErrorMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSev   INT            = ERROR_SEVERITY();
        DECLARE @ErrorState INT            = ERROR_STATE();
        RAISERROR(@ErrorMsg, @ErrorSev, @ErrorState);
        ROLLBACK TRANSACTION;
    END CATCH
END
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




-------------------------------------------------------
-----------------INSERTAR REGISTROS--------------------
-------------------------------------------------------

-- CLIENTES
-----------------------------------------------------
INSERT INTO clientes (codclien, nombre, telefono, email, direccion)
VALUES 
    ('CLI-001', 'Juan Carlos Martínez',   '7890-1234', 'jcmartinez@gmail.com',    'Col. Escalón, San Salvador'),
    ('CLI-002', 'María López de García',  '7654-3210', 'mlopez@hotmail.com',      'Res. Santa Elena, Antiguo Cuscatlán'),
    ('CLI-003', 'Distribuidora El Sol',   '2222-5555', 'elsol@distribuidora.com', 'Blvd. Los Héroes, San Salvador'),
    ('CLI-004', 'Roberto Flores',         '7111-2233', 'rflores@outlook.com',     'Col. Médica, San Salvador'),
    ('CLI-005', 'TechSolutions SV S.A.',  '2289-4400', 'info@techsolutions.sv',   'Zona Rosa, San Salvador');
GO



-- CATEGORIA
-----------------------------------------------------
INSERT INTO categoria (codigo, nombre, descripcion)
VALUES 
    ('CAT-001', 'Laptops',        'Computadoras portátiles de diferentes marcas y gamas'),
    ('CAT-002', 'Accesorios',    'Teclados, ratones, auriculares y accesorios de entrada'),
    ('CAT-003', 'Monitores',      'Pantallas LED, IPS y gaming de diversas pulgadas'),
    ('CAT-004', 'Almacenamiento', 'Discos duros HDD, SSD internos y externos'),
    ('CAT-005', 'Redes',          'Routers, switches, cables y equipos de conectividad');
GO

-- PROVEEDORES
-----------------------------------------------------
INSERT INTO proveedores (codprovee, nombre, telefono, email, direccion)
VALUES 
    ('PROV-001', 'Ingram Micro SV',    '2222-1100', 'ventas@gmail.com',     'Atendido desde Miami / Canal digital'),
    ('PROV-002', 'Tecno Avance S.A.',  '2222-7700', 'ventas@gmail.com',     'San Salvador, El Salvador'),
    ('PROV-003', 'Solution Box SV',    '2222-6600', 'info@gmail.com.sv',    'C. Chaparrastique N°27, Zona Ind. Santa Elena, La Libertad'),
    ('PROV-004', 'Intelmax SV',        '2222-3344', 'ventas@gmail.com.sv',     'Metrocentro San Salvador'),
    ('PROV-005', 'NESET S.A. de C.V.', '2222-9900', 'info@gmail.com',          'San Salvador, El Salvador');
GO


-- PRODUCTOS  (depende de categoria)
-----------------------------------------------------
INSERT INTO productos (codprod, codCategoria, descripcion, precioCompra, precioVenta, stock, stockMinimo)
VALUES 
    ('PROD-001', 1, 'Laptop HP 15.6" Core i5 12va Gen 8GB RAM 512GB SSD',   450.00,  699.99, 20, 5),
    ('PROD-002', 1, 'Laptop Lenovo IdeaPad 3 Ryzen 5 16GB RAM 1TB SSD',     520.00,  799.99, 15, 5),
    ('PROD-003', 2, 'Teclado Mecánico Logitech G413 TKL SE USB',              45.00,   75.00, 40, 8),
    ('PROD-004', 3, 'Monitor LG 24" Full HD IPS 75Hz HDMI/VGA',             150.00,  249.99, 18, 5),
    ('PROD-005', 4, 'SSD Kingston A400 1TB SATA 2.5"',                        60.00,   99.99, 35, 10);
GO


-- VENTAS
-----------------------------------------------------
INSERT INTO ventas (norden, ordenN, codclien, codvend, subtotal, iva, total, tipoPago, meses, tasaInteres, saldo)
VALUES 
    ('VEN-0001', 1, 'CLI-001', 4,  699.99,  90.99,  790.98, 'CONTADO', NULL, 0.00,   0.00),
    ('VEN-0002', 2, 'CLI-002', 4, 1499.98, 194.99, 1694.97, 'CREDITO',    6, 2.50, 1694.97),
    ('VEN-0003', 3, 'CLI-003', 2,  249.99,  32.49,  282.48, 'CONTADO', NULL, 0.00,   0.00),
    ('VEN-0004', 4, 'CLI-004', 4,  875.00, 113.75,  988.75, 'CREDITO',    3, 1.50,  988.75),
    ('VEN-0005', 5, 'CLI-005', 2,  199.98,  25.99,  225.97, 'CONTADO', NULL, 0.00,   0.00);
GO


-- 7. VENTAS DETALLE  (dispara TR_DisminuirStock)
-----------------------------------------------------
INSERT INTO ventasDetalle (norden, codprod, cantidad, pventa, subtotal)
VALUES 
    ('VEN-0001', 'PROD-001', 1,  699.99,  699.99),
    ('VEN-0002', 'PROD-002', 1,  799.99,  799.99),
    ('VEN-0002', 'PROD-003', 1,   75.00,   75.00),
    ('VEN-0003', 'PROD-004', 1,  249.99,  249.99),
    ('VEN-0004', 'PROD-001', 1,  699.99,  699.99),
    ('VEN-0005', 'PROD-003', 1,   75.00,   75.00),
    ('VEN-0005', 'PROD-005', 1,   99.99,   99.99);
GO


--COMPRAS (depende de proveedores y users)
-----------------------------------------------------
INSERT INTO compras (norden, ordenN, codprov, codusu, subtotal, iva, total)
VALUES 
    ('COM-0001', 1, 'PROV-001', 3,  4500.00,  585.00,  5085.00),
    ('COM-0002', 2, 'PROV-002', 3,  2250.00,  292.50,  2542.50),
    ('COM-0003', 3, 'PROV-003', 1,   900.00,  117.00,  1017.00),
    ('COM-0004', 4, 'PROV-001', 3,  3120.00,  405.60,  3525.60),
    ('COM-0005', 5, 'PROV-004', 3,  1800.00,  234.00,  2034.00);
GO


--  COMPRAS DETALLE
-----------------------------------------------------
INSERT INTO comprasDetalle (norden, codprod, cantidad, precio, subtotal)
VALUES 
    ('COM-0001', 'PROD-001', 10,  450.00, 4500.00),
    ('COM-0002', 'PROD-002',  5,  520.00, 2600.00),
    ('COM-0003', 'PROD-003', 20,   45.00,  900.00),
    ('COM-0004', 'PROD-004', 10,  150.00, 1500.00),
    ('COM-0004', 'PROD-001',  4,  450.00, 1800.00),
    ('COM-0005', 'PROD-005', 30,   60.00, 1800.00);
GO

-----------------------------------------------------
-- PLAN DE PAGOS (solo ventas en CREDITO: VEN-0002 y VEN-0004)
-- VEN-0002: 6 cuotas de ~282.49
-- VEN-0004: 3 cuotas de ~329.58
-----------------------------------------------------
INSERT INTO planPagos (norden, numeroCuota, fechaVencimiento, montoCuota)
VALUES
    -- VEN-0002 (6 cuotas)
    ('VEN-0002', 1, '2026-04-12', 282.49),
    ('VEN-0002', 2, '2026-05-12', 282.49),
    ('VEN-0002', 3, '2026-06-12', 282.49),
    ('VEN-0002', 4, '2026-07-12', 282.50),
    ('VEN-0002', 5, '2026-08-12', 282.50),
    ('VEN-0002', 6, '2026-09-12', 282.49),
    -- VEN-0004 (3 cuotas)
    ('VEN-0004', 1, '2026-04-17', 329.58),
    ('VEN-0004', 2, '2026-05-17', 329.58),
    ('VEN-0004', 3, '2026-06-17', 329.59);
GO


-- ABONOS (1 abono por venta crédito para probar TR_ActualizarSaldo)
-----------------------------------------------------
INSERT INTO abonosVentas (norden, monto, numeroCuota)
VALUES 
    ('VEN-0002', 282.49, 1),
    ('VEN-0002', 282.49, 2),
    ('VEN-0004', 329.58, 1),
    ('VEN-0002', 282.49, 3),
    ('VEN-0004', 329.58, 2);
GO


