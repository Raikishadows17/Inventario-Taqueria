using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Taqueria
{
    public partial class Inicio : Form
    {
        
        Datos datos = new Datos();
        string producto, precio, fecha;

        public Inicio()
        {
            InitializeComponent();
            datos.CrearDB();
            CargarDGV();

        }

        private void dgvTaqueria_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtNombre_Enter(object sender, EventArgs e)
        {
            TextBox textb = (TextBox)sender;
            if (textb.Text == textb.Tag.ToString())
            {
                textb.Text = string.Empty;
                textb.ForeColor = Color.Black;
            }
        }

        private void txtNombre_Leave(object sender, EventArgs e)
        {
            TextBox textb = (TextBox)sender;
            if (string.IsNullOrEmpty(textb.Text))
            {
                textb.Text = textb.Tag.ToString();
                textb.ForeColor = Color.DimGray;
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Vacio())
            {
                AsignarValores();
                GuardarDatos();

                txtNombre.Text="";
                txtPrecio.Text = "";
                txtFecha.Text = "";
            }
        }

        private void GuardarDatos()
        {
            datos.Conectar();
            string cadena = "Insert into Taqueria (Nombre, Precio, Fecha) Values" +
                "(@Nombre, @Precio, @Fecha)";
            SqliteCommand comando = new SqliteCommand(cadena, datos.con);
            comando.Parameters.AddWithValue("@Nombre", producto);
            comando.Parameters.AddWithValue("@Precio", precio);
            comando.Parameters.AddWithValue("@Fecha", fecha);
            comando.ExecuteNonQuery();
            MessageBox.Show("Datos almacenados exitosamente");
            datos.con.Close();
            CargarDGV();

        }
        private void CargarDGV()
        {
            dgvTaqueria.Rows.Clear();
            datos.con.Open();
            string cadena = "Select * From Taqueria";
            SqliteCommand comando = new SqliteCommand(cadena, datos.con);
            SqliteDataReader leer = comando.ExecuteReader();
            string[] info = new string[4];

            while (leer.Read())
            {
                for (int i = 0; i < 4; i++)
                {
                    info[i] = leer.GetValue(i).ToString();
                }
                dgvTaqueria.Rows.Add(info);
            }

            datos.con.Close();


        }

        private void txtfechafiltro_KeyPress(object sender, KeyPressEventArgs e)
        {
            Buscar();
        }
        private void Buscar()
        {

            dgvTaqueria.Rows.Clear();
            datos.con.Open();
            string cadena = "Select * From Taqueria where Fecha like('" + txtfechafiltro.Text + "%')";
            SqliteCommand comando = new SqliteCommand(cadena, datos.con);
            SqliteDataReader leer = comando.ExecuteReader();
            string[] info = new string[4];

            while (leer.Read())
            {
                for (int i = 0; i < 4; i++)
                {
                    info[i] = leer.GetValue(i).ToString();
                }
                dgvTaqueria.Rows.Add(info);
            }

            datos.con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            datos.con.Open();
            string cadena = "Select sum(precio) total From Taqueria where Fecha like('" + txtfechafiltro.Text + "%')";
            SqliteCommand comando = new SqliteCommand(cadena, datos.con);
            SqliteDataReader leer = comando.ExecuteReader();
            string total;
            leer.Read();
            total = leer["total"].ToString();
            //MessageBox.Show("Su ganancia es " + total);
            TxtTotal.Text = total.ToString();

            txtfechafiltro.Text = "";
            datos.con.Close();
            CargarDGV();
        }


        private void dgvTaqueria_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void AsignarValores()
        {
            producto = txtNombre.Text;
            precio = txtPrecio.Text;
            fecha = txtFecha.Text;
        }
        private bool Vacio()
        {
            foreach(Control ctrl in pmControles.Controls)
            {
                if(ctrl is TextBox &&  ctrl.Text == ctrl.Tag.ToString())
                {
                    MessageBox.Show("No debe dejar campos vacios");
                    return true;
                }
            }
            return false;
        }

    }
}
