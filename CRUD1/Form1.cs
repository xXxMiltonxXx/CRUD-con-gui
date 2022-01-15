using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace CRUD1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // botones para cerrar, maximizar y minimizar personalizados 
        //Cerrar
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        //Maximizar y luego de maximizar no sea visible
        // y el boton restaura si 

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            btnMaximizar.Visible = false;
            btnRestaurar.Visible = true;

        }
        // Restaurar se convierta en un boton no sea visible 
        // y el boton Maximizar si 
        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            btnRestaurar.Visible = false;
            btnMaximizar.Visible = true;

        }
        //minimizar la ventana
        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //usamos la libreria System.Runtime.InteropServices
        //para poder mover la ventana 

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
        
        //Boton Guardar funciones del boton Guardar
        //Guardar en la base de datos
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //funciones que va a tener a darle click 
                //recabamos en los datos de los controles

                String codigo = txtCodigo.Text;
                String nombre = txtNombre.Text;
                String descripcion = txtDescripcion.Text;
                //hacemos conversion ya que es de tipo double
                Double precio_publico = double.Parse(txtPrecioPublico.Text);
                //Hacemos conversion ya que las exixtencias son de tipo entero int
                int existencias = int.Parse(txtExistencias.Text);
                //Agregamos condiciones para que no se pueda guardar casilleros vacios
                if (codigo != "" && nombre != "" && descripcion != "" && precio_publico > 0 && existencias >= 0)
                {
                    //Crear una variable  manejar una insercion
                    //Insercion a la base de datos
                    //insertamos datos a la tabla de productos
                    //y a que columas voy a ingresar
                    //concatenamos
                    string sql = "INSERT INTO productos(codigo, nombre,descripcion, precio_publico,existencias) VALUES('" + codigo
                        + "','" + nombre + "','" + descripcion + "','" + precio_publico + "','" + existencias + "')";
                    // traemos nuestracomexion MySQL
                    //Treamos a la clase Conexionen y su metodo conexion 
                    //Traemos la libreria de MySQL a esta clase para usar sus recursos
                    MySqlConnection conexionBD = Conexion.conexion();
                    //Abrimos la conexión
                    conexionBD.Open();
                    //en la clase conexion esta agrergado el try y catch por si hay errores 
                    //al momento de iniciar secion con el MySQL no es necesario
                    //no es necesario implementar un try y catch 
                    //pero
                    //Usamos try y catch para implementar
                    //Vamos a insertar archivos a las tablas del servidor
                    // evitar errores y que se deje de ejecutar el programa
                    try
                    {
                        //preparamos para que se pueda insertar los datos
                        //insercion el string sql y la conexion conexionBD 
                        MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                        //ejecutamos el comando en Mysql
                        comando.ExecuteNonQuery();
                        //Enviamos un mensaje al usuario 
                        MessageBox.Show("Registro guardado");
                        //llamamos al metodo limpiar
                        limpiar();
                    }
                    //En caso de error o excepcion
                    catch (MySqlException ex)
                    {
                        //Enviamos el mensaje de error
                        MessageBox.Show("Error al guardar:" + ex.Message);
                    }
                    finally
                    {
                        //cerramos la conexion
                        //es necesario cerrarla
                        conexionBD.Close();
                    }
                    //Sino se cumple el if
                }
                else
                {
                    MessageBox.Show("Debe completar todos los campos");
                }
                //evitar errores en insertar formatos de escritura incorrectos en algun campo
            }catch(FormatException fex)
            {
                //mensaje de error
                MessageBox.Show("Datos incorrectos: " + fex.Message);
            }





        }
        //Boton Buscar Funciones del boton Buscar 
        //Buscar en la base de datos
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            //Vamos a tomar el campo del codigo 
            //y con este codigo se buscara en la base da datos algun campo coincida
            //Si queremos buscar por algun otro campo se debe de recivir de esta forma 
            string codigo = txtCodigo.Text;
            //contenedor donde se van a guardar los resultados de la consulta
            MySqlDataReader reader = null;
            //AQUI es donde va a estar la consulta
            //SELEC significa los campos o parametros que vamos a consultar
            //FROM es la tabla creada 
            //WHERE es como vamos a buscar (codigo)con el comando LIKE es una cadena de texto
            //LIMIT 1 es para que una vez que se encuentre el resultado lo muestre y no siga buscando 
            String sql = "SELECT id, codigo, nombre, descripcion, precio_publico, existencias FROM productos WHERE codigo LIKE '" + codigo + "'LIMIT 1";
            MySqlConnection conexionBD = Conexion.conexion();
            //Habrimos la conexion
            conexionBD.Open();

            try
            {
                //Preparamos el comando 
                //enviamos el comando squl y conexionBD
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                //Igualamos el resultado de la consulta a la variable reader
                //ya esta el resultado de la consulta
                reader = comando.ExecuteReader();
                //Verificamos que se haya encontrado un resultado 
                if (reader.HasRows)
                {
                    //En caso de que existan filas 
                    //usamos while para pasar por las columnas
                    while (reader.Read())
                    {
                        //Igualamos las columnas a la caja de texto
                        //Trabajamos con indices
                        txtId.Text = reader.GetString(0);
                        txtCodigo.Text = reader.GetString(1);
                        txtNombre.Text = reader.GetString(2);
                        txtDescripcion.Text = reader.GetString(3);
                        txtPrecioPublico.Text = reader.GetString(4);
                        txtExistencias.Text = reader.GetString(5);
                    }
                }
                else
                {
                    //Indicamos que no se encontraron registros
                    //mediante un mensaje 
                    MessageBox.Show("No se encontraron registros");
                }
            }
            catch (MySqlException ex)
            {
                //por hay un error al momento de buscar
                MessageBox.Show("Error al buscar" + ex.Message);

            }
            finally
            {
                //Cerramos la conexión
                conexionBD.Close();

            }

        }
        //Boton Actualizar Funciones del boton Actualizar
        //Actualizar datos en la base de datos
        //Es muy parecido al boton de Guardar
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                //funciones que va a tener a darle click 
                //recabamos en los datos de los controles

                String id = txtId.Text;
                String codigo = txtCodigo.Text;
                String nombre = txtNombre.Text;
                String descripcion = txtDescripcion.Text;
                //hacemos conversion ya que es de tipo double
                Double precio_publico = double.Parse(txtPrecioPublico.Text);
                //Hacemos conversion ya que las exixtencias son de tipo entero int
                int existencias = int.Parse(txtExistencias.Text);
                //condiconal
                //Agregamos condiciones para que no se pueda guardar casilleros vacios
                if (id != "" && codigo != "" && nombre != "" && descripcion != "" && precio_publico > 0 && existencias >= 0)
                {
                    //Crear una variable  manejar una actualizacion
                    //actualizar a la base de datos
                    //actualizaremos a la tabla de productos
                    //y a que columas voy a ingresar
                    //concatenamos un UPDATE
                    string sql = "UPDATE productos SET codigo='" + codigo + "', nombre='" + nombre
                    + "',descripcion='" + descripcion + "', precio_publico='" + precio_publico
                    + "',existencias='" + existencias + "' WHERE id='" + id + "'";
                    // traemos nuestracomexion MySQL
                    //Treamos a la clase Conexionen y su metodo conexion 
                    //Traemos la libreria de MySQL a esta clase para usar sus recursos
                    MySqlConnection conexionBD = Conexion.conexion();
                    //Abrimos la conexión
                    conexionBD.Open();
                    //en la clase conexion esta agrergado el try y catch por si hay errores 
                    //al momento de iniciar secion con el MySQL no es necesario
                    //no es necesario implementar un try y catch 
                    //pero
                    //Usamos try y catch 
                    //Vamos a actualizar archivos a las tablas del servidor
                    // evitar errores para que no se deje de ejecutar el programa
                    try
                    {
                        //preparamos para que se pueda actualizar los datos
                        //insercion el string sql y la conexion conexionBD 
                        MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                        //ejecutamos el comando en Mysql
                        comando.ExecuteNonQuery();
                        //Enviamos un mensaje al usuario 
                        MessageBox.Show("Registro Mdoificado");
                        //llamanos al metodo limpiar
                        limpiar();
                    }
                    //En caso de error o excepcion
                    catch (MySqlException ex)
                    {
                        //Enviamos el mensaje de error
                        MessageBox.Show("Error al modificar" + ex.Message);
                    }
                    finally
                    {
                        //cerramos la conexion
                        //es necesario cerrarla
                        conexionBD.Close();
                    }
                    //Si no se cumple el if
                }
                else 
                {
                    MessageBox.Show("Debe completar todos los campos");
                }
            }
            //evitar errores en actualizar formatos de escritura incorrectos en algun campo
            catch (FormatException fex)
            {
                MessageBox.Show("Datos incorrectos:" + fex.Message);

            }

        }
        //Boton Eliminar Funciones del boton Eliminar
        //Eliminar datos de la base de datos
        //Es muy parecido al boton Actualizar
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            //funciones que va a tener a darle click 
            //recabamos en los datos de los controles
            String id = txtId.Text;
            //Crear una variable  manejar un eliminar
            //eliminar datos de la base de datos
            //Eliminiremos datos de la tabla de productos
            //y a que columas voy a eliminar los datos 
            //concatenamos un Eliminar
            //DELETE FROM Lugar tabla productos
            //Where varible id 
            string sql = "DELETE FROM productos WHERE id='" + id + "'";
            // traemos nuestracomexion MySQL
            //Treamos a la clase Conexionen y su metodo conexion 
            //Traemos la libreria de MySQL a esta clase para usar sus recursos
            MySqlConnection conexionBD = Conexion.conexion();
            //Abrimos la conexión
            conexionBD.Open();
            //en la clase conexion esta agrergado el try y catch por si hay errores 
            //al momento de iniciar secion con el MySQL no es necesario
            //no es necesario implementar un try y catch 
            //pero
            //Usamos try y catch 
            //Vamos a eliminar archivos a las tablas del servidor
            // evitar errores para que no se deje de ejecutar el programa
            try
            {
                //preparamos para que se pueda eliminar los datos
                //insercion el string sql y la conexion conexionBD 
                MySqlCommand comando = new MySqlCommand(sql, conexionBD);
                //ejecutamos el comando en Mysql
                comando.ExecuteNonQuery();
                //Enviamos un mensaje al usuario 
                MessageBox.Show("Registro Eliminado");
                //llamanos al metodo limpiar
                limpiar();
            }
            //En caso de error o excepcion
            catch (MySqlException ex)
            {
                //Enviamos el mensaje de error
                MessageBox.Show("Error al eliminar:" + ex.Message);
            }
            finally
            {
                //cerramos la conexion
                //es necesario cerrarla
                conexionBD.Close();
            }

        }
        //Botn Limpiar Funciones del boton Limpiar
        //Limpiar datos de 
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            //Llmanos al metodo limpiar
            limpiar();
        }
        //creamos un metodo private a para el boton limpiar
        private void limpiar()
        {
            txtId.Text = "";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecioPublico.Text = "";
            txtExistencias.Text = "";
        }

        //mover la ventana a voluntad
        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        
    }
}
