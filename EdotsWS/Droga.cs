using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
    public class Droga
    {
        public int CodigoDroga { get; set; }
        public string Farmacos { get; set; }
        public string Siglas { get; set; }
        public string DosisMaxima { get; set; }

        public Droga()
        {
            this.CodigoDroga = 0;
            this.Farmacos = "";
            this.Siglas = "";
            this.DosisMaxima = "";
        }

        public Droga(int CodigoDroga, string Farmacos, string Siglas, string DosisMaxima)
        {
            this.CodigoDroga = CodigoDroga;
            this.Farmacos = Farmacos;
            this.Siglas = Siglas;
            this.DosisMaxima = DosisMaxima;
        }
    }
}