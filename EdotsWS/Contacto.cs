using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
    public class Contacto
    {
        public string CodigoPaciente { get; set; }
        public string CodigoUbigeo { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public string Telefono1 { get; set; }
        public string Telefono2 { get; set; }
        public string Celular { get; set; }

        public Contacto()
        {
            this.CodigoPaciente = "";
            this.CodigoUbigeo = "";
            this.Direccion = "";
            this.Referencia = "";
            this.Telefono1 = "";
            this.Telefono2 = "";
            this.Celular = "";

        }

        public Contacto(string CodigoPaciente, string CodigoUbigeo, string Direccion,
            string Referencia, string Telefono1, string Telefono2, string Celular)
        {
            this.CodigoPaciente = CodigoPaciente;
            this.CodigoUbigeo = CodigoUbigeo;
            this.Direccion = Direccion;
            this.Referencia = Referencia;
            this.Telefono1 = Telefono1;
            this.Telefono2 = Telefono2;
            this.Celular = Celular;

        }
    }
}