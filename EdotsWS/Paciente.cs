using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
    public class Paciente
    {
        public string CodigoPaciente { get; set; }

        public Paciente()
        {
            this.CodigoPaciente = "";
        }

        public Paciente(string CodigoPaciente)
        {
            this.CodigoPaciente = CodigoPaciente;

        }
    }
}