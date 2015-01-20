using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
    public class PacCelular
    {
        public string CodigoPaciente { get; set; }
        public string Celular { get; set; }

        public PacCelular()
        {
            this.CodigoPaciente = "";
            this.Celular = "";
        }

        public PacCelular(string CodigoPaciente, string Celular)
        {
            this.CodigoPaciente = CodigoPaciente;
            this.Celular = Celular;
        }
    }
}