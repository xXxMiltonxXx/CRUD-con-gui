using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD1
{
    class Conexion
    {
        //Hacemos la conexión a MySQL
        //Integramos la libreria
        public static MySqlConnection conexion()
        {
            //ruta de servidor
            //nombre de base de datos 
            //nombre de usuario
            //nombre de la contraseña
            string servidor = "localhost";
            string bd = "tienda";
            string usuario = "root";
            string password = "159753";
            //Agregamos la cadena de conexion 
            //concatenamos la cadena de conexion
            string cadenaConexion = "database=" + bd + "; Data Source=" + servidor +
                "; User Id=" + usuario + ";Password=" + password+"";
            //Usamos (try) y catch
            //por si envia algun error y continue el programa
            try
            {
                //agregamos el objeto
                MySqlConnection conexionBD = new MySqlConnection
                    (cadenaConexion);
                //retornamos 
                return conexionBD;

            }
            catch (MySqlException ex)
            {
                //variable que trae la informacion
                //nos envie el mensaje del error
                Console.WriteLine("Error:" + ex.Message);
                //por que no se logró la coenxión
                return null;



            }


        }
    }
}
