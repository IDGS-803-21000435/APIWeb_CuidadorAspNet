using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cuidador.Models;

public partial class DbAae280CuidadorContext : DbContext
{
    public DbAae280CuidadorContext()
    {
    }

    public DbAae280CuidadorContext(DbContextOptions<DbAae280CuidadorContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvisosMovil> AvisosMovils { get; set; }

    public virtual DbSet<Capacitacion> Capacitacions { get; set; }

    public virtual DbSet<CapacitacionUsuario> CapacitacionUsuarios { get; set; }

    public virtual DbSet<CertificacionesExperiencium> CertificacionesExperiencia { get; set; }

    public virtual DbSet<ComentariosUsuario> ComentariosUsuarios { get; set; }

    public virtual DbSet<Contrato> Contratos { get; set; }

    public virtual DbSet<ContratoItem> ContratoItems { get; set; }

    public virtual DbSet<CuentaBancarium> CuentaBancaria { get; set; }

    public virtual DbSet<DatosMedico> DatosMedicos { get; set; }

    public virtual DbSet<Documentacion> Documentacions { get; set; }

    public virtual DbSet<Domicilio> Domicilios { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MetodoPago> MetodoPagos { get; set; }

    public virtual DbSet<MetodoPagoUsuario> MetodoPagoUsuarios { get; set; }

    public virtual DbSet<MovimientoCuentum> MovimientoCuenta { get; set; }

    public virtual DbSet<NivelUsuario> NivelUsuarios { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Padecimiento> Padecimientos { get; set; }

    public virtual DbSet<PersonaFisica> PersonaFisicas { get; set; }

    public virtual DbSet<PersonaMoral> PersonaMorals { get; set; }

    public virtual DbSet<SalarioCuidador> SalarioCuidadors { get; set; }

    public virtual DbSet<Saldo> Saldos { get; set; }

    public virtual DbSet<SesionesUsuario> SesionesUsuarios { get; set; }

    public virtual DbSet<TareasContrato> TareasContratos { get; set; }

    public virtual DbSet<TarjetaUsuario> TarjetaUsuarios { get; set; }

    public virtual DbSet<TipoEstatus> TipoEstatuses { get; set; }

    public virtual DbSet<TipoUsuario> TipoUsuarios { get; set; }

    public virtual DbSet<TipousuarioMenu> TipousuarioMenus { get; set; }

    public virtual DbSet<TransaccionesSaldo> TransaccionesSaldos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VersionesMovil> VersionesMovils { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvisosMovil>(entity =>
        {
            entity.HasKey(e => e.Idaviso).HasName("PK__avisos_m__7564D874F9F744E2");

            entity.ToTable("avisos_movil");

            entity.Property(e => e.Idaviso).HasColumnName("idaviso");
            entity.Property(e => e.Detalle)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.Encabezado)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("encabezado");
            entity.Property(e => e.Esdisponible)
                .HasDefaultValue(true)
                .HasColumnName("esdisponible");
            entity.Property(e => e.TipoUsuarioDirigido).HasColumnName("tipo_usuario_dirigido");
            entity.Property(e => e.VisibleHasta)
                .HasColumnType("datetime")
                .HasColumnName("visible_hasta");

            entity.HasOne(d => d.TipoUsuarioDirigidoNavigation).WithMany(p => p.AvisosMovils)
                .HasForeignKey(d => d.TipoUsuarioDirigido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__avisos_mo__tipo___06CD04F7");
        });

        modelBuilder.Entity<Capacitacion>(entity =>
        {
            entity.HasKey(e => e.IdCapacitacion).HasName("PK__capacita__FA471D9B5398CDCD");

            entity.ToTable("capacitacion");

            entity.Property(e => e.IdCapacitacion).HasColumnName("id_capacitacion");
            entity.Property(e => e.CalificacionAprov)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("calificacion_aprov");
            entity.Property(e => e.CalificacionMaxima)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("calificacion_maxima");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Estatusid).HasColumnName("estatusid");
            entity.Property(e => e.FechaExpiracion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_expiracion");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaModifico)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modifico");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombreCapacitacion)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("nombre_capacitacion");
            entity.Property(e => e.UrlDocumento)
                .IsUnicode(false)
                .HasColumnName("url_documento");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Capacitacions)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__capacitac__estat__797309D9");
        });

        modelBuilder.Entity<CapacitacionUsuario>(entity =>
        {
            entity.HasKey(e => new { e.Usuarioid, e.Capacitacionid, e.Estatusid }).HasName("PK__capacita__D43DED77CFBC58C5");

            entity.ToTable("capacitacion_usuario");

            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");
            entity.Property(e => e.Capacitacionid).HasColumnName("capacitacionid");
            entity.Property(e => e.Estatusid).HasColumnName("estatusid");
            entity.Property(e => e.Calificacion)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("calificacion");
            entity.Property(e => e.FechaFinalizacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_finalizacion");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");

            entity.HasOne(d => d.Capacitacion).WithMany(p => p.CapacitacionUsuarios)
                .HasForeignKey(d => d.Capacitacionid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__capacitac__capac__7B5B524B");

            entity.HasOne(d => d.Estatus).WithMany(p => p.CapacitacionUsuarios)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__capacitac__estat__7C4F7684");

            entity.HasOne(d => d.Usuario).WithMany(p => p.CapacitacionUsuarios)
                .HasForeignKey(d => d.Usuarioid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__capacitac__usuar__7A672E12");
        });

        modelBuilder.Entity<CertificacionesExperiencium>(entity =>
        {
            entity.HasKey(e => e.IdCertificacion).HasName("PK__certific__D22535EED8546E71");

            entity.ToTable("certificaciones_experiencia");

            entity.Property(e => e.IdCertificacion).HasColumnName("id_certificacion");
            entity.Property(e => e.Descripcion)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.DocumentoId).HasColumnName("documento_id");
            entity.Property(e => e.ExperienciaAnios).HasColumnName("experiencia_anios");
            entity.Property(e => e.FechaCertificacion).HasColumnName("fecha_certificacion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.InstitucionEmisora)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("institucion_emisora");
            entity.Property(e => e.PersonaId).HasColumnName("persona_id");
            entity.Property(e => e.TipoCertificacion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo_certificacion");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.Vigente)
                .HasDefaultValue(true)
                .HasColumnName("vigente");

            entity.HasOne(d => d.Documento).WithMany(p => p.CertificacionesExperiencia)
                .HasForeignKey(d => d.DocumentoId)
                .HasConstraintName("FK__certifica__docum__22751F6C");

            entity.HasOne(d => d.Persona).WithMany(p => p.CertificacionesExperiencia)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__certifica__perso__1DB06A4F");
        });

        modelBuilder.Entity<ComentariosUsuario>(entity =>
        {
            entity.HasKey(e => e.IdComentarios).HasName("PK__comentar__AA5CFE249B42F396");

            entity.ToTable("comentarios_usuario");

            entity.Property(e => e.IdComentarios).HasColumnName("id_comentarios");
            entity.Property(e => e.Calificacion).HasColumnName("calificacion");
            entity.Property(e => e.Comentario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("comentario");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.PersonaEmisorid).HasColumnName("persona_emisorid");
            entity.Property(e => e.PersonaReceptorid).HasColumnName("persona_receptorid");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.PersonaEmisor).WithMany(p => p.ComentariosUsuarioPersonaEmisors)
                .HasForeignKey(d => d.PersonaEmisorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comentari__perso__19DFD96B");

            entity.HasOne(d => d.PersonaReceptor).WithMany(p => p.ComentariosUsuarioPersonaReceptors)
                .HasForeignKey(d => d.PersonaReceptorid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__comentari__perso__18EBB532");
        });

        modelBuilder.Entity<Contrato>(entity =>
        {
            entity.HasKey(e => e.IdContrato).HasName("PK__contrato__FF5F2A569E45F86F");

            entity.ToTable("contrato");

            entity.Property(e => e.IdContrato).HasColumnName("id_contrato");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaModifico)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modifico");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.PersonaidCliente).HasColumnName("personaid_cliente");
            entity.Property(e => e.PersonaidCuidador).HasColumnName("personaid_cuidador");
            entity.Property(e => e.UsuarioModifico)
                .HasColumnType("datetime")
                .HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Contratos)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__contrato__estatu__2B0A656D");

            entity.HasOne(d => d.PersonaidClienteNavigation).WithMany(p => p.ContratoPersonaidClienteNavigations)
                .HasForeignKey(d => d.PersonaidCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__contrato__person__2A164134");

            entity.HasOne(d => d.PersonaidCuidadorNavigation).WithMany(p => p.ContratoPersonaidCuidadorNavigations)
                .HasForeignKey(d => d.PersonaidCuidador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__contrato__person__29221CFB");
        });

        modelBuilder.Entity<ContratoItem>(entity =>
        {
            entity.HasKey(e => e.IdContratoitem).HasName("PK__contrato__75024B51C2025E36");

            entity.ToTable("contrato_item");

            entity.Property(e => e.IdContratoitem).HasColumnName("id_contratoitem");
            entity.Property(e => e.ContratoId).HasColumnName("contrato_id");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaAceptacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_aceptacion");
            entity.Property(e => e.FechaFinCuidado)
                .HasColumnType("datetime")
                .HasColumnName("fecha_fin_cuidado");
            entity.Property(e => e.FechaInicioCuidado)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio_cuidado");
            entity.Property(e => e.HorarioFinPropuesto)
                .HasColumnType("datetime")
                .HasColumnName("horario_fin_propuesto");
            entity.Property(e => e.HorarioInicioPropuesto)
                .HasColumnType("datetime")
                .HasColumnName("horario_inicio_propuesto");
            entity.Property(e => e.ImporteTotal)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("importe_total");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("observaciones");

            entity.HasOne(d => d.Contrato).WithMany(p => p.ContratoItems)
                .HasForeignKey(d => d.ContratoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__contrato___contr__3493CFA7");

            entity.HasOne(d => d.Estatus).WithMany(p => p.ContratoItems)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__contrato___estat__3587F3E0");
        });

        modelBuilder.Entity<CuentaBancarium>(entity =>
        {
            entity.HasKey(e => e.IdCuentabancaria).HasName("PK__cuenta_b__C07EC3A5B3175CA9");

            entity.ToTable("cuenta_bancaria");

            entity.Property(e => e.IdCuentabancaria).HasColumnName("id_cuentabancaria");
            entity.Property(e => e.ClabeInterbancaria)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("clabe_interbancaria");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombrebanco)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("nombrebanco");
            entity.Property(e => e.NumeroCuenta)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("numero_cuenta");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Usuario).WithMany(p => p.CuentaBancaria)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__cuenta_ba__usuar__02FC7413");
        });

        modelBuilder.Entity<DatosMedico>(entity =>
        {
            entity.HasKey(e => e.IdDatosmedicos).HasName("PK__datos_me__762B8F21D764240F");

            entity.ToTable("datos_medicos");

            entity.Property(e => e.IdDatosmedicos).HasColumnName("id_datosmedicos");
            entity.Property(e => e.Alergias)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("alergias");
            entity.Property(e => e.AntecedentesMedicos)
                .IsUnicode(false)
                .HasColumnName("antecedentes_medicos");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombreMedicofamiliar)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_medicofamiliar");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("observaciones");
            entity.Property(e => e.TelefonoMedicofamiliar)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_medicofamiliar");
            entity.Property(e => e.TipoSanguineo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_sanguineo");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
        });

        modelBuilder.Entity<Documentacion>(entity =>
        {
            entity.HasKey(e => e.IdDocumentacion).HasName("PK__document__965A0B078F1A3353");

            entity.ToTable("documentacion");

            entity.Property(e => e.IdDocumentacion).HasColumnName("id_documentacion");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaEmision).HasColumnName("fecha_emision");
            entity.Property(e => e.FechaExpiracion).HasColumnName("fecha_expiracion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombreDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_documento");
            entity.Property(e => e.PersonaId).HasColumnName("persona_id");
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("tipo_documento");
            entity.Property(e => e.UrlDocumento)
                .IsUnicode(false)
                .HasColumnName("url_documento");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.Version)
                .HasDefaultValue(1m)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("version");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Documentacions)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__documenta__estat__6EF57B66");

            entity.HasOne(d => d.Persona).WithMany(p => p.Documentacions)
                .HasForeignKey(d => d.PersonaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__documenta__perso__17F790F9");
        });

        modelBuilder.Entity<Domicilio>(entity =>
        {
            entity.HasKey(e => e.IdDomicilio).HasName("PK__domicili__A0CCE5C257BFBA67");

            entity.ToTable("domicilio");

            entity.Property(e => e.IdDomicilio).HasColumnName("id_domicilio");
            entity.Property(e => e.Calle)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("calle");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ciudad");
            entity.Property(e => e.Colonia)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("colonia");
            entity.Property(e => e.Estado)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estado");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NumeroExterior)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numero_exterior");
            entity.Property(e => e.NumeroInterior)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("numero_interior");
            entity.Property(e => e.Pais)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasDefaultValue("MÉXICO")
                .HasColumnName("pais");
            entity.Property(e => e.Referencias)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("referencias");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Domicilios)
                .HasForeignKey(d => d.EstatusId)
                .HasConstraintName("FK__domicilio__estat__6A30C649");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.HasKey(e => e.IdEstatus).HasName("PK__estatus__3B2CE2736DA93B89");

            entity.ToTable("estatus");

            entity.Property(e => e.IdEstatus).HasColumnName("id_estatus");
            entity.Property(e => e.Codigo)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("codigo");
            entity.Property(e => e.Color)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("color");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.TipoestatusId).HasColumnName("tipoestatus_id");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Tipoestatus).WithMany(p => p.Estatuses)
                .HasForeignKey(d => d.TipoestatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__estatus__tipoest__6B24EA82");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.IdFeedback).HasName("PK__feedback__36BC86300AC81674");

            entity.ToTable("feedback");

            entity.Property(e => e.IdFeedback).HasColumnName("id_feedback");
            entity.Property(e => e.Categoria)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("categoria");
            entity.Property(e => e.Cuerpo)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("cuerpo");
            entity.Property(e => e.Estatusid).HasColumnName("estatusid");
            entity.Property(e => e.Fecha).HasColumnName("fecha");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.FechaResolucion).HasColumnName("fecha_resolucion");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.UsuarioidRemitente).HasColumnName("usuarioid_remitente");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedback__estatu__0D44F85C");

            entity.HasOne(d => d.UsuarioidRemitenteNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UsuarioidRemitente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__feedback__usuari__0C50D423");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PK__menus__68A1D9DB4DF24053");

            entity.ToTable("menus");

            entity.Property(e => e.IdMenu).HasColumnName("id_menu");
            entity.Property(e => e.DescripcionMenu)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("descripcion_menu");
            entity.Property(e => e.Endpoint)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("endpoint");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombreMenu)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_menu");
            entity.Property(e => e.RutaMenu)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ruta_menu");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Menus)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__menus__estatus_i__76969D2E");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodopago).HasName("PK__metodo_p__47952DC9100ADDF4");

            entity.ToTable("metodo_pago");

            entity.Property(e => e.IdMetodopago).HasColumnName("id_metodopago");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
        });

        modelBuilder.Entity<MetodoPagoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdMetodousuario).HasName("PK__metodo_p__40B7B92EE1AACDF5");

            entity.ToTable("metodo_pago_usuario");

            entity.Property(e => e.IdMetodousuario).HasColumnName("id_metodousuario");
            entity.Property(e => e.Ccv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ccv");
            entity.Property(e => e.FechaModifico)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modifico");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.FechaVencimiento).HasColumnName("fecha_vencimiento");
            entity.Property(e => e.NombreBeneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_beneficiario");
            entity.Property(e => e.NumeroTarjeta)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("numero_tarjeta");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.VecesUsada).HasColumnName("veces_usada");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MetodoPagoUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__metodo_pa__usuar__2180FB33");
        });

        modelBuilder.Entity<MovimientoCuentum>(entity =>
        {
            entity.HasKey(e => e.IdMovimientocuenta).HasName("PK__movimien__4669B8FF5FEFC12A");

            entity.ToTable("movimiento_cuenta");

            entity.Property(e => e.IdMovimientocuenta).HasColumnName("id_movimientocuenta");
            entity.Property(e => e.ConceptoMovimiento)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("concepto_movimiento");
            entity.Property(e => e.CuentabancariaId).HasColumnName("cuentabancaria_id");
            entity.Property(e => e.FechaMovimiento)
                .HasColumnType("datetime")
                .HasColumnName("fecha_movimiento");
            entity.Property(e => e.ImporteMovimiento)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("importe_movimiento");
            entity.Property(e => e.MetodoPagoid).HasColumnName("metodo_pagoid");
            entity.Property(e => e.NumeroseguimientoBanco)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("numeroseguimiento_banco");
            entity.Property(e => e.SaldoActual)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("saldo_actual");
            entity.Property(e => e.SaldoAnterior)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("saldo_anterior");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_movimiento");

            entity.HasOne(d => d.Cuentabancaria).WithMany(p => p.MovimientoCuenta)
                .HasForeignKey(d => d.CuentabancariaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__movimient__cuent__04E4BC85");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.MovimientoCuenta)
                .HasForeignKey(d => d.MetodoPagoid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__movimient__metod__05D8E0BE");
        });

        modelBuilder.Entity<NivelUsuario>(entity =>
        {
            entity.HasKey(e => e.IdNivelusuario).HasName("PK__nivel_us__33E4C6809F155FCD");

            entity.ToTable("nivel_usuario");

            entity.Property(e => e.IdNivelusuario).HasColumnName("id_nivelusuario");
            entity.Property(e => e.ColorNivel)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("color_nivel");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.ImagenurlNivel)
                .IsUnicode(false)
                .HasColumnName("imagenurl_nivel");
            entity.Property(e => e.NombreNivel)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("nombre_nivel");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.VigenciaHasta)
                .HasColumnType("datetime")
                .HasColumnName("vigencia_hasta");
        });

        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.HasKey(e => e.IdNotificacion).HasName("PK__notifica__8270F9A59158B605");

            entity.ToTable("notificaciones");

            entity.Property(e => e.IdNotificacion).HasColumnName("id_notificacion");
            entity.Property(e => e.DescripcionNoti)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion_noti");
            entity.Property(e => e.Estatusid).HasColumnName("estatusid");
            entity.Property(e => e.FechaModifico)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modifico");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.PersonaidNoti).HasColumnName("personaid_noti");
            entity.Property(e => e.RutaMenu)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ruta_menu");
            entity.Property(e => e.TituloNoti)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo_noti");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__notificac__estat__31B762FC");

            entity.HasOne(d => d.PersonaidNotiNavigation).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.PersonaidNoti)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__notificac__perso__30C33EC3");
        });

        modelBuilder.Entity<Padecimiento>(entity =>
        {
            entity.HasKey(e => e.IdPadecimiento).HasName("PK__padecimi__D4D1535ED5707C19");

            entity.ToTable("padecimientos");

            entity.Property(e => e.IdPadecimiento).HasColumnName("id_padecimiento");
            entity.Property(e => e.DatosmedicosId).HasColumnName("datosmedicos_id");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PadeceDesde).HasColumnName("padece_desde");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Datosmedicos).WithMany(p => p.Padecimientos)
                .HasForeignKey(d => d.DatosmedicosId)
                .HasConstraintName("FK__padecimie__datos__6754599E");
        });

        modelBuilder.Entity<PersonaFisica>(entity =>
        {
            entity.HasKey(e => e.IdPersona).HasName("PK__persona___228148B0AB62F7AD");

            entity.ToTable("persona_fisica");

            entity.Property(e => e.IdPersona).HasColumnName("id_persona");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido_materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("apellido_paterno");
            entity.Property(e => e.AvatarImage)
                .IsUnicode(false)
                .HasColumnName("avatar_image");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.Curp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("curp");
            entity.Property(e => e.DatosMedicosid)
                .HasDefaultValue(0)
                .HasColumnName("datos_medicosid");
            entity.Property(e => e.DomicilioId).HasColumnName("domicilio_id");
            entity.Property(e => e.EsFamiliar).HasColumnName("esFamiliar");
            entity.Property(e => e.EstadoCivil)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("estado_civil");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.Genero)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("genero");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NombrecompletoFamiliar)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("nombrecompleto_familiar");
            entity.Property(e => e.Rfc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("rfc");
            entity.Property(e => e.TelefonoEmergencia)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_emergencia");
            entity.Property(e => e.TelefonoMovil)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_movil");
            entity.Property(e => e.TelefonoParticular)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telefono_particular");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.DatosMedicos).WithMany(p => p.PersonaFisicas)
                .HasForeignKey(d => d.DatosMedicosid)
                .HasConstraintName("FK__persona_f__datos__66603565");

            entity.HasOne(d => d.Domicilio).WithMany(p => p.PersonaFisicas)
                .HasForeignKey(d => d.DomicilioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__persona_f__domic__656C112C");

            entity.HasOne(d => d.Estatus).WithMany(p => p.PersonaFisicas)
                .HasForeignKey(d => d.EstatusId)
                .HasConstraintName("FK__persona_f__estat__14270015");

            entity.HasOne(d => d.Usuario).WithMany(p => p.PersonaFisicas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__persona_f__usuar__1332DBDC");
        });

        modelBuilder.Entity<PersonaMoral>(entity =>
        {
            entity.HasKey(e => e.IdPersonamoral).HasName("PK__persona___FD080125C2855A80");

            entity.ToTable("persona_moral");

            entity.Property(e => e.IdPersonamoral).HasColumnName("id_personamoral");
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("correo_electronico");
            entity.Property(e => e.DireccionId).HasColumnName("direccion_id");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombreComercial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_comercial");
            entity.Property(e => e.RazonSocial)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("razon_social");
            entity.Property(e => e.Rfc)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("rfc");
            entity.Property(e => e.Telefono)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Direccion).WithMany(p => p.PersonaMorals)
                .HasForeignKey(d => d.DireccionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__persona_m__direc__693CA210");
        });

        modelBuilder.Entity<SalarioCuidador>(entity =>
        {
            entity.HasKey(e => e.IdSueldonivel).HasName("PK__tmp_ms_x__813986A02999E20C");

            entity.ToTable("salario_cuidador");

            entity.Property(e => e.IdSueldonivel).HasColumnName("id_sueldonivel");
            entity.Property(e => e.Concurrencia).HasColumnName("concurrencia");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("dia_semana");
            entity.Property(e => e.Estatusid)
                .HasDefaultValue((byte)1)
                .HasColumnName("estatusid");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.HoraFin).HasColumnName("hora_fin");
            entity.Property(e => e.HoraInicio).HasColumnName("hora_inicio");
            entity.Property(e => e.PrecioPorHora)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("precio_por_hora");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.Usuarioid).HasColumnName("usuarioid");

            entity.HasOne(d => d.Usuario).WithMany(p => p.SalarioCuidadors)
                .HasForeignKey(d => d.Usuarioid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__salario_c__usuar__09746778");
        });

        modelBuilder.Entity<Saldo>(entity =>
        {
            entity.HasKey(e => e.IdSaldo).HasName("PK__saldos__6C46B2BD28F86841");

            entity.ToTable("saldos");

            entity.Property(e => e.IdSaldo).HasColumnName("id_saldo");
            entity.Property(e => e.Estatusid).HasColumnName("estatusid");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.SaldoActual)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("saldo_actual");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Saldos)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__saldos__estatusi__00200768");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Saldos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__saldos__usuario___7E37BEF6");
        });

        modelBuilder.Entity<SesionesUsuario>(entity =>
        {
            entity.HasKey(e => e.IdSesion).HasName("PK__sesiones__8D3F9DFE52FDD42D");

            entity.ToTable("sesiones_usuario");

            entity.Property(e => e.IdSesion).HasColumnName("id_sesion");
            entity.Property(e => e.Ipsesion).HasColumnName("ipsesion");
            entity.Property(e => e.IsSesionactiva).HasColumnName("is_sesionactiva");
            entity.Property(e => e.NombreEquipo)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_equipo");
            entity.Property(e => e.SistemaOperativo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("sistema_operativo");
            entity.Property(e => e.TokenFirebase)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("token_firebase");
            entity.Property(e => e.UltimocambioContrasenia)
                .HasColumnType("datetime")
                .HasColumnName("ultimocambio_contrasenia");
            entity.Property(e => e.UltimoinicioSesion)
                .HasColumnType("datetime")
                .HasColumnName("ultimoinicio_sesion");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

            entity.HasOne(d => d.Usuario).WithMany(p => p.SesionesUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__sesiones___usuar__75A278F5");
        });

        modelBuilder.Entity<TareasContrato>(entity =>
        {
            entity.HasKey(e => e.IdTareas).HasName("fk_id_tareas_contrato");

            entity.ToTable("tareas_contrato");

            entity.Property(e => e.IdTareas).HasColumnName("id_tareas");
            entity.Property(e => e.ContratoitemId).HasColumnName("contratoitem_id");
            entity.Property(e => e.DescripcionTarea)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("descripcion_tarea");
            entity.Property(e => e.EstatusId).HasColumnName("estatus_id");
            entity.Property(e => e.FechaARealizar)
                .HasColumnType("datetime")
                .HasColumnName("fecha_a_realizar");
            entity.Property(e => e.FechaFinalizacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_finalizacion");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fecha_inicio");
            entity.Property(e => e.FechaPospuesta)
                .HasColumnType("datetime")
                .HasColumnName("fecha_pospuesta");
            entity.Property(e => e.TipoTarea)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("tipo_tarea");
            entity.Property(e => e.TituloTarea)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("titulo_tarea");

            entity.HasOne(d => d.Contratoitem).WithMany(p => p.TareasContratos)
                .HasForeignKey(d => d.ContratoitemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tareas_co__contr__395884C4");

            entity.HasOne(d => d.Estatus).WithMany(p => p.TareasContratos)
                .HasForeignKey(d => d.EstatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tareas_co__estat__367C1819");
        });

        modelBuilder.Entity<TarjetaUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTarjeta).HasName("PK__tarjeta___E92BCFEA2970FC93");

            entity.ToTable("tarjeta_usuario");

            entity.Property(e => e.IdTarjeta).HasColumnName("id_tarjeta");
            entity.Property(e => e.Banco)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("banco");
            entity.Property(e => e.Beneficiario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("beneficiario");
            entity.Property(e => e.Ccv)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("ccv");
            entity.Property(e => e.ConcurrenciaUso).HasColumnName("concurrencia_uso");
            entity.Property(e => e.NumeroTarjeta)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("numero_tarjeta");
            entity.Property(e => e.TipoTarjeta)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("tipo_tarjeta");
            entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");
            entity.Property(e => e.Vencimiento)
                .HasColumnType("datetime")
                .HasColumnName("vencimiento");

            entity.HasOne(d => d.Usuario).WithMany(p => p.TarjetaUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tarjeta_u__usuar__46B27FE2");
        });

        modelBuilder.Entity<TipoEstatus>(entity =>
        {
            entity.HasKey(e => e.IdTipoestatus).HasName("PK__tipo_est__8036499E56CD50F9");

            entity.ToTable("tipo_estatus");

            entity.Property(e => e.IdTipoestatus).HasColumnName("id_tipoestatus");
            entity.Property(e => e.NombreTipoestatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre_tipoestatus");
        });

        modelBuilder.Entity<TipoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdTipousuario).HasName("PK__tipo_usu__6B5A4DE0E6AD184B");

            entity.ToTable("tipo_usuario");

            entity.Property(e => e.IdTipousuario).HasColumnName("id_tipousuario");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombreTipo)
                .HasMaxLength(60)
                .IsUnicode(false)
                .HasColumnName("nombre_tipo");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
        });

        modelBuilder.Entity<TipousuarioMenu>(entity =>
        {
            entity.HasKey(e => e.IdUsuariomenu).HasName("PK__usuario___8FCAEEFD5898C2CB");

            entity.ToTable("tipousuario_menu");

            entity.Property(e => e.IdUsuariomenu).HasColumnName("id_usuariomenu");
            entity.Property(e => e.MenuId).HasColumnName("menu_id");
            entity.Property(e => e.TipousuarioId).HasColumnName("tipousuario_id");

            entity.HasOne(d => d.Menu).WithMany(p => p.TipousuarioMenus)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario_m__menu___778AC167");

            entity.HasOne(d => d.Tipousuario).WithMany(p => p.TipousuarioMenus)
                .HasForeignKey(d => d.TipousuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tipousuar__tipou__1AD3FDA4");
        });

        modelBuilder.Entity<TransaccionesSaldo>(entity =>
        {
            entity.HasKey(e => e.IdTransaccionsaldo).HasName("PK__transacc__C0C63E94FB861DBA");

            entity.ToTable("transacciones_saldos");

            entity.Property(e => e.IdTransaccionsaldo).HasColumnName("id_transaccionsaldo");
            entity.Property(e => e.ConceptoTransaccion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("concepto_transaccion");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.FechaTransaccion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_transaccion");
            entity.Property(e => e.ImporteTransaccion)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("importe_transaccion");
            entity.Property(e => e.MetodoPagoid).HasColumnName("metodo_pagoid");
            entity.Property(e => e.SaldoActual)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("saldo_actual");
            entity.Property(e => e.SaldoAnterior)
                .HasColumnType("decimal(10, 4)")
                .HasColumnName("saldo_anterior");
            entity.Property(e => e.SaldoId).HasColumnName("saldo_id");
            entity.Property(e => e.TipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_movimiento");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");

            entity.HasOne(d => d.MetodoPago).WithMany(p => p.TransaccionesSaldos)
                .HasForeignKey(d => d.MetodoPagoid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__transacci__metod__02084FDA");

            entity.HasOne(d => d.Saldo).WithMany(p => p.TransaccionesSaldos)
                .HasForeignKey(d => d.SaldoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__transacci__saldo__01142BA1");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__usuario__4E3E04AD79BD4F08");

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Contrasenia)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("contrasenia");
            entity.Property(e => e.Estatusid).HasColumnName("estatusid");
            entity.Property(e => e.FechaModificacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_modificacion");
            entity.Property(e => e.FechaRegistro)
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.TipoUsuarioid).HasColumnName("tipo_usuarioid");
            entity.Property(e => e.Usuario1)
                .HasMaxLength(90)
                .IsUnicode(false)
                .HasColumnName("usuario");
            entity.Property(e => e.UsuarioModifico).HasColumnName("usuario_modifico");
            entity.Property(e => e.UsuarioRegistro).HasColumnName("usuario_registro");
            entity.Property(e => e.UsuarionivelId).HasColumnName("usuarionivel_id");

            entity.HasOne(d => d.Estatus).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.Estatusid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario__estatus__71D1E811");

            entity.HasOne(d => d.TipoUsuario).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.TipoUsuarioid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario__tipo_us__70DDC3D8");

            entity.HasOne(d => d.Usuarionivel).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.UsuarionivelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__usuario__usuario__6FE99F9F");
        });

        modelBuilder.Entity<VersionesMovil>(entity =>
        {
            entity.HasKey(e => e.IdVersiones).HasName("PK__versione__4842269620B2C3CD");

            entity.ToTable("versiones_movil");

            entity.Property(e => e.IdVersiones).HasColumnName("id_versiones");
            entity.Property(e => e.DescripcionVersion)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("descripcion_version");
            entity.Property(e => e.FechaDespliegue)
                .HasColumnType("datetime")
                .HasColumnName("fecha_despliegue");
            entity.Property(e => e.HashCommit)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("hash_commit");
            entity.Property(e => e.NumeroVersion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numero_version");
            entity.Property(e => e.ObservacionesPost)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("observaciones_post");
            entity.Property(e => e.UsuarioDesplego).HasColumnName("usuario_desplego");

            entity.HasOne(d => d.UsuarioDesplegoNavigation).WithMany(p => p.VersionesMovils)
                .HasForeignKey(d => d.UsuarioDesplego)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__versiones__usuar__07C12930");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
