using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
    public class Inscripcion
    {
        public string CodigoPaciente { get; set; }
        public string Lunes { get; set; }
        public string Martes { get; set; }
        public string Miercoles { get; set; }
        public string Jueves { get; set; }
        public string Viernes { get; set; }
        public string Sabado { get; set; }
        public string Domingo { get; set; }
        public string FechaComienzo{ get; set; }
        public string FechaTermino { get; set; }
        public string Activo { get; set; }

        public Inscripcion()
        {
            this.CodigoPaciente = "";
            this.Lunes = "";
            this.Martes = "";
            this.Miercoles = "";
            this.Jueves = "";
            this.Viernes = "";
            this.Sabado = "";
            this.Domingo = "";
            this.FechaComienzo = "";
            this.FechaTermino = "";
            this.Activo = "";
        }

        public Inscripcion(string CodigoPaciente, string Lunes, string Martes,
            string Miercoles, string Jueves, string Viernes, string Sabado, 
            string Domingo,String FechaComienzo,String FechaTermino,String Activo)
        {
            this.CodigoPaciente = CodigoPaciente;
            this.Lunes = Lunes;
            this.Martes = Martes;
            this.Miercoles = Miercoles;
            this.Jueves = Jueves;
            this.Viernes = Viernes;
            this.Sabado = Sabado;
            this.Domingo = Domingo;
            this.FechaComienzo = FechaComienzo;
            this.FechaTermino = FechaTermino;
            this.Activo = Activo;
        }
    }

}