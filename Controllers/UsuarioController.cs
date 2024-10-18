using Cuidador.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Cuidador.Dto.User.ListarDatoUsuario;
using Cuidador.Dto.User.IniciarSesion;
using Cuidador.Dto.User.RegistrarUsuario;
using Cuidador.Dto.User.RegistrarFamiliar;
using Cuidador.Dto.User.ListarCuidador;
using System.Text.Json;

namespace Cuidador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // VARIABLE DE COTEXTO DE BD
        private readonly DbAaaabeCuidadorContext _baseDatos;

        public UsuarioController(DbAaaabeCuidadorContext baseDatos)
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
                try
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

                    var modelOutLogin = new OutLogin
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
                    return Ok(modelOutLogin);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
                }

                
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
        [Route("loginWeb")]
        public async Task<IActionResult> LoginCuidador([FromBody] UsuarioLoginDTO user)
        {
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.Usuario1 == user.Usuario);

            if (usuario == null)
            {
                var res = new
                {
                    error = "No se encontro el usuario"
                };

                return BadRequest(res);
            }
            // tipo cuidador
            if (usuario.Contrasenia == user.Contrasenia && usuario.TipoUsuarioid == 1)
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

                var modelOutLoginCuidador = new OutLoginCuidador
                {
                    domicilio = domicilio,
                    datosMedico = datosMedicos,
                    padecimientos = padecimientos,
                    usuario = usuario,
                    personaFisica = persona,
                    documentaciones = documentacion,
                    certificaciones = cer
                };

                return Ok(modelOutLoginCuidador);
                
            }
            // tipo cliente
            if(usuario.Contrasenia == user.Contrasenia && usuario.TipoUsuarioid == 2)
            {
                var persona = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.UsuarioId == usuario.IdUsuario && p.EsFamiliar == 1);
                if (persona == null)
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

                var listaAdultoDto = new List<AdultoDTO>();

                foreach (var adlt in personaAdulto)
                {
                    var dom = adlt != null ? await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == adlt.DomicilioId) : null;

                    var dataMedico = adlt != null ? await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == adlt.DatosMedicosid) : null;

                    var padec = dataMedico != null ? await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == dataMedico.IdDatosmedicos).ToListAsync() : new List<Padecimiento>();

                    var document = adlt != null ? await _baseDatos.Documentacions.Where(d => d.PersonaId == adlt.IdPersona).ToListAsync() : new List<Documentacion>();

                    var cert = adlt != null ? await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == adlt.IdPersona).ToListAsync() : new List<CertificacionesExperiencium>();

                    var modelAdultoDto = new AdultoDTO
                    {
                        domicilio = dom,
                        DatosMedico = dataMedico,
                        Padecimiento = padec,
                        PersonaFisica = adlt,
                        documentacion = document
                    };

                    listaAdultoDto.Add(modelAdultoDto);
                }

                var modelOut = new OutLoginCliente
                {
                    domicilio = domicilio,
                    datosMedico = datosMedicos,
                    padecimiento = padecimientos,
                    usuario = usuario,
                    personaFisica = persona,
                    documentacion = documentacion,
                    adulto = listaAdultoDto
                };

                return Ok(modelOut);
            }
            else
            {
                var res = new
                {
                    error = "Por favor intente de nuevo, si persiste contacte soporte"
                };

                return BadRequest(res);
            }
        }

        [HttpPost]
        [Route("loginWebOptimizado")]
        public async Task<IActionResult> LoginWebOptimizado([FromBody] UsuarioLoginDTO user)
        {
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.Usuario1 == user.Usuario);

            if (usuario == null)
            {
                var res = new { error = "No se encontró el usuario" };
                return BadRequest(res);
            }

            // tipo cuidador
            if (usuario.Contrasenia == user.Contrasenia && usuario.TipoUsuarioid == 1)
            {
                var persona = await _baseDatos.PersonaFisicas
                    .Include(p => p.Domicilio)
                    .Include(p => p.DatosMedicos)
                    .ThenInclude(dm => dm.Padecimientos)
                    .Include(p => p.Documentacions)
                    .Include(p => p.CertificacionesExperiencia)
                    .SingleOrDefaultAsync(p => p.UsuarioId == usuario.IdUsuario);

                if (persona == null)
                {
                    return BadRequest(new { error = "No se encontró la persona física del cuidador" });
                }

                var modelOut = new OutLoginCuidador
                {
                    domicilio = persona.Domicilio,
                    datosMedico = persona.DatosMedicos,
                    padecimientos = persona.DatosMedicos?.Padecimientos.ToList() ?? new List<Padecimiento>(),
                    usuario = usuario,
                    personaFisica = persona,
                    documentaciones = persona.Documentacions.ToList(),
                    certificaciones = persona.CertificacionesExperiencia.ToList()
                };

                return Ok(modelOut);
            }

            // tipo cliente
            if (usuario.Contrasenia == user.Contrasenia && usuario.TipoUsuarioid == 2)
            {
                var personaFamiliar = await _baseDatos.PersonaFisicas
                    .Include(p => p.Domicilio)
                    .Include(p => p.DatosMedicos)
                    .ThenInclude(dm => dm.Padecimientos)
                    .Include(p => p.Documentacions)
                    .Include(p => p.CertificacionesExperiencia)
                    .SingleOrDefaultAsync(p => p.UsuarioId == usuario.IdUsuario && p.EsFamiliar == 1);

                if (personaFamiliar == null)
                {
                    return BadRequest(new { error = "No se encuentra el usuario del familiar" });
                }

                // Datos de personas que necesitan cuidado
                var personasAdulto = await _baseDatos.PersonaFisicas
                    .Where(p => p.UsuarioId == usuario.IdUsuario)
                    .Include(p => p.Domicilio)
                    .Include(p => p.DatosMedicos)
                    .ThenInclude(dm => dm.Padecimientos)
                    .Include(p => p.Documentacions)
                    .ToListAsync();

                var listaAdulto = personasAdulto.Select(adlt => new AdultoDTO
                {
                    domicilio = adlt.Domicilio,
                    DatosMedico = adlt.DatosMedicos,
                    Padecimiento = adlt.DatosMedicos?.Padecimientos.ToList() ?? new List<Padecimiento>(),
                    PersonaFisica = adlt,
                    documentacion = adlt.Documentacions.ToList()
                }).ToList();

                var modelOut = new OutLoginCliente
                {
                    domicilio = personaFamiliar.Domicilio,
                    datosMedico = personaFamiliar.DatosMedicos,
                    padecimiento = personaFamiliar.DatosMedicos?.Padecimientos.ToList() ?? new List<Padecimiento>(),
                    usuario = usuario,
                    personaFisica = personaFamiliar,
                    documentacion = personaFamiliar.Documentacions.ToList(),
                    adulto = listaAdulto
                };

                return Ok(modelOut);
            }

            return Ok(new { admin = "usuario admin" });
        }

        [HttpGet("verCliente/{idCliente}")]
        public async Task<IActionResult> verCliente(int idCliente)
        {
            var usuario = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.IdUsuario == idCliente && u.TipoUsuarioid == 2);
            if (usuario == null)
            {
                var res = new
                {
                    error = "no se encontro el usuario"
                };
                return BadRequest(res);
            }

            var persona = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == idCliente).ToListAsync();
            var tipUsuario = await _baseDatos.TipoUsuarios.SingleOrDefaultAsync(t => t.IdTipousuario == usuario.TipoUsuarioid);
            var nivelUsuario = await _baseDatos.NivelUsuarios.SingleOrDefaultAsync(n => n.IdNivelusuario == usuario.UsuarionivelId);
            var listPersona = new List<PersonaFisica>();
            foreach (var p in persona)
            {
                var padecimientos = await _baseDatos.Padecimientos.Where(ped => ped.DatosmedicosId == p.DatosMedicosid).ToListAsync();
                var modelDatosMedicos = new DatosMedico
                {
                    Padecimientos = padecimientos,
                };
                var person = new PersonaFisica
                {
                    IdPersona = p.IdPersona,
                    Nombre = p.Nombre,
                    ApellidoPaterno = p.ApellidoPaterno,
                    ApellidoMaterno = p.ApellidoMaterno,
                    CorreoElectronico = p.CorreoElectronico,
                    FechaNacimiento = p.FechaNacimiento,
                    Genero = p.Genero,
                    EstadoCivil = p.EstadoCivil,
                    Domicilio = p.Domicilio,
                    AvatarImage = p.AvatarImage,
                    DatosMedicos = modelDatosMedicos
                };

                listPersona.Add(person);
            }

            var salario = await _baseDatos.SalarioCuidadors.SingleOrDefaultAsync(s => s.Usuarioid == usuario.IdUsuario);
            
            var outModel = new OUTVerCliente
            {
                idUsuario = usuario.IdUsuario,
                usuario = usuario.Usuario1,
                nivel_usuario = nivelUsuario.NombreNivel,
                tipo_usuario = tipUsuario.NombreTipo,
                personaFisica = listPersona
            };

            return Ok(outModel);
        }

        /* CUIDADOR */
        [HttpGet]
        [Route("verUsuarioWeb/{idPersona}")]
        public async Task<IActionResult> verUsuario(int idPersona)
        {
            var persona = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == idPersona).ToListAsync();
            if (persona == null)
            {
                var res = new
                {
                    error = "no se encontro el usuario"
                };
                return BadRequest(res);
            }            

            var listaComen = new List<ComentariosUsuario>();
            var listaDomicilio = new List<Domicilio>();
            var contratosRealizados = new List<Contrato>();
            var us = new Usuario();
            var nivelUsuario = new NivelUsuario();
            var tipUsuario = new TipoUsuario();
            foreach (var p in persona)
            {
                us = await _baseDatos.Usuarios.SingleOrDefaultAsync(u => u.IdUsuario == p.UsuarioId && u.TipoUsuarioid == 1);
                if (us == null)
                {
                    return BadRequest(new { error = "usuario no encontrado" });
                }
                tipUsuario = await _baseDatos.TipoUsuarios.SingleOrDefaultAsync(t => t.IdTipousuario == us.TipoUsuarioid);
                nivelUsuario = await _baseDatos.NivelUsuarios.SingleOrDefaultAsync(n => n.IdNivelusuario == us.UsuarionivelId);
                var certificaciones = await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == p.IdPersona).ToListAsync();
                var comen = await _baseDatos.ComentariosUsuarios.SingleOrDefaultAsync(c => c.PersonaReceptor.IdPersona == p.IdPersona);
                var domicilio = await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == p.DomicilioId);
                listaComen.Add(comen);
                listaDomicilio.Add(domicilio);
                var contratos = await _baseDatos.Contratos.Where(c => c.PersonaidCuidador == p.IdPersona && c.EstatusId == 9).ToListAsync();
                contratosRealizados.AddRange(contratos);
            }

            var salario = await _baseDatos.SalarioCuidadors.SingleOrDefaultAsync(s => s.Usuarioid == us.IdUsuario);

            var modelOutVerUsuario = new OUTVerUsuario();

            if (salario == null)
            {
                salario = null;
            }
            else
            {
                modelOutVerUsuario = new OUTVerUsuario
                {
                    id_usuario = us.IdUsuario,
                    usuario = us.Usuario1,
                    nivelUsuario = nivelUsuario.NombreNivel,
                    tipo_usuario = tipUsuario.NombreTipo,
                    personaFisica = persona,
                    comentarios_usuario = listaComen,
                    domicilio = listaDomicilio,
                    salario_cuidador = salario.PrecioPorHora,
                    cuidados_realizados = contratosRealizados.Count()
                };

                return Ok(modelOutVerUsuario);
            }

            modelOutVerUsuario = new OUTVerUsuario
            {
                id_usuario = us.IdUsuario,
                usuario = us.Usuario1,
                nivelUsuario = nivelUsuario.NombreNivel,
                tipo_usuario = tipUsuario.NombreTipo,
                personaFisica = pers,
                comentarios_usuario = listaComen,
                domicilio = listaDomicilio
            };

            return Ok(modelOutVerUsuario);            
        }

        [HttpGet]
        [Route("listarDatosUsuarioWeb/{idUser}")]
        public async Task<IActionResult> listarDatos(int idUser)
        {
            /* 
             * necesita validaciones para nulos, mas de un objeto en sentencias single y no encontrados
             */

            var personas = await _baseDatos.PersonaFisicas.ToListAsync();
            var persona = personas.SingleOrDefault(p => p.UsuarioId == idUser);
            var domicilio = await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == persona.DomicilioId);            
            var datosMedicos = await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == persona.DatosMedicosid);
            var padecimientos = datosMedicos != null ? await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == datosMedicos.IdDatosmedicos).ToListAsync(): null;
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

        [HttpGet("verCuidadores")]
        public async Task<IActionResult> verCuidadores()
        {
            var outLista = new List<OUTVerCuidador>();
            var usuarios = await _baseDatos.Usuarios.Where(u => u.TipoUsuarioid == 1).ToListAsync();
            try
            {
                foreach (var us in usuarios)
                {
                    var nivelUsuario = await _baseDatos.NivelUsuarios.SingleOrDefaultAsync(n => n.IdNivelusuario == us.UsuarionivelId);
                    var personas = await _baseDatos.PersonaFisicas.Where(p => p.UsuarioId == us.IdUsuario).ToListAsync();
                    if (personas == null || personas.Count == 0)
                    {
                        personas = null;
                    }
                    var comentarios = personas != null ? await _baseDatos.ComentariosUsuarios.Where(c => c.PersonaReceptorid == personas.First().IdPersona).ToListAsync(): null;
                    var cer = personas != null ? await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == personas.First().IdPersona).ToListAsync() : null;
                    var certificacionesExperiencia = new List<CertificacionesExperiencium>();
                    

                    if (cer != null)
                    {
                        foreach (var cert in cer)
                        {
                            var certEXp = new CertificacionesExperiencium
                            {
                               IdCertificacion = cert.IdCertificacion,
                               TipoCertificacion = cert.TipoCertificacion,
                               InstitucionEmisora = cert.InstitucionEmisora,
                               Descripcion = cert.Descripcion
                            };

                            certificacionesExperiencia.Add(certEXp);
                        }
                    }

                    var listaPersonas = new List<OUTPersonaFisicaDTO>();
                    var contratosRealizados = new List<Contrato>();
                    if (personas != null)
                    {
                        foreach (var p in personas)
                        {
                            DateTime fechaActual = DateTime.Today;
                            int edad = fechaActual.Year - p.FechaNacimiento.Year;

                            var aux = new OUTPersonaFisicaDTO
                            {
                                idPersona = p.IdPersona,
                                nombre = p.Nombre,
                                apellidoPaterno = p.ApellidoPaterno,
                                apellidoMaterno = p.ApellidoMaterno,
                                edad = edad,
                                avatarImage = p.AvatarImage
                            };

                            var contratos = await _baseDatos.Contratos.Where(c => c.PersonaidCuidador == p.IdPersona && c.EstatusId == 9).ToListAsync();
                            contratosRealizados.AddRange(contratos);

                            listaPersonas.Add(aux);
                        }
                    }
                    
                    var salario = await _baseDatos.SalarioCuidadors.SingleOrDefaultAsync(s => s.Usuarioid == us.IdUsuario);
                    
                    if (salario == null)
                    {
                        salario = null;
                    }
                    else
                    {
                        var datosOut = new OUTVerCuidador
                        {
                            idUsuario = us.IdUsuario,
                            usuario = us.Usuario1,
                            nivelUsuario = nivelUsuario.NombreNivel,
                            comentariosUsuario = comentarios,//lista comentarios
                            certificaciones = certificacionesExperiencia, //lista certificaciones
                            personaFisica = listaPersonas, //lista personasfisicas
                            cuidadosrealizados = contratosRealizados.Count(), //cuidados realizados
                            salarioCuidador = salario.PrecioPorHora, //salarios
                        };
                        outLista.Add(datosOut);
                    }                   
                }

                return Ok(outLista.ToList());
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException;
                return BadRequest(new { error = ex.Message, innerError = innerException?.Message });
            }
        }

        [HttpGet("verCuidadores2")]
        public async Task<IActionResult> VerCuidadores2()
        {

            var cuidadores = await _baseDatos.Usuarios
                .Where(u => u.TipoUsuarioid == 1)
                .Select(u => new
                {
                    u.IdUsuario,
                    u.UsuarionivelId,
                    u.TipoUsuarioid,
                    u.Estatusid,
                    u.Usuario1,
                    u.Contrasenia,
                    u.FechaRegistro,
                    u.UsuarioRegistro,
                    u.FechaModificacion,
                    u.UsuarioModifico,
                    personas = _baseDatos.PersonaFisicas
                        .Where(p => p.UsuarioId == u.IdUsuario)
                        .Select(p => new {
                            p.IdPersona,
                            p.UsuarioId,
                            p.Nombre,
                            p.ApellidoPaterno,
                            p.ApellidoMaterno,
                            p.CorreoElectronico,
                            p.FechaNacimiento,
                            p.Genero,
                            p.EstadoCivil,
                            p.Rfc,
                            p.Curp,
                            p.TelefonoParticular,
                            p.TelefonoMovil,
                            p.TelefonoEmergencia,
                            p.NombrecompletoFamiliar,
                            p.DomicilioId,
                            p.DatosMedicosid,
                            p.AvatarImage,
                            p.EstatusId,
                            p.FechaRegistro,
                            p.UsuarioRegistro,
                            p.FechaModificacion,
                            p.UsuarioModifico
                        }).FirstOrDefault(),
                    documentacion = _baseDatos.Documentacions
                        .Where(d => d.PersonaId == u.IdUsuario)
                        .Select(d => new {
                            d.IdDocumentacion,
                            d.PersonaId,
                            d.TipoDocumento,
                            d.NombreDocumento,
                            d.UrlDocumento,
                            d.FechaEmision,
                            d.FechaExpiracion,
                            d.Version,
                            d.EstatusId,
                            d.FechaRegistro,
                            d.UsuarioRegistro,
                            d.FechaModificacion,
                            d.UsuarioModifico
                        }).ToList(),
                    certificaciones = _baseDatos.CertificacionesExperiencia
                        .Where(c => c.PersonaId == u.IdUsuario)
                        .Select(c => new {
                            c.IdCertificacion,
                            c.TipoCertificacion,
                            c.InstitucionEmisora,
                            c.FechaCertificacion,
                            c.Vigente,
                            c.Descripcion,
                            c.FechaRegistro,
                            c.UsuarioRegistro,
                            c.FechaModificacion,
                            c.UsuarioModifico
                        }).ToList(),
                    domicilio = _baseDatos.Domicilios
                        .Where(d => d.IdDomicilio == u.IdUsuario)
                        .Select(d => new {
                            d.IdDomicilio,
                            d.Calle,
                            d.Colonia,
                            d.NumeroInterior,
                            d.NumeroExterior,
                            d.Ciudad,
                            d.Estado,
                            d.Pais,
                            d.Referencias,
                            d.EstatusId,
                            d.FechaRegistro,
                            d.UsuarioRegistro,
                            d.FechaModificacion,
                            d.UsuarioModifico
                        }).FirstOrDefault(),
                    datosMedicos = _baseDatos.DatosMedicos
                        .Where(dm => dm.IdDatosmedicos == u.IdUsuario)
                        .Select(dm => new {
                            dm.IdDatosmedicos,
                            dm.AntecedentesMedicos,
                            dm.Alergias,
                            dm.TipoSanguineo,
                            dm.NombreMedicofamiliar,
                            dm.TelefonoMedicofamiliar,
                            dm.Observaciones,
                            dm.FechaRegistro,
                            dm.UsuarioRegistro,
                            dm.FechaModificacion,
                            dm.UsuarioModifico
                        }).ToList(),
                    padecimientos = _baseDatos.Padecimientos
                        .Where(p => p.DatosmedicosId == u.IdUsuario)
                        .Select(p => new {
                            p.IdPadecimiento,
                            p.DatosmedicosId,
                            p.Nombre,
                            p.Descripcion,
                            p.PadeceDesde,
                            p.FechaRegistro,
                            p.UsuarioRegistro,
                            p.FechaModificacion,
                            p.UsuarioModifico
                        }).ToList()
                })
                .ToListAsync<dynamic>();

            var jsonResult = JsonSerializer.Serialize(cuidadores);
            return Ok(jsonResult);
        }

        // api igual que la de arriba
        [HttpGet("mostrarDatosCuidadoresWeb")]
        public async Task<IActionResult> mostrarCuidadores()
        {
            var outLista = new List<OUTDatosCuidador>();
            var usuarios = await _baseDatos.Usuarios.Where(u => u.TipoUsuarioid == 1).ToListAsync();
            
            try
            {
                foreach (var us in usuarios)
                {
                    var usuarioOUT = new OUTUsuarioDTO
                    {
                        id_usuario = us.IdUsuario,
                        usuarionivel_id = us.UsuarionivelId,
                        tipo_usuarioid = us.TipoUsuarioid,
                        estatusid = us.Estatusid,
                        usuario = us.Usuario1,
                        contrasenia = us.Contrasenia,
                        fecha_registro = us.FechaRegistro,
                        usuario_registro = us.UsuarioRegistro,
                        fecha_modificacion = us.FechaModificacion
                    };
                    var personas = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.UsuarioId == us.IdUsuario);
                    var personaOut = new OUTPersonaFisicaWebDTO();
                    if (personas != null)
                    {
                        personaOut = new OUTPersonaFisicaWebDTO
                        {
                            id_persona = personas.IdPersona,
                            usuario_id = personas.UsuarioId,
                            nombre = personas.Nombre,
                            apellido_paterno = personas.ApellidoPaterno,
                            apellido_materno = personas.ApellidoMaterno,
                            correo_electronico = personas.CorreoElectronico,
                            fecha_nacimiento = personas.FechaNacimiento,
                            genero = personas.Genero,
                            estado_Civil = personas.EstadoCivil,
                            rfc = personas.Rfc,
                            curp = personas.Curp,
                            telefono_particular = personas.TelefonoParticular,
                            telefono_movil = personas.TelefonoParticular,
                            telefono_emergencia = personas.TelefonoEmergencia,
                            nombrecompleto_familiar = personas.NombrecompletoFamiliar,
                            domicilio_id = personas.DomicilioId,
                            datos_medicosid = personas.DatosMedicosid,
                            avatar_image = personas.AvatarImage,
                            estatus_id = personas.EstatusId,
                            fecha_registro = personas.FechaRegistro,
                            usuario_registro = personas.UsuarioRegistro,
                            fecha_modificacion = personas.FechaModificacion,
                            usuario_modifico = personas.UsuarioModifico
                        };
                    }
                    var domicilioModel = personas != null ? await _baseDatos.Domicilios.SingleOrDefaultAsync(d => d.IdDomicilio == personas.DomicilioId): null;
                    var domicilioOut = new OUTDomicilioDTO();
                    if (domicilioModel != null)
                    {
                        domicilioOut = new OUTDomicilioDTO
                        {
                            id_domicilio = domicilioModel.IdDomicilio,
                            calle = domicilioModel.Calle,
                            colonia = domicilioModel.Colonia,
                            numero_interior = domicilioModel.NumeroInterior,
                            numero_exterior = domicilioModel.NumeroExterior,
                            ciudad = domicilioModel.Ciudad,
                            estado = domicilioModel.Estado,
                            pais = domicilioModel.Pais,
                            referencias = domicilioModel.Referencias,
                            estatus_id = domicilioModel.EstatusId,
                            fecha_registro = domicilioModel?.FechaRegistro,
                            usuario_registro = domicilioModel.UsuarioRegistro,
                            fecha_modificacion = domicilioModel.FechaModificacion
                        };
                    }

                    var datosMedicosModel = personas != null ? await _baseDatos.DatosMedicos.SingleOrDefaultAsync(dm => dm.IdDatosmedicos == personas.DatosMedicosid): null;
                    var datosMedicosOut = new OUTDatosMedicosDTO();
                    if (datosMedicosModel != null)
                    {
                        datosMedicosOut = new OUTDatosMedicosDTO
                        {
                            id_datosmedicos = datosMedicosModel.IdDatosmedicos,
                            antecedentes_medicos = datosMedicosModel.AntecedentesMedicos,
                            alergias = datosMedicosModel.Alergias,
                            tipo_sanguineo = datosMedicosModel.TipoSanguineo,
                            nombre_medicofamiliar = datosMedicosModel.NombreMedicofamiliar,
                            telefono_medicofamiliar = datosMedicosModel.TelefonoMedicofamiliar,
                            observaciones = datosMedicosModel?.Observaciones,
                            fecha_registro = datosMedicosModel.FechaRegistro
                        };
                    }
                    var padecimientos = datosMedicosModel != null ? await _baseDatos.Padecimientos.Where(p => p.DatosmedicosId == datosMedicosModel.IdDatosmedicos).ToListAsync(): null;
                    var listaPadecimientosOut = new List<OUTPadecimientosDTO>();
                    if(padecimientos != null)
                    {
                        foreach (var p in padecimientos)
                        {
                            var padecimientosOut = new OUTPadecimientosDTO
                            {
                                id_padecimiento = p.IdPadecimiento,
                                datosmedicos_id = p.DatosmedicosId,
                                nombre = p.Nombre,
                                descripcion = p.Descripcion,
                                padeceDesde = p.PadeceDesde,
                                fecha_registro = p.FechaRegistro,
                                usuario_registro = p.UsuarioRegistro,
                                fecha_modificacion = p.FechaModificacion,
                                usuario_modifico = p.UsuarioModifico
                            };

                            listaPadecimientosOut.Add(padecimientosOut);
                        }
                    }
                    var documentacion = personas != null ? await _baseDatos.Documentacions.Where(d => d.PersonaId == personas.IdPersona).ToListAsync(): null;
                    var listaDocumentacionOut = new List<OUTDocumentacionDTO>();
                    if (documentacion != null)
                    {
                        foreach (var p in documentacion)
                        {
                            var docuOUT = new OUTDocumentacionDTO
                            {
                                id_documentacion = p.IdDocumentacion,
                                persona_id = p.PersonaId,
                                tipo_documento = p.TipoDocumento,
                                nombre_documento = p.NombreDocumento,
                                url_documento = p.UrlDocumento,
                                fecha_emision = p.FechaEmision,
                                fecha_expiracion  = p.FechaExpiracion,
                                version = p.Version,
                                estatus_id = p.EstatusId,
                                fecha_registro = p.FechaRegistro,
                                usuario_registro = p.UsuarioRegistro,
                                fecha_modificacion = p.FechaModificacion,
                                usuario_modifico = p.UsuarioModifico
                            };

                            listaDocumentacionOut.Add(docuOUT);
                        }
                    }
                    var cer = personas != null ? await _baseDatos.CertificacionesExperiencia.Where(c => c.PersonaId == personas.IdPersona).ToListAsync() : null;
                
                    var certificacionesExperiencia = new List<OUTCertificacionYDocumentoOUT>();
                    if (cer != null)
                    {
                        foreach (var cert in cer)
                        {
                            var certificadoAux = await _baseDatos.CertificacionesExperiencia.SingleOrDefaultAsync(c => c.IdCertificacion == cert.IdCertificacion);
                            var certificacionOut = new OUTCertificacionExpDTO
                            {
                                id_certificacion = cert.IdCertificacion,
                                tipo_certificacion = cert.TipoCertificacion.ToString(),
                                institucion_emisora = cert.InstitucionEmisora,
                                fecha_certificacion = cert.FechaCertificacion,
                                vigente = cert.Vigente,
                                descripcion = cert.Descripcion,
                                fecha_registro = cert.FechaRegistro,
                                usuario_registro = cert.UsuarioRegistro,
                                fecha_modificacion = cert.FechaModificacion,
                                usuario_modifico = cert.UsuarioModifico
                            };

                            var docs = await _baseDatos.Documentacions.SingleOrDefaultAsync(d => d.IdDocumentacion == cert.DocumentoId);
                            var docsOut = new OUTDocumentacionDTO
                            {
                                id_documentacion = docs.IdDocumentacion,
                                persona_id = docs.PersonaId,
                                tipo_documento = docs.TipoDocumento,
                                nombre_documento = docs.NombreDocumento,
                                url_documento = docs.UrlDocumento,
                                fecha_emision = docs.FechaEmision,
                                fecha_expiracion = docs.FechaExpiracion,
                                version = docs.Version,
                                estatus_id = docs.EstatusId,
                                fecha_registro = docs.FechaRegistro,
                                usuario_registro = docs.UsuarioRegistro,
                                fecha_modificacion = docs.FechaModificacion,
                                usuario_modifico = docs.UsuarioModifico
                            };

                            var certEXp = new OUTCertificacionYDocumentoOUT
                            {
                                certificacionExpDTO = certificacionOut,
                                documentacionDTO = docsOut
                            };

                            certificacionesExperiencia.Add(certEXp);
                        }
                    }

                    var datosOut = new OUTDatosCuidador
                    {
                        Domicilio = domicilioOut,
                        DatosMedicos = datosMedicosOut,
                        Padecimientos = listaPadecimientosOut,
                        Usuario = usuarioOUT,
                        Persona = personaOut,
                        Documentacion = listaDocumentacionOut,
                        CertificacionesExperiencia = certificacionesExperiencia
                    };

                    outLista.Add(datosOut);
                }

                return Ok(outLista.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }           
        }

        

        [HttpPost("registrarFamiliarWeb")]
        public async Task<IActionResult> RegistrarFamiliar([FromBody] RegistrarFamiliar usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    var domicilio = new Domicilio
                    {
                        Calle = usuarioDTO.domicilio.Calle,
                        Colonia = usuarioDTO.domicilio.Colonia,
                        NumeroInterior = usuarioDTO.domicilio.NumeroInterior,
                        NumeroExterior = usuarioDTO.domicilio.NumeroExterior,
                        Ciudad = usuarioDTO.domicilio.Ciudad,
                        Estado = usuarioDTO.domicilio.Estado,
                        Pais = usuarioDTO.domicilio.Pais,
                        Referencias = usuarioDTO.domicilio.Referencias,
                        EstatusId = usuarioDTO.domicilio.EstatusId,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro
                    };

                    _baseDatos.Domicilios.Add(domicilio);
                    await _baseDatos.SaveChangesAsync();

                    var datosMedicos = new DatosMedico
                    {
                        AntecedentesMedicos = usuarioDTO.datos_medicos.AntecedentesMedicos,
                        Alergias = usuarioDTO.datos_medicos.Alergias,
                        TipoSanguineo = usuarioDTO.datos_medicos.TipoSanguineo,
                        NombreMedicofamiliar = usuarioDTO.datos_medicos.NombreMedicoFamiliar,
                        TelefonoMedicofamiliar = usuarioDTO.datos_medicos.TelefonoMedicoFamiliar,
                        Observaciones = usuarioDTO.datos_medicos.Observaciones,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                    };

                    _baseDatos.DatosMedicos.Add(datosMedicos);
                    await _baseDatos.SaveChangesAsync();

                    var padecimientos = new List<Padecimiento>();

                    foreach (var padecimientoDTO in usuarioDTO.padecimientos)
                    {
                        var padecimiento = new Padecimiento
                        {
                            DatosmedicosId = datosMedicos.IdDatosmedicos,
                            Nombre = padecimientoDTO.Nombre,
                            Descripcion = padecimientoDTO.Descripcion,
                            PadeceDesde = padecimientoDTO.PadeceDesde,
                            FechaRegistro = DateTime.Now,
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro
                        };

                        padecimientos.Add(padecimiento);
                    }

                    _baseDatos.Padecimientos.AddRange(padecimientos);
                    await _baseDatos.SaveChangesAsync();

                    bool usuarioExiste = await _baseDatos.Usuarios.AnyAsync(u => u.Usuario1 == usuarioDTO.usuario.Usuario);
                    var usuario = new Usuario();

                    if (usuarioExiste)
                    {
                        return BadRequest(new { error = "El nombre de usuario ya existe" });
                    }
                    else
                    {
                        usuario = new Usuario
                        {
                            UsuarionivelId = 6,
                            TipoUsuarioid = usuarioDTO.usuario.TipoUsuarioId,
                            Estatusid = usuarioDTO.usuario.EstatusId,
                            Usuario1 = usuarioDTO.usuario.Usuario,
                            Contrasenia = usuarioDTO.usuario.Contrasenia,
                            FechaRegistro = DateTime.Now,
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                        };

                        _baseDatos.Usuarios.Add(usuario);
                        await _baseDatos.SaveChangesAsync();
                    }                   


                    var persona = new PersonaFisica
                    {
                        Nombre = usuarioDTO.persona.Nombre,
                        ApellidoPaterno = usuarioDTO.persona.ApellidoPaterno,
                        ApellidoMaterno = usuarioDTO.persona.ApellidoMaterno,
                        CorreoElectronico = usuarioDTO.persona.CorreoElectronico,
                        FechaNacimiento = usuarioDTO.persona.FechaNacimiento,
                        Genero = usuarioDTO.persona.Genero,
                        EstadoCivil = usuarioDTO.persona.EstadoCivil,
                        Rfc = usuarioDTO.persona.RFC,
                        Curp = usuarioDTO.persona.CURP,
                        TelefonoParticular = usuarioDTO.persona.TelefonoParticular,
                        TelefonoMovil = usuarioDTO.persona.TelefonoMovil,
                        TelefonoEmergencia = usuarioDTO.persona.TelefonoEmergencia,
                        NombrecompletoFamiliar = usuarioDTO.persona.NombreCompletoFamiliar,
                        DomicilioId = domicilio.IdDomicilio, // Recuperado después de insertar domicilio
                        DatosMedicosid = datosMedicos.IdDatosmedicos, // Recuperado después de insertar datos médicos
                        AvatarImage = usuarioDTO.persona.AvatarImage,
                        EstatusId = usuarioDTO.persona.EstatusId,
                        FechaRegistro = DateTime.Now, // Se coloca en el controlador
                        UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro, /*PUEDE TENER DETALLE*/
                        UsuarioId = usuario.IdUsuario
                    };

                    _baseDatos.PersonaFisicas.Add(persona);
                    await _baseDatos.SaveChangesAsync();

                    var documentaciones = new List<Documentacion>();

                    foreach (var listaDocumentacion in usuarioDTO.documentacion)
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
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                        };

                        documentaciones.Add(documentacion);
                    }

                    _baseDatos.Documentacions.AddRange(documentaciones);
                    await _baseDatos.SaveChangesAsync();

                    // Si todas las operaciones son exitosas, se confirman los cambios
                    await transaction.CommitAsync();

                    return Ok(new { res = "Se regstro exitosamente" });
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, se revierte toda la transacción
                    await transaction.RollbackAsync();
                    var innerException = ex.InnerException;
                    return BadRequest(new { error = ex.Message, innerError = innerException?.Message }
);
                }
            }
        }

        [HttpGet("dashboardCuidador/{usuarioid}")]
        public async Task<IActionResult> DashboardCuidador(int usuarioid)
        {
            DashboardDTO dashboard = new DashboardDTO();
            try
            {
                PersonaFisica personaCuidador = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.UsuarioId == usuarioid) ?? new PersonaFisica();
                List<Contrato> contratos = await _baseDatos.Contratos.Where(c => c.PersonaidCuidador == personaCuidador.IdPersona).ToListAsync();
                List<ContratoItem> contratoItems = new List<ContratoItem>();

                foreach (var contrato in contratos)
                {
                    contratoItems.AddRange(await _baseDatos.ContratoItems.Where(ci => ci.ContratoId == contrato.IdContrato).ToListAsync());
                }

                foreach (var item in contratoItems.Where(ci => ci.EstatusId == 7 || ci.EstatusId == 19)) // Contratos en curso o aceptados 
                {
                    PersonaFisica nombreCliente = await _baseDatos.PersonaFisicas.SingleOrDefaultAsync(p => p.IdPersona == item.Contrato.PersonaidCliente) ?? new PersonaFisica();
                    fechasConContratos fechasConContratos = new fechasConContratos
                    {
                        horarioInicioPropuesto = item.HorarioInicioPropuesto,
                        horarioFinPropuesto = item.HorarioFinPropuesto,
                        nombreCliente = nombreCliente.Nombre + " " + nombreCliente.ApellidoPaterno + " " + nombreCliente.ApellidoMaterno
                    };
                }

                var resultado = await _baseDatos.Database.SqlQueryRaw<horasPorMes>(
                    "SELECT [dbo].[month_name](fecha_inicio_cuidado, 'en-ES') as mes, ISNULL(SUM(DATEDIFF(HOUR, fecha_inicio_cuidado, fecha_fin_cuidado)), 0) as horas " +
                    "FROM contrato " +
                    "INNER JOIN contrato_item on id_contrato = contrato_id " +
                    "WHERE personaid_cuidador = {0} GROUP BY fecha_inicio_cuidado", usuarioid)
                    .FirstOrDefaultAsync();


                dashboard.horasPorMes = resultado ?? new horasPorMes();

                dashboard.contratoEnCurso = contratoItems.FirstOrDefault(ci => ci.EstatusId == 19) ?? new ContratoItem();

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            return Ok(dashboard);
        }

        [HttpPost("registrarUsuarioWeb")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistrarUsuarioDTO usuarioDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var transaction = await _baseDatos.Database.BeginTransactionAsync())
            {
                try
                {
                    var domicilio = new Domicilio
                    {
                        Calle = usuarioDTO.domicilio.Calle,
                        Colonia = usuarioDTO.domicilio.Colonia,
                        NumeroInterior = usuarioDTO.domicilio.NumeroInterior,
                        NumeroExterior = usuarioDTO.domicilio.NumeroExterior,
                        Ciudad = usuarioDTO.domicilio.Ciudad,
                        Estado = usuarioDTO.domicilio.Estado,
                        Pais = usuarioDTO.domicilio.Pais,
                        Referencias = usuarioDTO.domicilio.Referencias,
                        EstatusId = usuarioDTO.domicilio.EstatusId,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro
                    };

                    _baseDatos.Domicilios.Add(domicilio);
                    await _baseDatos.SaveChangesAsync();

                    var datosMedicos = new DatosMedico
                    {
                        AntecedentesMedicos = usuarioDTO.datos_medicos.AntecedentesMedicos,
                        Alergias = usuarioDTO.datos_medicos.Alergias,
                        TipoSanguineo = usuarioDTO.datos_medicos.TipoSanguineo,
                        NombreMedicofamiliar = usuarioDTO.datos_medicos.NombreMedicoFamiliar,
                        TelefonoMedicofamiliar = usuarioDTO.datos_medicos.TelefonoMedicoFamiliar,
                        Observaciones = usuarioDTO.datos_medicos.Observaciones,
                        FechaRegistro = DateTime.Now,
                        UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                    };

                    _baseDatos.DatosMedicos.Add(datosMedicos);
                    await _baseDatos.SaveChangesAsync();

                    var padecimientos = new List<Padecimiento>();

                    foreach (var padecimientoDTO in usuarioDTO.padecimientos)
                    {
                        var padecimiento = new Padecimiento
                        {
                            DatosmedicosId = datosMedicos.IdDatosmedicos,
                            Nombre = padecimientoDTO.Nombre,
                            Descripcion = padecimientoDTO.Descripcion,
                            PadeceDesde = padecimientoDTO.PadeceDesde,
                            FechaRegistro = DateTime.Now,
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro
                        };

                        padecimientos.Add(padecimiento);
                    }

                    _baseDatos.Padecimientos.AddRange(padecimientos);
                    await _baseDatos.SaveChangesAsync();

                    bool usuarioExiste = await _baseDatos.Usuarios.AnyAsync(u => u.Usuario1 == usuarioDTO.usuario.Usuario);
                    var usuario = new Usuario();

                    if (usuarioExiste)
                    {
                        return BadRequest(new { error = "El nombre de usuario ya existe" });
                    }
                    else
                    {
                        usuario = new Usuario
                        {
                            UsuarionivelId = 6,
                            TipoUsuarioid = usuarioDTO.usuario.TipoUsuarioId,
                            Estatusid = usuarioDTO.usuario.EstatusId,
                            Usuario1 = usuarioDTO.usuario.Usuario,
                            Contrasenia = usuarioDTO.usuario.Contrasenia,
                            FechaRegistro = DateTime.Now,
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                        };

                        _baseDatos.Usuarios.Add(usuario);
                        await _baseDatos.SaveChangesAsync();
                    }

                    var persona = new PersonaFisica
                    {
                        Nombre = usuarioDTO.persona.Nombre,
                        ApellidoPaterno = usuarioDTO.persona.ApellidoPaterno,
                        ApellidoMaterno = usuarioDTO.persona.ApellidoMaterno,
                        CorreoElectronico = usuarioDTO.persona.CorreoElectronico,
                        FechaNacimiento = usuarioDTO.persona.FechaNacimiento,
                        Genero = usuarioDTO.persona.Genero,
                        EstadoCivil = usuarioDTO.persona.EstadoCivil,
                        Rfc = usuarioDTO.persona.RFC,
                        Curp = usuarioDTO.persona.CURP,
                        TelefonoParticular = usuarioDTO.persona.TelefonoParticular,
                        TelefonoMovil = usuarioDTO.persona.TelefonoMovil,
                        TelefonoEmergencia = usuarioDTO.persona.TelefonoEmergencia,
                        NombrecompletoFamiliar = usuarioDTO.persona.NombreCompletoFamiliar,
                        DomicilioId = domicilio.IdDomicilio, // Recuperado después de insertar domicilio
                        DatosMedicosid = datosMedicos.IdDatosmedicos, // Recuperado después de insertar datos médicos
                        AvatarImage = usuarioDTO.persona.AvatarImage,
                        EstatusId = usuarioDTO.persona.EstatusId,
                        FechaRegistro = DateTime.Now, // Se coloca en el controlador
                        UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro, /*PUEDE TENER DETALLE*/
                        UsuarioId = usuario.IdUsuario
                    };

                    _baseDatos.PersonaFisicas.Add(persona);
                    await _baseDatos.SaveChangesAsync();

                    var documentaciones = new List<Documentacion>();

                    foreach (var listaDocumentacion in usuarioDTO.documentacion)
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
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/
                        };

                        documentaciones.Add(documentacion);
                    }

                    _baseDatos.Documentacions.AddRange(documentaciones);
                    await _baseDatos.SaveChangesAsync();

                    foreach (var certificacionExp in usuarioDTO.CertificacionesExperiencia.certificaciones)
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
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro /*PUEDE TENER DETALLE*/

                        };

                        _baseDatos.Documentacions.Add(documento);
                        await _baseDatos.SaveChangesAsync();

                        var certificacionExperiencia = new CertificacionesExperiencium
                        {
                            TipoCertificacion = certificacionExp.Certificacion.TipoCertificacion,
                            InstitucionEmisora = certificacionExp.Certificacion.InstitucionEmisora,
                            FechaCertificacion = certificacionExp.Certificacion.FechaCertificacion,
                            Vigente = certificacionExp.Certificacion.Vigente,
                            Descripcion = certificacionExp.Certificacion.Descripcion,
                            FechaRegistro = DateTime.Now, // Se coloca en el controlador
                            UsuarioRegistro = usuarioDTO.domicilio.UsuarioRegistro, /*PUEDE TENER DETALLE*/
                            PersonaId = persona.IdPersona,
                            DocumentoId = documento.IdDocumentacion
                        };

                        _baseDatos.CertificacionesExperiencia.Add(certificacionExperiencia);
                        await _baseDatos.SaveChangesAsync();

                    }

                    // Si todas las operaciones son exitosas, se confirman los cambios
                    await transaction.CommitAsync();

                    return Ok(new { res = "Se regstro exitosamente" });
                }
                catch (Exception ex)
                {
                    // Si ocurre un error, se revierte toda la transacción
                    await transaction.RollbackAsync();

                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
