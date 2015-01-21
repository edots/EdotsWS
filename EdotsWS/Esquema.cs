using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EdotsWS
{
public class Esquema
    {
        public string CodigoPaciente { get; set; }
        public string CodigoEsquema { get; set; }                               
        public string LunesManana { get; set; }
        public string MartesManana { get; set; }
        public string MiercolesManana { get; set; }
        public string JuevesManana { get; set; }
        public string ViernesManana { get; set; }
        public string SabadoManana { get; set; }
        public string DomingoManana { get; set; }
        public string LunesTarde { get; set; }
        public string MartesTarde { get; set; }
        public string MiercolesTarde { get; set; }
        public string JuevesTarde { get; set; }
        public string ViernesTarde { get; set; }
        public string SabadoTarde { get; set; }
        public string DomingoTarde { get; set; }
        public string FechaComienzo{ get; set; }
        public string FechaTermino { get; set; }
        public string Activo { get; set; }
        public string TipoDeVisita { get; set; }
        public string EsquemaNombre { get; set; }
        public string EsquemaFase { get; set; }
                
                

        public Esquema()
        {
            this.CodigoPaciente = "";
            this.CodigoEsquema = "";                
            this.LunesManana = "";
            this.MartesManana = "";
            this.MiercolesManana = "";
            this.JuevesManana = "";
            this.ViernesManana = "";
            this.SabadoManana = "";
            this.DomingoManana = "";
            this.LunesTarde = "";
            this.MartesTarde = "";
            this.MiercolesTarde = "";
            this.JuevesTarde = "";
            this.ViernesTarde = "";
            this.SabadoTarde = "";
            this.DomingoTarde = "";
            this.FechaComienzo= "";
            this.FechaTermino = "";
            this.Activo = "";
            this.TipoDeVisita = "";
            this.EsquemaNombre = "";
            this.EsquemaFase = "";
        }

        public Esquema(string CodigoPaciente , string CodigoEsquema,string LunesManana , string MartesManana , string MiercolesManana , string JuevesManana , string ViernesManana , string SabadoManana , string DomingoManana , string LunesTarde , string MartesTarde , string MiercolesTarde , string JuevesTarde , string ViernesTarde , string SabadoTarde , string DomingoTarde , string FechaComienzo, string FechaTermino , string Activo , string TipoDeVisita , string EsquemaNombre , string EsquemaFase )
        {
            this.CodigoPaciente = CodigoPaciente;
            this.CodigoEsquema =CodigoEsquema;  
            this.LunesManana = LunesManana ;
            this.MartesManana = MartesManana;
            this.MiercolesManana = MiercolesManana ;
            this.JuevesManana = JuevesManana;
            this.ViernesManana = ViernesManana ;
            this.SabadoManana = SabadoManana;
            this.DomingoManana = DomingoManana ;
            this.LunesTarde = LunesTarde;
            this.MartesTarde = MartesTarde;
            this.MiercolesTarde = MiercolesTarde;
            this.JuevesTarde = JuevesTarde;
            this.ViernesTarde = ViernesTarde;
            this.SabadoTarde = SabadoTarde;
            this.DomingoTarde = DomingoTarde;
            this.FechaComienzo= FechaComienzo;
            this.FechaTermino = FechaTermino ;
            this.Activo = Activo ;
            this.TipoDeVisita = TipoDeVisita;
            this.EsquemaNombre = EsquemaNombre;
            this.EsquemaFase = EsquemaFase;
        }
    }

}

