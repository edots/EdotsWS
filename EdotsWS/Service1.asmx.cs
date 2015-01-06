using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

namespace EdotsWS
{
    /// <summary>
    /// Descripción breve de ServicioClientes
    /// </summary>
    [WebService(Namespace = "http://demo.sociosensalud.org.pe/")]

    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class Service1 : System.Web.Services.WebService
    {
        //hace referencia a la clase conexion, ahi esta la cadena de conexion y nuestros metodos
        Conexion con = new Conexion();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public Geofence[] ListadoGeofences()
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT G.CodigoGeofence, G.CodigoLocal, L.Nombre, G.Latitud, G.Longitud, G.Radio, G.DuracionExpiracion, G.TipoTransicion " +
                "FROM GEOFENCE AS G INNER JOIN " +
                "LOCAL AS L ON G.CodigoLocal = L.CodigoLocal ";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Geofence> lista = new List<Geofence>();
            Geofence geo = new Geofence();
            geo.codigogeofence = 0;
            geo.codigolocal = 0;
            geo.nombre = "Seleccione Local";
            geo.latitud = "";
            geo.longitud = "";
            geo.radio = "";
            geo.duracionexpiracion = "";
            geo.tipotransicion = 0;

            lista.Add(geo);

            while (reader.Read())
            {
                lista.Add(new Geofence(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7)
                    ));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public int InsertarGeoPoint(
            string Latitud,
            string Longitud,
            string Fecha,
            string Hora,
            int CodigoUsuario,
            string CodigoDispositivo,
            int TransicionGeofence,
            int CodigoGeofence)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_GEOPOINT", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Latitud", SqlDbType.VarChar, 50)).Value = Latitud;
                cmd.Parameters.Add(new SqlParameter("@Longitud", SqlDbType.VarChar, 50)).Value = Longitud;
                cmd.Parameters.Add(new SqlParameter("@Fecha", SqlDbType.VarChar, 10)).Value = Fecha;
                cmd.Parameters.Add(new SqlParameter("@Hora", SqlDbType.VarChar, 5)).Value = Hora;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                cmd.Parameters.Add(new SqlParameter("@CodigoDispositivo", SqlDbType.VarChar, 50)).Value = CodigoDispositivo;
                cmd.Parameters.Add(new SqlParameter("@CodigoGeofence", SqlDbType.Int)).Value = CodigoGeofence;
                cmd.Parameters.Add(new SqlParameter("@TransicionGeofence", SqlDbType.Int)).Value = TransicionGeofence;
                cmd.Transaction = trx;
                intretorno = cmd.ExecuteNonQuery();
                trx.Commit();
                cn.Close();
                return intretorno;
            }
            catch (SqlException sqlException)
            {
                strRespuesta = sqlException.Message.ToString();
                cn.Close();
                return -1;
            }
            catch (Exception exception)
            {
                strRespuesta = exception.Message.ToString();
                cn.Close();
                return -1;
            }
        }



    }
}