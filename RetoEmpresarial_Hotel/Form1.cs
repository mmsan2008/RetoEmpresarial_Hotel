using System;
using System.Windows.Forms;

namespace RetoEmpresarial_Hotel
{
    public partial class FormLogIn : Form
    {
        public FormLogIn()
        {
            InitializeComponent();
            lblError.Visible = false;
            txtboxContrasena.UseSystemPasswordChar = false;
            txtboxContrasena.PlaceholderText = "Contraseña / Nombre (cliente nuevo)";
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string input = txtboxUsuario.Text.Trim();
            lblError.Visible = false;

            if (!int.TryParse(input, out int codigo))
            {
                MostrarError("Ingrese un número válido (documento o código).");
                return;
            }

            if (codigo == 1) // ── Administrador ──────────────────────────
            {
                App.UsuarioActual = new Administrador("Administrador", 1, App.Hotel);
                this.Hide();
                new Form3Admin().ShowDialog();
                App.UsuarioActual = null;
                this.Close();
            }
            else if (codigo == 2) // ── Personal / Recepcionista ───────────
            {
                App.UsuarioActual = new Personal("Recepcionista", 2, App.Hotel);
                this.Hide();
                new Form2Recepcion().ShowDialog();
                App.UsuarioActual = null;
                this.Close();
            }
            else // ── Cliente ────────────────────────────────────────────
            {
                Cliente cliente = App.Hotel.Clientes.BuscarPorDocumento(codigo);

                if (cliente == null) // cliente nuevo: necesita nombre
                {
                    string nombre = txtboxContrasena.Text.Trim();
                    if (string.IsNullOrWhiteSpace(nombre))
                    {
                        MostrarError("Cliente nuevo: escriba su nombre en el campo inferior.");
                        return;
                    }
                    cliente = new Cliente(nombre, codigo, App.Hotel);
                    App.Hotel.Clientes.Registrar(cliente);
                }

                App.UsuarioActual = cliente;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void MostrarError(string msg)
        {
            lblError.Text = msg;
            lblError.Visible = true;
        }
    }
}