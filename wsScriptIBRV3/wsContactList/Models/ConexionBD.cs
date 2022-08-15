using System.Data.SqlClient;

namespace wsTeleavenceService.Models
{
    public class ConnexionBD
    {
        public static SqlConnection connexionBDTeleavance()
        {
            string conexion = @"Data Source=172.20.1.230;Initial Catalog=GENESYS;User ID=ibrscript;Password=ibrscript123";
            //string conexion = @"Data Source=10.37.16.12;Initial Catalog=GENESYS;User ID=Prueba;Password=Aa123456";
            //string conexion = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=GenesysCloudTeleavance;Integrated Security=True";
            SqlConnection conn = new System.Data.SqlClient.SqlConnection(conexion);
            return conn;
        }


    }
    public class ConnexionBDTeleavance
    {
        public static SqlConnection connexionBDDTeleavance()
        {
            string conexion = @"Data Source=172.20.1.230;Initial Catalog=GENESYS;User ID=econtact;Password=NiiIrCg3xUqe";
            //string conexion = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=GenesysCloudTeleavance;Integrated Security=True";
            SqlConnection conn = new System.Data.SqlClient.SqlConnection(conexion);
            return conn;
        }
    }
        
}