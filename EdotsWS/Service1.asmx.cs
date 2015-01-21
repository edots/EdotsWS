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
        DataTable dtDatos = new DataTable();
        [WebMethod]
        public String LoginUsuario(String login, String pass, int codLocal)
        {
            string msje = "";
            //int intLocal = 2;
            con.Abrir();
            msje = con.InicioSesion(login, pass, codLocal);
            con.Cerrar();
            return msje;
        }

        [WebMethod]
        public Login[] LoginUsuario1(String login, String pass, int codLocal)
        {
            con.Abrir();
            Login[] log = con.InicioSesion1(login, pass, codLocal);
            con.Cerrar();
            return log;
        }
        [WebMethod]
        public string ExisteParticipante(string DocIdentidad)
        {
            SqlConnection cn = con.conexion();
            string existe = "0";
            cn.Open();
            string sql = "select Nombres,ApellidoPaterno,ApellidoMaterno," +
                    "CodigoTipoDocumento,DocumentoIdentidad,convert(varchar(10),FechaNacimiento,103) FechaNacimiento," +
                    "Sexo from PACIENTE WHERE DocumentoIdentidad = '" + DocIdentidad + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                existe = "1";
            }
            cn.Close();
            return existe;
        }


        [WebMethod]
        public Participante[] BuscarParticipante(string DocIdentidad)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            string sql = "select CONVERT(varchar(100), CodigoPaciente, 103) AS CodigoPaciente,Nombres,ApellidoPaterno,ApellidoMaterno," +
                    "CodigoTipoDocumento,DocumentoIdentidad,convert(varchar(10),FechaNacimiento,103) FechaNacimiento," +
                    "Sexo from PACIENTE WHERE DocumentoIdentidad = '" + DocIdentidad + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Participante> lista = new List<Participante>();

            while (reader.Read())
            {
                lista.Add(new Participante(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7)));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public string NuevoParticipanteSimple(string Nombres, string ApellidoP, string ApellidoM, int CodigoTipoDocumento, string DocumentoIdentidad, string FechaNacimiento, int Sexo)
        {
            string msje = "";

            con.Abrir();

            dtDatos = this.RegistrarPacientes(Nombres, ApellidoP, ApellidoM, CodigoTipoDocumento, DocumentoIdentidad, FechaNacimiento, Sexo);

            if (dtDatos.Rows.Count > 0)
            {
                string Resultado = dtDatos.Rows[0]["Mensaje"].ToString();
                if (Resultado == "1")
                {
                    msje = "El participante ya existe..por favor verifique bien los datos ingresados";
                }
                else
                {
                    msje = "Los datos se grabaron correctamente";
                }
            }


            return msje;
        }

        [WebMethod]
        public string NuevoParticipanteObjeto(Participante participante)
        {
            string msje = "";

            dtDatos = this.RegistrarPacientes(participante.Nombres, participante.ApellidoPaterno, participante.ApellidoMaterno, participante.CodigoTipoDocumento, participante.DocumentoIdentidad, participante.FechaNacimiento, participante.Sexo);

            if (dtDatos.Rows.Count > 0)
            {
                string Resultado = dtDatos.Rows[0]["Mensaje"].ToString();

                if (Resultado == "1")
                {
                    msje = "Los datos se grabaron correctamente";
                }
                else
                {
                    msje = "El participante ya existe..por favor verifique bien los datos ingresados";
                }
            }

            return msje;

        }

        [WebMethod]
        public Local[] ListadoLocales()
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT CodigoLocal, Nombre FROM local";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Local> lista = new List<Local>();
            Local loc = new Local();
            loc.CodigoLocal = 0;
            loc.Nombre = "Seleccione Local";
            lista.Add(loc);

            while (reader.Read())
            {
                lista.Add(new Local(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
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
        public DataTable RegistrarPacientes(string Nombres, string ApellidoP, string ApellidoM, int CodigoTipoDocumento, string DocumentoIdentidad, string FechaNacimiento, int Sexo)
        {
            SqlConnection cn = con.conexion();
            cn.Open();

            SqlDataAdapter dap = new SqlDataAdapter("SPI_PACIENTE", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@Nombres", Nombres);
            dap.SelectCommand.Parameters.AddWithValue("@ApellidoP", ApellidoP);
            dap.SelectCommand.Parameters.AddWithValue("@ApellidoM", ApellidoM);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoTipoDocumento", CodigoTipoDocumento);
            dap.SelectCommand.Parameters.AddWithValue("@DocumentoIdentidad", DocumentoIdentidad);
            dap.SelectCommand.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento);
            dap.SelectCommand.Parameters.AddWithValue("@Sexo", Sexo);
            dap.Fill(dt);

            cn.Close();
            return dt;

        }

        public DataTable BuscarPacxDocumento(string Documento)
        {
            SqlConnection cn = con.conexion();
            cn.Open();

            SqlDataAdapter dap = new SqlDataAdapter("SPS_PACIENTE_BS", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@Documento", Documento);
            dap.Fill(dt);

            cn.Close();
            return dt;
        }

        [WebMethod]
        public Proyecto[] ListadoProyectos(string local)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT p.CodigoProyecto,p.Nombre " +
                "FROM LOCAL_PROYECTO AS lp INNER JOIN " +
                "PROYECTO AS p ON lp.CodigoProyecto = p.CodigoProyecto " +
                "WHERE lp.estado=1 AND CodigoLocal = " + local;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Proyecto> lista = new List<Proyecto>();

            while (reader.Read())
            {
                lista.Add(new Proyecto(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
        }
        [WebMethod]
        public Proyecto[] ListadoProyectos1(int CodigoLocal, int CodigoUsuario)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_PROYECTOS_X_LOCAL_X_USUARIO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", CodigoUsuario);
            dap.Fill(dt);

            DataTableReader reader = dt.CreateDataReader();

            List<Proyecto> lista = new List<Proyecto>();

            while (reader.Read())
            {
                lista.Add(new Proyecto(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
        }
        [WebMethod]
        public Visita[] ListadoGrupoVisitas(string CodigoPaciente, int CodigoLocal, int CodigoProyecto)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_DATOS_SEDE_PROY1", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.Fill(dt);

            DataTableReader reader = dt.CreateDataReader();

            List<Visita> lista = new List<Visita>();

            while (reader.Read())
            {
                lista.Add(new Visita(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetString(2),
                    reader.GetInt32(3),
                    reader.GetString(4),
                    reader.GetBoolean(5),
                    reader.GetInt32(6)));
            }

            cn.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public int InsertarVisitas(int CodigoLocal, int CodigoProyecto, int CodigoGrupoVisita, int CodigoVisita, string CodigoPaciente, string FechaVisita, string HoraCita, int CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_VISITAS", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoLocal", SqlDbType.Int)).Value = CodigoLocal;
                cmd.Parameters.Add(new SqlParameter("@CodigoProyecto", SqlDbType.Int)).Value = CodigoProyecto;
                cmd.Parameters.Add(new SqlParameter("@CodigoGrupoVisita", SqlDbType.Int)).Value = CodigoGrupoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoVisita", SqlDbType.Int)).Value = CodigoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@FechaVisita", SqlDbType.VarChar, 10)).Value = FechaVisita;
                cmd.Parameters.Add(new SqlParameter("@HoraCita", SqlDbType.VarChar, 5)).Value = HoraCita;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
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

        [WebMethod]
        public Visitas[] ListadoVisitas(string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            //            SELECT        PY.Nombre AS Proyecto, E.Nombre AS Visita, SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) 
            //                         AS FechaVisita, CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita
            //            FROM         VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 
            //                         INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita 
            //                         INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5
            //            WHERE        (V.CodigoPaciente = 'b0875796-a823-455b-a48a-4da85d050fca') AND (V.CodigoLocal = 1) AND (V.CodigoProyecto = 1)
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita " +
                "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                "WHERE V.CodigoPaciente = '" + CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas> lista = new List<Visitas>();

            while (reader.Read())
            {
                lista.Add(new Visitas(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4)));
            }

            cn.Close();
            return lista.ToArray();
        }
        [WebMethod]
        public String ListadoFormatos(String CodigoUsuario, int CodigoLocal, int CodigoProyecto, int CodigoGrupoVisita, int CodigoVisita)
        {
            SqlConnection cn = con.conexion();
            //WHERE        (F.IdTipoDeFormato = '04') AND (U.CodigoUsuarioSP = '0') AND (PU.CodigoProyecto = 5) AND (U.CodigoLocal = 1) AND (R.CodigoVisita = 1) AND 
            //             
            cn.Open();
            string sql = "SELECT F.IdFormatoNemotecnico AS FormID " +
                         "FROM SEIS_DATA.dbo.Usuarios AS U INNER JOIN " +
                         "SEIS_DATA.dbo.Proyecto_Usuario AS PU ON U.CodigoUsuario = PU.CodigoUsuario INNER JOIN " +
                         "SEIS_DATA.dbo.RutaServicioFormato AS R ON PU.CodigoProyecto = R.CodigoProyecto INNER JOIN " +
                         "SEIS_DATA.dbo.Formato AS F ON R.IdFormato = F.IdFormato " +
                         "WHERE F.IdTipoDeFormato = '04' AND U.CodigoUsuarioSP = '" + CodigoUsuario + "' AND " +
                            "PU.CodigoProyecto = " + CodigoProyecto + " AND U.CodigoLocal = " + CodigoLocal + " AND " +
                            "R.CodigoVisita = " + CodigoVisita + " AND R.CodigoGrupoVisita = " + CodigoGrupoVisita;
            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            String lstFormatos = "";

            while (reader.Read())
            {
                lstFormatos += reader.GetString(0) + "/";
            }

            cn.Close();

            return lstFormatos;
        }
        [WebMethod]
        public Visitas1[] ListadoVisitas1(string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), V.CodigoVisitas, 103) AS CodigoVisitas " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas1> lista = new List<Visitas1>();

            while (reader.Read())
            {
                lista.Add(new Visitas1(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public Visitas1[] ListadoVisitas2(string CodigoPaciente, string CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT PY.Nombre AS Proyecto, E.Nombre AS Visita, " +
                 "SUBSTRING(DATENAME(dw, V.FechaVisita), 1, 3) + ' ' + CONVERT(varchar(10), V.FechaVisita, 103) AS FechaVisita," +
                 "CONVERT(varchar(5), V.HoraInicio, 108) AS HoraCita, EC.Descripcion AS EstadoVisita ,CONVERT(varchar(5), V.CodigoProyecto, 103) AS CodigoProyecto," +
                 "CONVERT(varchar(5), V.CodigoGrupoVisita, 103) AS CodigoGrupoVisita,CONVERT(varchar(5), V.CodigoVisita, 103) AS CodigoVisita, CONVERT(varchar(5), V.CodigoVisitas, 103) AS CodigoVisitas " +
                 "FROM  VISITAS AS V INNER JOIN PROYECTO AS PY ON V.CodigoProyecto = PY.CodigoProyecto AND V.Estado = 1 " +
                 "INNER JOIN USUARIOS_PROYECTO AS UP ON UP.CodigoProyecto = V.CodigoProyecto " +
                 "INNER JOIN VISITA AS E ON V.CodigoProyecto = E.CodigoProyecto AND V.CodigoGrupoVisita = E.CodigoGrupoVisita AND V.CodigoVisita = E.CodigoVisita " +
                 "INNER JOIN PARAMETROS AS EC ON V.CodigoEstadoVisita = EC.CodigoParametro AND EC.Codigo = 5 " +
                 "WHERE V.CodigoPaciente = '" + CodigoPaciente + "' AND UP.CodigoUsuario = " + CodigoUsuario;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Visitas1> lista = new List<Visitas1>();

            while (reader.Read())
            {
                lista.Add(new Visitas1(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public int EstadoVisita(int CodigoLocal, int CodigoProyecto, int CodigoVisita, int CodigoVisitas,
            string CodigoPaciente, int CodigoEstadoVisita, int CodigoEstatusPaciente, int CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPU_ESTADO_VISITA", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoLocal", SqlDbType.Int)).Value = CodigoLocal;
                cmd.Parameters.Add(new SqlParameter("@CodigoProyecto", SqlDbType.Int)).Value = CodigoProyecto;
                cmd.Parameters.Add(new SqlParameter("@CodigoVisita", SqlDbType.Int)).Value = CodigoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoVisitas", SqlDbType.Int)).Value = CodigoVisitas;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoEstadoVisita", SqlDbType.Int)).Value = CodigoEstadoVisita;
                cmd.Parameters.Add(new SqlParameter("@CodigoEstatusPaciente", SqlDbType.Int)).Value = CodigoEstatusPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
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

        [WebMethod]
        public lstId[] ListadoIds(string CodigoPaciente, string CodigoUsuario)
        {
            SqlConnection cn = con.conexion();
            cn.Open();
            string sql = "SELECT CONVERT(varchar(5), U.CodigoProyecto, 103) AS CodigoProyecto,PY.Nombre AS Proyecto, " +
                 "CASE WHEN ct.Numero IS NULL THEN '' ELSE ct.Numero END AS IdTAM, " +
                 "CASE WHEN ce.Numero IS NULL THEN '' ELSE ce.Numero END AS IdENR " +
                 "FROM dbo.USUARIOS_PROYECTO U " +
                 "INNER JOIN PROYECTO AS PY ON U.CodigoProyecto = PY.CodigoProyecto AND U.Estado=1 " +
                 "INNER JOIN PACIENTE_LOCAL_PROYECTO PP ON PY.CodigoProyecto = PP.CodigoProyecto " +
                 "LEFT JOIN PACIENTE_COD_TAM ct on ct.CodigoTAM=PP.CodigoTAM AND ct.CodigoLocal=PP.CodigoLocal AND ct.CodigoProyecto=PP.CodigoProyecto " +
                 "LEFT JOIN PACIENTE_COD_ENR ce on ce.CodigoENR=PP.CodigoENR AND ce.CodigoLocal=PP.CodigoLocal AND ce.CodigoProyecto=PP.CodigoProyecto " +
                 "WHERE PP.CodigoPaciente = '" + CodigoPaciente + "' AND U.CodigoUsuario = " + CodigoUsuario;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<lstId> lista = new List<lstId>();

            while (reader.Read())
            {
                lista.Add(new lstId(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)));
            }

            cn.Close();
            return lista.ToArray();
        }

        [WebMethod]
        public Idreg[] MostrarTipoId(int CodigoLocal, int CodigoProyecto, String CodigoPaciente)
        {
            DataTable dt = new DataTable();
            dt = this.ListarDatosxPaciente(CodigoLocal, CodigoProyecto, CodigoPaciente);
            List<Idreg> lista = new List<Idreg>();
            if (dt.Rows.Count > 0)
            {
                String vPaciente = dt.Rows[0]["NombreCompleto"].ToString();
                int vTipoTAM = Convert.ToInt32(dt.Rows[0]["TipoTAM"].ToString());
                String vIdTAM = dt.Rows[0]["IdTAM"].ToString();
                int vTipoENR = Convert.ToInt32(dt.Rows[0]["TipoENR"].ToString());
                String vIdENR = dt.Rows[0]["IdENR"].ToString();
                //if (vTipoENR == 2) msj = "auto";
                //if (vTipoENR == 0 || vTipoENR == 1) msj = "manual";
                lista.Add(new Idreg(
                    vPaciente,
                    vTipoTAM,
                    vIdTAM,
                    vTipoENR,
                    vIdENR));
            }
            return lista.ToArray();
        }

        [WebMethod]
        public string AsignarID_ENR(int TipoENR, int CodigoLocal, int CodigoProyecto, String CodigoPaciente, String IdENR, int CodigoUsuario)
        {
            DataTable dtRegistro = new DataTable();
            string msje = "";
            if (TipoENR == 0 || TipoENR == 1)
            {
                dtRegistro = this.RegistrarIdENR(CodigoLocal, CodigoProyecto, CodigoPaciente, IdENR, CodigoUsuario);
            }
            if (TipoENR == 2)
            {
                dtRegistro = this.RegistrarIdENRauto(CodigoLocal, CodigoProyecto, CodigoPaciente, CodigoUsuario);
            }
            if (dtRegistro.Rows.Count > 0)
            {
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "3")
                {
                    msje = "El ID se asigno correctamente...";
                }
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "1")
                {
                    msje = dtRegistro.Rows[0]["Texto"].ToString();
                }
            }
            return msje;
        }

        [WebMethod]
        public string AsignarID_TAM(int TipoTAM, int CodigoLocal, int CodigoProyecto, String CodigoPaciente, String IdTAM, int CodigoUsuario)
        {
            DataTable dtRegistro = new DataTable();
            string msje = "";
            if (TipoTAM == 0 || TipoTAM == 1)
            {
                dtRegistro = this.RegistrarIdTAM(CodigoLocal, CodigoProyecto, CodigoPaciente, IdTAM, CodigoUsuario);
            }
            if (TipoTAM == 2)
            {
                dtRegistro = this.RegistrarIdTAMauto(CodigoLocal, CodigoProyecto, CodigoPaciente, CodigoUsuario);
            }
            if (dtRegistro.Rows.Count > 0)
            {
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "3")
                {
                    msje = "El ID se asigno correctamente...";
                }
                if (dtRegistro.Rows[0]["Respuesta"].ToString() == "1")
                {
                    msje = dtRegistro.Rows[0]["Texto"].ToString();
                }
            }
            return msje;
        }
        public DataTable RegistrarIdENRauto(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_ENR_AUTO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }

        public DataTable RegistrarIdENR(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, string IdENR, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_ENR", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@IdENR", IdENR);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }
        public DataTable RegistrarIdTAMauto(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_TAM_AUTO", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }
        public DataTable RegistrarIdTAM(int CodigoLocal, int CodigoProyecto, string CodigoPaciente, string IdTAM, int IdUsuario)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPI_ID_TAM", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.SelectCommand.Parameters.AddWithValue("@IdTAM", IdTAM);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoUsuario", IdUsuario);
            dap.Fill(dt);
            return dt;
        }
        public DataTable ListarDatosxPaciente(int CodigoLocal, int CodigoProyecto, string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();
            SqlDataAdapter dap = new SqlDataAdapter("SPS_CABECERA", cn);
            DataTable dt = new DataTable();
            dap.SelectCommand.CommandType = CommandType.StoredProcedure;
            dap.SelectCommand.Parameters.AddWithValue("@CodigoLocal", CodigoLocal);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoProyecto", CodigoProyecto);
            dap.SelectCommand.Parameters.AddWithValue("@CodigoPaciente", CodigoPaciente);
            dap.Fill(dt);
            return dt;
        }
        [WebMethod]
        public Paciente[] ListadoPacientesUsuario(string CodigoUsuario)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT convert(varchar(100),CodigoPaciente,103) AS CodigoPaciente " +
                "FROM USUARIOS_PACIENTES " +
                "WHERE CodigoUsuario=" + CodigoUsuario;

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Paciente> lista = new List<Paciente>();

            while (reader.Read())
            {
                lista.Add(new Paciente(reader.GetString(0)));
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
        [WebMethod]
        public Participante[] BuscarParticipanteConCodigo(string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            string sql = "select CONVERT(varchar(100), CodigoPaciente, 103) AS CodigoPaciente,Nombres,ApellidoPaterno,ApellidoMaterno," +
                    "CodigoTipoDocumento,DocumentoIdentidad,convert(varchar(10),FechaNacimiento,103) FechaNacimiento," +
                    "Sexo from PACIENTE WHERE CodigoPaciente = '" + CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Participante> lista = new List<Participante>();

            while (reader.Read())
            {
                lista.Add(new Participante(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetInt32(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetInt32(7)));
            }

            cn.Close();

            return lista.ToArray();
        }
        [WebMethod]
        public int RegistrarPacientesUsuarios(string CodigoPaciente, int CodigoUsuario, int Estado)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_USUARIO_PACIENTE", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoUsuario", SqlDbType.Int)).Value = CodigoUsuario;
                cmd.Parameters.Add(new SqlParameter("@Estado", SqlDbType.Int)).Value = Estado;
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

        [WebMethod]
        public int RegistrarPacientesContactos(
            string CodigoPaciente, 
            String CodigoUbigeo,
            String Direccion,
            String Referencia,
            String Telefono1,
            String Telefono2,
            String Celular)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_PACIENTE_CONTACTO", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoUbigeo", SqlDbType.VarChar, 9)).Value = CodigoUbigeo;
                cmd.Parameters.Add(new SqlParameter("@Direccion", SqlDbType.VarChar, 1000)).Value = Direccion;
                cmd.Parameters.Add(new SqlParameter("@Referencia", SqlDbType.VarChar, 1000)).Value = Referencia;
                cmd.Parameters.Add(new SqlParameter("@Telefono1", SqlDbType.VarChar, 20)).Value = Telefono1;
                cmd.Parameters.Add(new SqlParameter("@Telefono2", SqlDbType.VarChar, 20)).Value = Telefono2;
                cmd.Parameters.Add(new SqlParameter("@Celular", SqlDbType.VarChar, 20)).Value = Celular;
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

        [WebMethod]
        public Contacto[] ListadoPacientesContactos(string CodigoPaciente)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT convert(varchar(100),CodigoPaciente,103) AS CodigoPaciente, " +
                "CASE WHEN CodigoUbigeo IS NULL THEN '' ELSE CodigoUbigeo END AS CodigoUbigeo, " +
                "CASE WHEN Direccion IS NULL THEN '' ELSE Direccion END AS Direccion, " +
                "CASE WHEN Referencia IS NULL THEN '' ELSE Referencia END AS Referencia, " +
                "CASE WHEN Telefono1 IS NULL THEN '' ELSE Telefono1 END AS Telefono1, " +
                "CASE WHEN Telefono2 IS NULL THEN '' ELSE Telefono2 END AS Telefono2, " +
                "CASE WHEN Celular IS NULL THEN '' ELSE Celular END AS Celular " +
                "FROM PACIENTE_CONTACTO " +
                "WHERE CodigoPaciente='" + CodigoPaciente + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Contacto> lista = new List<Contacto>();

            while (reader.Read())
            {
                lista.Add(new Contacto
                    (reader.GetString(0),
                     reader.GetString(1),
                     reader.GetString(2),
                     reader.GetString(3),
                     reader.GetString(4),
                     reader.GetString(5),
                     reader.GetString(6)));
            }

            cn.Close();

            return lista.ToArray();
        }
        [WebMethod]
        public Proyecto[] ListadoProyectos2()
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT p.CodigoProyecto,p.Nombre " +
                "FROM PROYECTO AS p " +
                "WHERE p.estado=1";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Proyecto> lista = new List<Proyecto>();

            while (reader.Read())
            {
                lista.Add(new Proyecto(reader.GetInt32(0), reader.GetString(1)));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public Inscripcion[] ListadoPacienteInscripcion(String CodigoPaciente)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT convert(varchar(100),CodigoPaciente,103) AS CodigoPaciente " +
                        ",CONVERT(varchar, ISNULL(Lunes, 0)) AS Lunes " +
                        ",CONVERT(varchar, ISNULL(Martes, 0)) AS Martes " +
                        ",CONVERT(varchar, ISNULL(Miercoles, 0)) AS Miercoles " +
                        ",CONVERT(varchar, ISNULL(Jueves, 0)) AS Jueves " +
                        ",CONVERT(varchar, ISNULL(Viernes, 0)) AS Viernes " +
                        ",CONVERT(varchar, ISNULL(Sabado, 0)) AS Sabado " +
                        ",CONVERT(varchar, ISNULL(Domingo, 0)) AS Domingo " +
                        ",CONVERT(varchar(10), FechaComienzo, 103) AS FechaComienzo " +
                        ",CONVERT(varchar(10), FechaTermino, 103) AS FechaTermino " +
                        ",CONVERT(varchar, ISNULL(Activo, 0)) AS Activo FROM PACIENTE_INSCRIPCION " +
                        "WHERE CodigoPaciente ='" + CodigoPaciente + "' AND Activo = 1";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Inscripcion> lista = new List<Inscripcion>();

            while (reader.Read())
            {
                lista.Add(new Inscripcion(
                    reader.GetString(0), 
                    reader.GetString(1),
                    reader.GetString(2), 
                    reader.GetString(3),
                    reader.GetString(4), 
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9),
                    reader.GetString(10)
                    ));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public PacCelular[] ListadoPacienteCelular(int CodigoUsuario,String Fecha)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT convert(varchar(100),cp.CodigoPaciente,103) AS CodigoPaciente, "+
                        "CASE WHEN c.Celular IS NULL THEN '' ELSE Celular END AS Celular FROM " +
                        "(SELECT up.CodigoPaciente FROM " +
                        "(SELECT CodigoPaciente FROM USUARIOS_PACIENTES WHERE CodigoUsuario = " + CodigoUsuario + ") up " +
                        "LEFT OUTER JOIN  " +
                        "(SELECT * FROM " +
                        "(SELECT v.CodigoPaciente,dayofweek, pi.Domingo, pi.Lunes, pi.Martes, pi.Miercoles, pi.Jueves, pi.Viernes, pi.Sabado FROM  " +
                        "(SELECT CodigoPaciente, datepart(dw,FechaVisita) as dayofweek FROM VISITAS WHERE FechaVisita ='" + Fecha + "') v " +
                        "LEFT OUTER JOIN " +
                        "(SELECT * FROM  PACIENTE_INSCRIPCION WHERE ACTIVO  = 1) pi " +
                        "ON v.CodigoPaciente = pi.CodigoPaciente) k " +
                        "WHERE ((dayofweek = 1 AND Domingo = 1) " +
                        "OR (dayofweek = 2 AND Lunes=1) " +
                        "OR (dayofweek =3 AND Martes=1) " +
                        "OR (dayofweek =4 AND Miercoles=1) " +
                        "OR (dayofweek =5 AND Jueves=1) " +
                        "OR (dayofweek = 6 AND Jueves = 1) " +
                        "OR (dayofweek = 7 AND Sabado = 1))) d " +
                        "on up.CodigoPaciente = d.CodigoPaciente " +
                        "WHERE dayofweek is NULL) cp  " +
                        "LEFT OUTER JOIN " +
                        "(SELECT * FROM PACIENTE_CONTACTO) c " +
                        "on cp.CodigoPaciente = c.CodigoPaciente";
            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<PacCelular> lista = new List<PacCelular>();

            while (reader.Read())
            {
                lista.Add(new PacCelular(
                    reader.GetString(0),
                    reader.GetString(1)
                    ));
            }

            cn.Close();

            return lista.ToArray();
        }

        [WebMethod]
        public int RegistrarPacientesInscripcion(
            String CodigoPaciente,
            String Lunes,
            String Martes,
            String Miercoles,
            String Jueves,
            String Viernes,
            String Sabado,
            String Domingo,
            String FechaComienzo,
            String FechaTermino)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_PACIENTE_INSCRIPCION", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar, 50)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@Lunes", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Lunes);
                cmd.Parameters.Add(new SqlParameter("@Martes", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Martes);
                cmd.Parameters.Add(new SqlParameter("@Miercoles", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Miercoles);
                cmd.Parameters.Add(new SqlParameter("@Jueves", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Jueves);
                cmd.Parameters.Add(new SqlParameter("@Viernes", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Viernes);
                cmd.Parameters.Add(new SqlParameter("@Sabado", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Sabado);
                cmd.Parameters.Add(new SqlParameter("@Domingo", SqlDbType.Bit, 1)).Value = Convert.ToBoolean(Domingo);
                cmd.Parameters.Add(new SqlParameter("@FechaComienzo", SqlDbType.VarChar, 10)).Value = FechaComienzo;
                cmd.Parameters.Add(new SqlParameter("@FechaTermino", SqlDbType.VarChar, 10)).Value = FechaTermino;
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

        [WebMethod]
        public Esquema[] ListadoPacienteEsquema(String CodigoPaciente)
        {
            SqlConnection cn = con.conexion();

            cn.Open();

            string sql = "SELECT PE.CodigoPaciente,"+
                        "PE.CodigoEsquema,"+
                        "PE.LunesManana,"+
                        "PE.MartesManana,"+
                        "PE.MiercolesManana,"+
                        "PE.JuevesManana,"+
                        "PE.ViernesManana,"+
                        "PE.SabadoManana,"+
                        "PE.DomingoManana,"+
                        "PE.LunesTarde,"+
                        "PE.MartesTarde,"+
                        "PE.MiercolesTarde,"+
                        "PE.JuevesTarde,"+
                        "PE.ViernesTarde,"+
                        "PE.SabadoTarde,"+
                        "PE.DomingoTarde,"+
                        "E.Nombre,"+ 
                        "E.Fase "+
                        "FROM "+
                        "(SELECT convert(varchar(100),CodigoPaciente,103) AS CodigoPaciente "+ 
                        ",CONVERT(varchar(10), ISNULL(CodigoEsquema, 0)) AS CodigoEsquema "+
                        ",CONVERT(varchar, ISNULL(LunesManana, 0)) AS LunesManana "+
                        ",CONVERT(varchar, ISNULL(MartesManana, 0)) AS MartesManana "+
                        ",CONVERT(varchar, ISNULL(MiercolesManana, 0)) AS MiercolesManana "+ 
                        ",CONVERT(varchar, ISNULL(JuevesManana, 0)) AS JuevesManana "+ 
                        ",CONVERT(varchar, ISNULL(ViernesManana, 0)) AS ViernesManana "+ 
                        ",CONVERT(varchar, ISNULL(SabadoManana, 0)) AS SabadoManana "+ 
                        ",CONVERT(varchar, ISNULL(DomingoManana, 0)) AS DomingoManana "+ 
                        ",CONVERT(varchar, ISNULL(LunesTarde, 0)) AS LunesTarde "+ 
                        ",CONVERT(varchar, ISNULL(MartesTarde, 0)) AS MartesTarde "+ 
                        ",CONVERT(varchar, ISNULL(MiercolesTarde, 0)) AS MiercolesTarde "+ 
                        ",CONVERT(varchar, ISNULL(JuevesTarde, 0)) AS JuevesTarde "+ 
                        ",CONVERT(varchar, ISNULL(ViernesTarde, 0)) AS ViernesTarde "+ 
                        ",CONVERT(varchar, ISNULL(SabadoTarde, 0)) AS SabadoTarde "+ 
                        ",CONVERT(varchar, ISNULL(DomingoTarde, 0)) AS DomingoTarde "+ 
                        ",CONVERT(varchar(10), FechaComienzo, 103) AS FechaComienzo "+ 
                        ",CONVERT(varchar(10), FechaTermino, 103) AS FechaTermino "+ 
                        ",CONVERT(varchar, ISNULL(TipoDeVisita, 0)) AS TipoDeVisito "+ 
                        ",CONVERT(varchar, ISNULL(Activo, 0)) AS Activo FROM PACIENTE_ESQUEMA "+
                        "WHERE CodigoPaciente ='" + CodigoPaciente + "' AND Activo = 1) PE " +
                        "INNER JOIN (SELECT  convert(varchar(100),Nombre,103) AS Nombre, "+
                        "CONVERT(varchar, ISNULL(Fase, 0)) AS Fase, "+ 
                        "CONVERT(varchar(10), ISNULL(CodigoEsquema, 0)) AS CodigoEsquema "+ 
                        "FROM ESQUEMAS) E "+
                        "ON PE.CodigoEsquema=E.CodigoEsquema";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Esquema> lista = new List<Esquema>();

            while (reader.Read())
            {
                lista.Add(new Esquema(
                    reader.GetString(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6),
                    reader.GetString(7),
                    reader.GetString(8),
                    reader.GetString(9),
                    reader.GetString(10),
                    reader.GetString(11),
                    reader.GetString(12),
                    reader.GetString(13),
                    reader.GetString(14),
                    reader.GetString(15),
                    reader.GetString(16),
                    reader.GetString(17),
                    reader.GetString(18),
                    reader.GetString(19),
                    reader.GetString(20),
                    reader.GetString(21)
                    ));
            }

            cn.Close();

            return lista.ToArray();
        }

/**
 * list the drug information (id, name, symbol, dosage) given a schema number
 */
        [WebMethod]
        public Droga[] ListadoEsquemasDrogas(string CodigoEsquema)
        {
            SqlConnection cn = con.conexion();

            cn.Open();
            string sql = "SELECT dr.CodigoDroga, Farmacos, Siglas, DosisMaxima " +
                            "FROM ESQUEMAS_DROGAS AS ed " + 
                            "INNER JOIN DROGAS AS dr " +
                            "ON ed.CodigoDroga = dr.CodigoDroga " +
                            "WHERE CodigoEsquema = '" + CodigoEsquema + "'";

            SqlCommand cmd = new SqlCommand(sql, cn);

            SqlDataReader reader = cmd.ExecuteReader();

            List<Droga> lista = new List<Droga>();

            while (reader.Read())
            {
                lista.Add(new Droga(
                    reader.GetInt32(0), 
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)
                    ));
            }

            cn.Close();

            return lista.ToArray();
        }

/**
 * insert a patient schema into the data base
 */
        [WebMethod]
        public int InsertarPacienteEsquema(string CodigoPaciente, string CodigoEsquema,
            string LunesManana, string MartesManana,
            string MiercolesManana, string JuevesManana, 
            string ViernesManana, string SabadoManana,
            string DomingoManana, string LunesTarde,
            string MartesTarde, string MiercolesTarde,
            string JuevesTarde, string ViernesTarde,
            string SabadoTarde, string DomingoTarde,
            string FechaComienzo, string FechaTermino, string TipoDeVisita)
        {
            SqlConnection cn = con.conexion();
            SqlCommand cmd = new SqlCommand("SPI_PACIENTE_ESQUEMA", cn);
            SqlTransaction trx;
            int intretorno;
            string strRespuesta;

            try
            {
                cn.Open();
                trx = cn.BeginTransaction();
                cmd.Transaction = trx;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CodigoPaciente", SqlDbType.VarChar,256)).Value = CodigoPaciente;
                cmd.Parameters.Add(new SqlParameter("@CodigoEsquema", SqlDbType.Int)).Value = CodigoEsquema;
                cmd.Parameters.Add(new SqlParameter("@LunesManana", SqlDbType.Bit)).Value = LunesManana;
                cmd.Parameters.Add(new SqlParameter("@MartesManana", SqlDbType.Bit)).Value = MartesManana;
                cmd.Parameters.Add(new SqlParameter("@MiercolesManana", SqlDbType.Bit)).Value = MiercolesManana;
                cmd.Parameters.Add(new SqlParameter("@JuevesManana", SqlDbType.Bit)).Value = JuevesManana;
                cmd.Parameters.Add(new SqlParameter("@ViernesManana", SqlDbType.Bit)).Value = ViernesManana;
                cmd.Parameters.Add(new SqlParameter("@SabadoManana", SqlDbType.Bit)).Value = SabadoManana;
                cmd.Parameters.Add(new SqlParameter("@DomingoManana", SqlDbType.Bit)).Value = DomingoManana;
                cmd.Parameters.Add(new SqlParameter("@LunesTarde", SqlDbType.Bit)).Value = LunesTarde;
                cmd.Parameters.Add(new SqlParameter("@MartesTarde", SqlDbType.Bit)).Value = MartesTarde;
                cmd.Parameters.Add(new SqlParameter("@MiercolesTarde", SqlDbType.Bit)).Value = MiercolesTarde;
                cmd.Parameters.Add(new SqlParameter("@JuevesTarde", SqlDbType.Bit)).Value = JuevesTarde;
                cmd.Parameters.Add(new SqlParameter("@ViernesTarde", SqlDbType.Bit)).Value = ViernesTarde;
                cmd.Parameters.Add(new SqlParameter("@SabadoTarde", SqlDbType.Bit)).Value = SabadoTarde;
                cmd.Parameters.Add(new SqlParameter("@DomingoTarde", SqlDbType.Bit)).Value = DomingoTarde;
                cmd.Parameters.Add(new SqlParameter("@FechaComienzo", SqlDbType.VarChar, 10)).Value = FechaComienzo;
                cmd.Parameters.Add(new SqlParameter("@FechaTermino", SqlDbType.VarChar, 10)).Value = FechaTermino;
                cmd.Parameters.Add(new SqlParameter("@TipoDeVisita", SqlDbType.Bit)).Value = TipoDeVisita;

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
                return -2;
            }
        }   
    }

}