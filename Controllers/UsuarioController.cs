using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Cuidador.Dto.User.ListarDatoUsuario;
using Cuidador.Dto.User.IniciarSesion;
using Cuidador.Dto.User.ListarDatoUsuario;
using Cuidador.Dto.User.RegistrarUsuario;

namespace Cuidador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // VARIABLE DE COTEXTO DE BD
        private readonly SysCuidadorV2Context _baseDatos;

        public UsuarioController(SysCuidadorV2Context baseDatos)
        {
            this._baseDatos = baseDatos;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Log([FromBody] UsuarioLoginDTO user)
        {
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.Usuario1 == user.Usuario);

            if (usuario == null) {
                var res = new
                {
                    error = "No se encontro el usuario"
                };

                return BadRequest(res);
            }

            if(usuario.Contrasenia == user.Contrasenia)
            {
                var persona = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == usuario.IdUsuario && p.EsFamiliar == 0).ToArrayAsync();
                var menus = await (
                    from Menu in _baseDatos.Menus
                    where (
                        from TipousuarioMenu in _baseDatos.TipousuarioMenus
                        where (
                            from Usuario in _baseDatos.Usuarios
                            where Usuario.Usuario1 == user.Usuario
                            select Usuario.TipoUsuarioid
                            )
                        .Contains(TipousuarioMenu.TipousuarioId)
                         select TipousuarioMenu.MenuId)
                    .Contains(Menu.IdMenu)
                    select Menu).ToListAsync();

                var modelOut = new OutLogin
                {
                    IdUsuario = usuario.IdUsuario,
                    UsuarionivelId = usuario.UsuarionivelId,
                    TipoUsuarioid = usuario.TipoUsuarioid,
                    Estatusid = usuario.Estatusid,
                    Usuario1 = usuario.Usuario1,
                    Contrasenia = usuario.Contrasenia,
                    PersonaFisicas = persona,
                    Menu = menus
                };

                return Ok(modelOut);
            }
            else
            {
                var res = new
                {
                    error = "Contraseña no valida"
                };
                return BadRequest(res);
            }            
        }

        [HttpPost]
        [Route("loginCuidador")]
        public async Task<IActionResult> LoginCuidador([FromBody] UsuarioLoginDTO user)
        {
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.Usuario1 == user.Usuario && u.TipoUsuarioid == 1);

            if (usuario == null)
            {
                var res = new
                {
                    error = "No se encontro el usuario"
                };

                return BadRequest(res);
            }

            if (usuario.Contrasenia == user.Contrasenia)
            {
                
                var persona = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.UsuarioId == usuario.IdUsuario);
               
                /* 
                 * persona != null ? 
                 * es una validacion que de acuerdo de si se cumple la condicion 
                 * continuara con las demas lineass de pues del simbolo ? 
                 * de lo contrario :  se queda null
                */

                var domicilio = persona != null ? await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == persona.DomicilioId) : null;

                var datosMedicos = persona != null ? await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == persona.DatosMedicosid) : null;

                var padecimientos = datosMedicos != null ? await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == datosMedicos.IdDatosmedicos).ToListAsync() : new List<Padecimiento>();

                var documentacion = persona != null ? await _baseDatos.Documentacions.Where(d => d.PersonaId == persona.IdPersona).ToListAsync() : new List<Documentacion>();

                var cer = persona != null ? await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == persona.IdPersona).ToListAsync() : new List<CertificacionesExperiencium>();

                var modelOut = new OutLoginCuidador
                {
                    domicilio = domicilio,
                    padecimientos = padecimientos,
                    usuario = usuario,
                    personaFisica = persona,
                    documentaciones = documentacion,
                    certificaciones = cer
                };

                return Ok(modelOut);
                
            }
            else
            {
                var res = new
                {
                    error = "Contraseña no valida"
                };
                return BadRequest(res);
            }
        }

        [HttpPost]
        [Route("loginCliente")]
        public async Task<IActionResult> Logincliente([FromBody] UsuarioLoginDTO user)
        {
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.Usuario1 == user.Usuario && u.TipoUsuarioid == 2);

            if (usuario == null)
            {
                var res = new
                {
                    error = "No se encontro el usuario"
                };

                return BadRequest(res);
            }

            if (usuario.Contrasenia == user.Contrasenia)
            {

                var persona = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.UsuarioId == usuario.IdUsuario && p.EsFamiliar == 1);
                if(persona == null)
                {
                    var res = new
                    {
                        error = "no se encuntra el usuario del familiar"
                    };
                    return BadRequest(res);
                }
                /* 
                 * persona != null ? 
                 * es una validacion que de acuerdo de si se cumple la condicion 
                 * continuara con las demas lineass de pues del simbolo ? 
                 * de lo contrario :  se queda null
                */

                // datos de usuarioFamiliar
                var domicilio = persona != null ? await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == persona.DomicilioId) : null;

                var datosMedicos = persona != null ? await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == persona.DatosMedicosid) : null;

                var padecimientos = datosMedicos != null ? await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == datosMedicos.IdDatosmedicos).ToListAsync() : new List<Padecimiento>();

                var documentacion = persona != null ? await _baseDatos.Documentacions.Where(d => d.PersonaId == persona.IdPersona).ToListAsync() : new List<Documentacion>();

                var cer = persona != null ? await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == persona.IdPersona).ToListAsync() : new List<CertificacionesExperiencium>();

                // datos de persona que necesita de familiar
                var personaAdulto = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == usuario.IdUsuario).ToArrayAsync();

                var listaAdulto = new List<AdultoDTO>();

                foreach (var adlt in personaAdulto)
                {
                    var dom = adlt != null ? await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == adlt.DomicilioId) : null;

                    var dataMedico = adlt != null ? await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == adlt.DatosMedicosid) : null;

                    var padec = dataMedico != null ? await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == dataMedico.IdDatosmedicos).ToListAsync() : new List<Padecimiento>();

                    var document = adlt != null ? await _baseDatos.Documentacions.Where(d => d.PersonaId == adlt.IdPersona).ToListAsync() : new List<Documentacion>();

                    var cert = adlt != null ? await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == adlt.IdPersona).ToListAsync() : new List<CertificacionesExperiencium>();
                    
                    var adulto = new AdultoDTO
                    {
                        domicilio = dom,
                        DatosMedico = dataMedico,
                        Padecimiento = padec,
                        PersonaFisica = adlt,
                        documentacion = document
                    };

                    listaAdulto.Add(adulto);
                }

                var modelOut = new OutLoginCliente
                {
                    domicilio = domicilio,
                    datosMedico = datosMedicos,
                    padecimiento = padecimientos,
                    usuario = usuario,
                    personaFisica = persona,
                    documentacion = documentacion,
                    adulto = listaAdulto
                };

                return Ok(modelOut);

            }
            else
            {
                var res = new
                {
                    error = "Contraseña no valida"
                };
                return BadRequest(res);
            }
        }


        [HttpGet]
        [Route("verUsuario/{idUser}")]
        public async Task<IActionResult> verUsuario(int idUser)
        {
            var us = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.IdUsuario == idUser && u.TipoUsuarioid == 1);
            if (us == null)
            {
                var res = new
                {
                    error = "no se encontro el usuario"
                };
                return BadRequest(res);
            }
            var pers = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId ==  idUser).ToListAsync();
            var tipUsuario = await _baseDatos.TipoUsuarios.SingleOrDefaultAsync(t => t.IdTipousuario == us.TipoUsuarioid);
            var nivelUsuario = await _baseDatos.NivelUsuarios.SingleOrDefaultAsync(n => n.IdNivelusuario == us.UsuarionivelId);
            var listaComen = new List<ComentariosUsuario>();
            var listaDomicilio = new List<Domicilio>();
            foreach(var p in pers)
            {
                var comen = await _baseDatos.ComentariosUsuarios.SingleOrDefaultAsync(c => c.PersonaReceptor.IdPersona == p.IdPersona);
                var domicilio = await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == p.DomicilioId);
                listaComen.Add(comen);
                listaDomicilio.Add(domicilio);
            }

            var outModel = new OUTVerUsuario
            {
                id_usuario = us.IdUsuario,
                usuario = us.Usuario1,
                nivelUsuario = nivelUsuario.NombreNivel,
                tipo_usuario = tipUsuario.NombreTipo,
                personaFisica = pers,
                comentarios_usuario = listaComen,
                domicilio = listaDomicilio
            };
            return Ok(outModel);            
        }

        [HttpGet]
        [Route("listarDatosUsuario/{idUser}")]
        public async Task<IActionResult> listarDatos(int idUser)
        {
            /* 
             * necesita validaciones para nulos, mas de un objeto en sentencias single y no encontrados
             */
            var personas = await _baseDatos.PersonaFisicas.ToListAsync();
            var persona = personas.SingleOrDefault(p => p.UsuarioId == idUser);
            var domicilio = await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == persona.DomicilioId);            
            var datosMedicos = await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == persona.DatosMedicosid);
            var padecimientos = await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == datosMedicos.IdDatosmedicos).ToListAsync();
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.IdUsuario == idUser);
            var documentacion = await _baseDatos.Documentacions.Where(d => d.PersonaId == persona.IdPersona).ToListAsync();

            var cer = await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == persona.IdPersona).ToListAsync();
            var certificacionesExperiencia = new List<CertificacionExperienciaListarDTO>();

            foreach(var cert in cer)
            {
                var certEXp = new CertificacionExperienciaListarDTO
                {
                    Certificacion = await _baseDatos.CertificacionesExperiencia.SingleOrDefaultAsync(c => c.IdCertificacion == cert.IdCertificacion),
                    Documento = await _baseDatos.Documentacions.SingleOrDefaultAsync(d => d.IdDocumentacion == cert.DocumentoId)
                };

                certificacionesExperiencia.Add(certEXp);
            }

            if (domicilio == null || datosMedicos == null || usuario == null || persona == null)
            {
                return NotFound("no se encontraron datos domicilio/datosMed/usuario/persona");
            }

            try
            {
                var datosOut = new OutListarUsuario
                {
                    Domicilio = domicilio,
                    DatosMedicos = datosMedicos,
                    Padecimientos = padecimientos,
                    Usuario = usuario,
                    Persona = persona,
                    Documentacion = documentacion,
                    CertificacionesExperiencia = certificacionesExperiencia
                };

                return Ok(datosOut);
            }
            catch (Exception ex) {
                return BadRequest(ex);
            }            
        }

        [HttpPost("registrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistrarUsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var domicilio = new Domicilio
            {
                Calle = usuarioDTO.Domicilio.Calle,
                Colonia = usuarioDTO.Domicilio.Colonia,
                NumeroInterior = usuarioDTO.Domicilio.NumeroInterior,
                NumeroExterior = usuarioDTO.Domicilio.NumeroExterior,
                Ciudad = usuarioDTO.Domicilio.Ciudad,
                Estado = usuarioDTO.Domicilio.Estado,
                Pais = usuarioDTO.Domicilio.Pais,
                Referencias = usuarioDTO.Domicilio.Referencias,
                EstatusId = usuarioDTO.Domicilio.EstatusId,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro
            };

            _baseDatos.Domicilios.Add(domicilio);
            await _baseDatos.SaveChangesAsync();

            var datosMedicos = new DatosMedico
            {
                AntecedentesMedicos = usuarioDTO.DatosMedicos.AntecedentesMedicos,
                Alergias = usuarioDTO.DatosMedicos.Alergias,
                TipoSanguineo = usuarioDTO.DatosMedicos.TipoSanguineo,
                NombreMedicofamiliar = usuarioDTO.DatosMedicos.NombreMedicoFamiliar,
                TelefonoMedicofamiliar = usuarioDTO.DatosMedicos.TelefonoMedicoFamiliar,
                Observaciones = usuarioDTO.DatosMedicos.Observaciones,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
            };

            _baseDatos.DatosMedicos.Add(datosMedicos);
            await _baseDatos.SaveChangesAsync();

            var padecimientos = new List<Padecimiento>();

            foreach (var padecimientoDTO in usuarioDTO.Padecimientos)
            {
                var padecimiento = new Padecimiento
                {
                    DatosmedicosId = datosMedicos.IdDatosmedicos,
                    Nombre = padecimientoDTO.Nombre,
                    Descripcion = padecimientoDTO.Descripcion,
                    PadeceDesde = padecimientoDTO.PadeceDesde,
                    FechaRegistro = DateTime.Now,
                    UsuarioRegistro = padecimientoDTO.UsuarioRegistro
                };

                padecimientos.Add(padecimiento);
            }

            _baseDatos.Padecimientos.AddRange(padecimientos);
            await _baseDatos.SaveChangesAsync();

            var usuario = new Usuario
            {
                UsuarionivelId = 6,
                TipoUsuarioid = usuarioDTO.Usuario.TipoUsuarioId,
                Estatusid = usuarioDTO.Usuario.EstatusId,
                Usuario1 = usuarioDTO.Usuario.Usuario,
                Contrasenia = usuarioDTO.Usuario.Contrasenia,
                FechaRegistro = DateTime.Now,
                UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
            };            
            
            _baseDatos.Usuarios.Add(usuario);
            await _baseDatos.SaveChangesAsync();
            
            var persona = new PersonaFisica
            {
                Nombre = usuarioDTO.Persona.Nombre,
                ApellidoPaterno = usuarioDTO.Persona.ApellidoPaterno,
                ApellidoMaterno = usuarioDTO.Persona.ApellidoMaterno,
                CorreoElectronico = usuarioDTO.Persona.CorreoElectronico,
                FechaNacimiento = usuarioDTO.Persona.FechaNacimiento,
                Genero = usuarioDTO.Persona.Genero,
                EstadoCivil = usuarioDTO.Persona.EstadoCivil,
                Rfc = usuarioDTO.Persona.RFC,
                Curp = usuarioDTO.Persona.CURP,
                TelefonoParticular = usuarioDTO.Persona.TelefonoParticular,
                TelefonoMovil = usuarioDTO.Persona.TelefonoMovil,
                TelefonoEmergencia = usuarioDTO.Persona.TelefonoEmergencia,
                NombrecompletoFamiliar = usuarioDTO.Persona.NombreCompletoFamiliar,
                DomicilioId = domicilio.IdDomicilio, // Recuperado después de insertar domicilio
                DatosMedicosid = datosMedicos.IdDatosmedicos, // Recuperado después de insertar datos médicos
                AvatarImage = usuarioDTO.Persona.AvatarImage,
                EstatusId = usuarioDTO.Persona.EstatusId,
                FechaRegistro = DateTime.Now, // Se coloca en el controlador
                UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro, /*PUEDE TENER DETALLE*/
                UsuarioId = usuario.IdUsuario
            };

            _baseDatos.PersonaFisicas.Add(persona);
            await _baseDatos.SaveChangesAsync();

            var documentaciones = new List<Documentacion>();

            foreach(var listaDocumentacion in usuarioDTO.Documentacion)
            {
                var documentacion = new Documentacion
                {
                    PersonaId = persona.IdPersona,
                    TipoDocumento = listaDocumentacion.TipoDocumento,
                    NombreDocumento = listaDocumentacion.NombreDocumento,
                    UrlDocumento = listaDocumentacion.UrlDocumento,
                    FechaEmision = listaDocumentacion.FechaEmision,
                    FechaExpiracion = listaDocumentacion.FechaExpiracion,
                    Version = listaDocumentacion.Version,
                    EstatusId = listaDocumentacion.EstatusId,
                    FechaRegistro = DateTime.Now, // Se coloca en el controlador
                    UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                };

                documentaciones.Add(documentacion);
            }

            _baseDatos.AddRange(documentaciones);
            await _baseDatos.SaveChangesAsync();

            var certificacionesExp = new List<CertificacionesExperiencium>();

            foreach (var certificacionExp in usuarioDTO.CertificacionesExperiencia)
            {
                var documento = new Documentacion
                {
                    PersonaId = persona.IdPersona,
                    TipoDocumento = certificacionExp.Documento.TipoDocumento,
                    NombreDocumento = certificacionExp.Documento.NombreDocumento,
                    UrlDocumento = certificacionExp.Documento.UrlDocumento,
                    FechaEmision = certificacionExp.Documento.FechaEmision,
                    FechaExpiracion = certificacionExp.Documento.FechaExpiracion,
                    Version = certificacionExp.Documento.Version,
                    EstatusId = certificacionExp.Documento.EstatusId,
                    FechaRegistro = DateTime.Now, // Se coloca en el controlador
                    UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/

                };

                _baseDatos.Add(documento);
                await _baseDatos.SaveChangesAsync();

                var certificacionExperiencia = new CertificacionesExperiencium
                {
                    TipoCertificacion = certificacionExp.Certificacion.TipoCertificacion,
                    InstitucionEmisora = certificacionExp.Certificacion.InstitucionEmisora,
                    FechaCertificacion = certificacionExp.Certificacion.FechaCertificacion,
                    Vigente = certificacionExp.Certificacion.Vigente,
                    Descripcion = certificacionExp.Certificacion.Descripcion,
                    FechaRegistro = DateTime.Now, // Se coloca en el controlador
                    UsuarioRegistro = usuarioDTO.Domicilio.UsuarioRegistro, /*PUEDE TENER DETALLE*/
                    PersonaId = persona.IdPersona,
                    DocumentoId = documento.IdDocumentacion
                };

                certificacionesExp.Add(certificacionExperiencia);
                await _baseDatos.SaveChangesAsync();

            }

            return Ok();
        }
    }
}
