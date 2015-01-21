using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
    public class VisitaDia
    {
        public string Dia { get; set; }
        public string CodigoPaciente { get; set; }
        public string Manana { get; set; }
        public string Tarde { get; set; }

        public VisitaDia()
        {
            this.Dia = "";
            this.CodigoPaciente = "";
            this.Manana = "";
            this.Tarde = "";
        }

        public VisitaDia(String Dia, string CodigoPaciente, string Manana, string Tarde)
        {
            this.Dia = Dia;
            this.CodigoPaciente = CodigoPaciente;
            this.Manana = Manana;
            this.Tarde = Tarde;
        }
    }
}
