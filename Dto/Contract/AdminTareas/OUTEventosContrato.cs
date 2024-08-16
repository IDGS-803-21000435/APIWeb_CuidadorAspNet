using Cuidador.Models;

namespace Cuidador.Dto.Contract.AdminTareas
{
    public class OUTEventosContrato
    {

        public int id { get; set; }
        public bool esTarea { get; set; }   
        public string titulo { get; set; }
        public DateTime? fecha { get; set; }
        public string estatus { get; set; }

    }
}