using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;
namespace Taqueria
{
    public class Datos
    {
        static string database = Application.StartupPath + "/database.db";
        public SqliteConnection con;
        public Datos()
        {
            CrearDB();
            con = new SqliteConnection("Data Source=" + database);
            CrearTabla();

        }

        public void CrearDB()
        {
            if (!File.Exists(database))
            {
                var file = File.Create(database);
                file.Close();
            }
        }
        public void Conectar()
        {
            if(con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }
      
        private void CrearTabla()
        {
            Conectar();
            string cadena = "Create Table If Not Exists Taqueria (id Integer not null Primary Key Autoincrement, " +
                " Nombre Text Not null, Precio Text Not Null, Fecha Date Not null)";
            SqliteCommand comando = new SqliteCommand(cadena, con);
            comando.ExecuteNonQuery();
            con.Close();
        }
    }
}
