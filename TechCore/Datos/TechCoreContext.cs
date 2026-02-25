using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TechCore.Models;

namespace TechCore.Datos;

public partial class TechCoreContext : DbContext
{
    public TechCoreContext(DbContextOptions<TechCoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AbonosVenta> AbonosVentas { get; set; }

    public virtual DbSet<Categorium> Categoria { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<ComprasDetalle> ComprasDetalles { get; set; }

    public virtual DbSet<PlanPago> PlanPagos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    public virtual DbSet<VentasDetalle> VentasDetalles { get; set; }

    public virtual DbSet<VwCuotasPorVencer> VwCuotasPorVencers { get; set; }

    public virtual DbSet<VwCuotasVencida> VwCuotasVencidas { get; set; }

    public virtual DbSet<VwEstadoCuentum> VwEstadoCuenta { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbonosVenta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__abonosVe__3213E83F5337B15E");

            entity.ToTable("abonosVentas", tb => tb.HasTrigger("TR_ActualizarSaldo"));

            entity.HasIndex(e => e.Fecha, "IDX_abonosVentas_fecha");

            entity.HasIndex(e => e.Norden, "IDX_abonosVentas_norden");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.NumeroCuota).HasColumnName("numeroCuota");

            entity.HasOne(d => d.NordenNavigation).WithMany(p => p.AbonosVenta)
                .HasForeignKey(d => d.Norden)
                .HasConstraintName("FK__abonosVen__norde__7F2BE32F");
        });

        modelBuilder.Entity<Categorium>(entity =>
        {
            entity.HasKey(e => e.CodCategoria).HasName("PK__categori__DC56567D832DEA12");

            entity.ToTable("categoria");

            entity.HasIndex(e => e.Codigo, "IDX_categoria_codigo").IsUnique();

            entity.HasIndex(e => e.Estado, "IDX_categoria_estado");

            entity.HasIndex(e => e.Nombre, "IDX_categoria_nombre");

            entity.HasIndex(e => e.Codigo, "UQ__categori__40F9A206C88944E2").IsUnique();

            entity.Property(e => e.CodCategoria).HasColumnName("codCategoria");
            entity.Property(e => e.Codigo)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Codclien).HasName("PK__clientes__62CB7D2CF525EE4E");

            entity.ToTable("clientes");

            entity.HasIndex(e => e.Email, "IDX_clientes_email");

            entity.HasIndex(e => e.Estado, "IDX_clientes_estado");

            entity.HasIndex(e => e.Nombre, "IDX_clientes_nombre");

            entity.Property(e => e.Codclien)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codclien");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Direccion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.Norden).HasName("PK__compras__34C7253AD4F2BD47");

            entity.ToTable("compras");

            entity.HasIndex(e => e.Codprov, "IDX_compras_codprov");

            entity.HasIndex(e => e.Codusu, "IDX_compras_codusu");

            entity.HasIndex(e => e.Estado, "IDX_compras_estado");

            entity.HasIndex(e => e.Fecha, "IDX_compras_fecha");

            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.Codprov)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codprov");
            entity.Property(e => e.Codusu).HasColumnName("codusu");
            entity.Property(e => e.Estado)
                .HasDefaultValue(1)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Iva)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("iva");
            entity.Property(e => e.OrdenN).HasColumnName("ordenN");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.CodprovNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.Codprov)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compras__codprov__72C60C4A");

            entity.HasOne(d => d.CodusuNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.Codusu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__compras__codusu__73BA3083");
        });

        modelBuilder.Entity<ComprasDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__comprasD__3213E83F534A4F1B");

            entity.ToTable("comprasDetalle");

            entity.HasIndex(e => e.Codprod, "IDX_comprasDetalle_codprod");

            entity.HasIndex(e => e.Norden, "IDX_comprasDetalle_norden");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Codprod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codprod");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.CodprodNavigation).WithMany(p => p.ComprasDetalles)
                .HasForeignKey(d => d.Codprod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comprasDe__codpr__778AC167");

            entity.HasOne(d => d.NordenNavigation).WithMany(p => p.ComprasDetalles)
                .HasForeignKey(d => d.Norden)
                .HasConstraintName("FK__comprasDe__norde__76969D2E");
        });

        modelBuilder.Entity<PlanPago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__planPago__3213E83FA05E11F1");

            entity.ToTable("planPagos");

            entity.HasIndex(e => e.FechaVencimiento, "IDX_planPagos_fechaVencimiento");

            entity.HasIndex(e => e.Norden, "IDX_planPagos_norden");

            entity.HasIndex(e => e.Pagada, "IDX_planPagos_pagada").HasFilter("([pagada]=(0))");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fechaVencimiento");
            entity.Property(e => e.MontoCuota)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoCuota");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.NumeroCuota).HasColumnName("numeroCuota");
            entity.Property(e => e.Pagada)
                .HasDefaultValue(false)
                .HasColumnName("pagada");

            entity.HasOne(d => d.NordenNavigation).WithMany(p => p.PlanPagos)
                .HasForeignKey(d => d.Norden)
                .HasConstraintName("FK__planPagos__norde__7B5B524B");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Codprod).HasName("PK__producto__9FC36C24D5FBA89C");

            entity.ToTable("productos");

            entity.HasIndex(e => e.Descripcion, "IDX_productos_descripcion");

            entity.HasIndex(e => e.Estado, "IDX_productos_estado");

            entity.HasIndex(e => e.CodCategoria, "IDX_productos_idcategoria");

            entity.HasIndex(e => new { e.Stock, e.StockMinimo }, "IDX_productos_stock");

            entity.Property(e => e.Codprod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codprod");
            entity.Property(e => e.CodCategoria).HasColumnName("codCategoria");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.PrecioCompra)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precioCompra");
            entity.Property(e => e.PrecioVenta)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precioVenta");
            entity.Property(e => e.Stock)
                .HasDefaultValue(0)
                .HasColumnName("stock");
            entity.Property(e => e.StockMinimo)
                .HasDefaultValue(5)
                .HasColumnName("stockMinimo");

            entity.HasOne(d => d.CodCategoriaNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CodCategoria)
                .HasConstraintName("FK_productos_categoria");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.Codprovee).HasName("PK__proveedo__78D16CC4530ADD96");

            entity.ToTable("proveedores");

            entity.HasIndex(e => e.Estado, "IDX_proveedores_estado");

            entity.HasIndex(e => e.Nombre, "IDX_proveedores_nombre");

            entity.Property(e => e.Codprovee)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codprovee");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Direccion)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado)
                .HasDefaultValue(1)
                .HasColumnName("estado");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__rol__3213E83F860D590C");

            entity.ToTable("rol");

            entity.HasIndex(e => e.Habilitado, "IDX_rol_habilitado");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Habilitado)
                .HasDefaultValue(true)
                .HasColumnName("habilitado");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3213E83FD31B69EA");

            entity.ToTable("users");

            entity.HasIndex(e => e.Code, "IDX_users_code").IsUnique();

            entity.HasIndex(e => e.Email, "IDX_users_email");

            entity.HasIndex(e => e.Idrol, "IDX_users_idrol");

            entity.HasIndex(e => e.Username, "UQ__users__F3DBC5729B3AF7EE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("code");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_date");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Idrol).HasColumnName("idrol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Pwd)
                .IsUnicode(false)
                .HasColumnName("pwd");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("username");

            entity.HasOne(d => d.IdrolNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Idrol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__idrol__4E88ABD4");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Norden).HasName("PK__ventas__34C7253AF740C580");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.Codclien, "IDX_ventas_codclien");

            entity.HasIndex(e => e.Codvend, "IDX_ventas_codvend");

            entity.HasIndex(e => e.Fecha, "IDX_ventas_fecha");

            entity.HasIndex(e => e.Nula, "IDX_ventas_nula").HasFilter("([nula]=(0))");

            entity.HasIndex(e => e.TipoPago, "IDX_ventas_tipoPago");

            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.Codclien)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codclien");
            entity.Property(e => e.Codvend).HasColumnName("codvend");
            entity.Property(e => e.Estado)
                .HasDefaultValue(true)
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.Iva)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("iva");
            entity.Property(e => e.Meses).HasColumnName("meses");
            entity.Property(e => e.Nula)
                .HasDefaultValue(false)
                .HasColumnName("nula");
            entity.Property(e => e.OrdenN).HasColumnName("ordenN");
            entity.Property(e => e.Saldo)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("saldo");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.TasaInteres)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("tasaInteres");
            entity.Property(e => e.TipoPago)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("CONTADO")
                .HasColumnName("tipoPago");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.CodclienNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.Codclien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ventas__codclien__693CA210");

            entity.HasOne(d => d.CodvendNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.Codvend)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ventas__codvend__6A30C649");
        });

        modelBuilder.Entity<VentasDetalle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ventasDe__3213E83F71F52647");

            entity.ToTable("ventasDetalle", tb => tb.HasTrigger("TR_DisminuirStock"));

            entity.HasIndex(e => e.Codprod, "IDX_ventasDetalle_codprod");

            entity.HasIndex(e => e.Norden, "IDX_ventasDetalle_norden");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Codprod)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codprod");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.Pventa)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("pventa");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.CodprodNavigation).WithMany(p => p.VentasDetalles)
                .HasForeignKey(d => d.Codprod)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ventasDet__codpr__6E01572D");

            entity.HasOne(d => d.NordenNavigation).WithMany(p => p.VentasDetalles)
                .HasForeignKey(d => d.Norden)
                .HasConstraintName("FK__ventasDet__norde__6D0D32F4");
        });

        modelBuilder.Entity<VwCuotasPorVencer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CuotasPorVencer");

            entity.Property(e => e.Cliente)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("cliente");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fechaVencimiento");
            entity.Property(e => e.MontoCuota)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoCuota");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.NumeroCuota).HasColumnName("numeroCuota");
        });

        modelBuilder.Entity<VwCuotasVencida>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CuotasVencidas");

            entity.Property(e => e.Cliente)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("cliente");
            entity.Property(e => e.DiasAtraso).HasColumnName("diasAtraso");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fechaVencimiento");
            entity.Property(e => e.MontoCuota)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("montoCuota");
            entity.Property(e => e.MoraCalculada)
                .HasColumnType("numeric(32, 4)")
                .HasColumnName("moraCalculada");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.NumeroCuota).HasColumnName("numeroCuota");
        });

        modelBuilder.Entity<VwEstadoCuentum>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_EstadoCuenta");

            entity.Property(e => e.Cliente)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("cliente");
            entity.Property(e => e.Meses).HasColumnName("meses");
            entity.Property(e => e.Norden)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("norden");
            entity.Property(e => e.Saldo)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("saldo");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("total");
            entity.Property(e => e.TotalAbonado)
                .HasColumnType("decimal(38, 2)")
                .HasColumnName("totalAbonado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
