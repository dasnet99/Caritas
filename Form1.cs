using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaComun.Cache;
using System.Globalization;
using MenuBancoAlimentos;

namespace Caritas
{
    public partial class Form1 : Form
    {
        private IconButton iconoBoton; private IconButton cambiotitulo;
        private Panel panelresalta, PanelDesplazable;
        private Form formualarioActual;
        public static bool Cerrar = true;
        public Form1()
        {
            InitializeComponent();
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            panelresalta = new Panel();
            panelresalta.Size = new Size(7, 55);
            panel1.Controls.Add(panelresalta);
        }
        
        public void abrirformulario(Form nuevoform, object tituloicon)
        {
            if (formualarioActual != null)
            {
                formualarioActual.Close();
            }

            formualarioActual = nuevoform;
            nuevoform.TopLevel = false;
            panel4.Controls.Add(nuevoform);
            panel4.Tag = nuevoform;
            nuevoform.BringToFront();
            nuevoform.Show();
            cambiotitulo = (IconButton)tituloicon;
            titulo.Text = cambiotitulo.Text;
            titulo.IconChar = cambiotitulo.IconChar;
            titulo.IconColor = cambiotitulo.IconColor;
        }
        private void botonActivo(object botonremi, Color color, object PanelD)
        {
            if (botonremi != null)
            {
                DesactivarBoton();
                PanelDesplazable = (Panel)PanelD;
                iconoBoton = (IconButton)botonremi;
                iconoBoton.BackColor = Color.FromArgb(210,210,210);

                iconoBoton.TextAlign = ContentAlignment.MiddleCenter;
                iconoBoton.IconColor = color;
                iconoBoton.TextImageRelation = TextImageRelation.TextBeforeImage;
                iconoBoton.ImageAlign = ContentAlignment.MiddleRight;

                panelresalta.BackColor = color;
                panelresalta.Location = new Point(0, iconoBoton.Location.Y);
                panelresalta.Visible = true;
                panelresalta.BringToFront();

                PanelDesplazable.Visible = true;
            }
        }
        private void DesactivarBoton()
        {
            if (iconoBoton != null)
            {
                iconoBoton.BackColor = Color.White;
                iconoBoton.TextAlign = ContentAlignment.MiddleCenter;
                iconoBoton.IconColor = Color.FromArgb(237, 32, 123);
                iconoBoton.TextImageRelation = TextImageRelation.ImageBeforeText;
                iconoBoton.ImageAlign = ContentAlignment.MiddleLeft;
                panelresalta.Visible = false;
            }
            if (PanelDesplazable != null)
            {
                PanelDesplazable.Visible = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (formualarioActual != null)
            {
                formualarioActual.Close();
            }
            DesactivarBoton();
            panelresalta.Visible = false;
            titulo.Text = "Recepcion";
            titulo.IconChar = IconChar.Home;
            titulo.IconColor=Color.FromArgb(140, 0, 88);
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]  
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam,int lParam );
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
        ReleaseCapture();
        SendMessage(this.Handle,0x112,0xf012,0);
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {
            Form msgcerrar = new MensajesCaritas.Form2();
            msgcerrar.ShowDialog();
            if(MensajesCaritas.Form2.Cerrar==1)Application.Exit();
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
        private void Privilegios()
        {
            if (CacheLoginUsuario.cargo == Cargos.Administrador)
            {
                iconButton9.Visible= true;
                
            }
            if (CacheLoginUsuario.cargo == Cargos.BancoDeAlimentos)
            {
                iconButton2.Enabled = false;
                iconButton3.Enabled = false; 
                iconButton4.Enabled = false;
            }
            if (CacheLoginUsuario.cargo == Cargos.BazarDeRopa)
            {
                iconButton1.Enabled = false;
                iconButton3.Enabled = false;
                iconButton4.Enabled = false;
            }
            if (CacheLoginUsuario.cargo == Cargos.Dispensario)
            {
                iconButton2.Enabled = false;
                iconButton1.Enabled = false;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CargarDatosUser();
            Privilegios();
        }
        private void CargarDatosUser()
        {
            Nombre.Text = CacheLoginUsuario.nombres;
            Cargo.Text = CacheLoginUsuario.cargo;
        }
        private void iconButton1_Click(object sender, EventArgs e)
        {
            if (iconButton1.BackColor == Color.FromArgb(210, 210, 210))
            {
                DesactivarBoton();
            }
            else
            {
                botonActivo(sender, Color.Crimson, panel5);
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            if (iconButton2.BackColor == Color.FromArgb(210, 210, 210))
            {
                DesactivarBoton();
            }
            else
            {
                botonActivo(sender, Color.FromArgb(0, 119, 176), panel6);
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            //botonActivo(sender, Color.Indigo);
            abrirformulario(new CambiarCargo(), sender);
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            //botonActivo(sender, Color.FromArgb(216,48,48));
            abrirformulario(new CambiarCargo(), sender);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss");
            label2.Text = DateTime.Now.ToLongDateString();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton7_Click(object sender, EventArgs e)
        {
            Form cerse = new MensajesCaritas.Form1();
            cerse.ShowDialog();
            if (MensajesCaritas.Form1.cerrar == 1)
            {
                MensajeBienvenida.abrirOcerrar = 0;
                this.Hide();
                MensajeBienvenida bienve = new MensajeBienvenida();
                bienve.ShowDialog();
                LoginCaritas login = new LoginCaritas();
                login.Show();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int cont = 0;
            if (this.Opacity < .99) this.Opacity += 0.03;
            cont += 1;
            if (cont == 100)
            {
                timer2.Stop();

            }
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            Form useredit = new EditarUsuario();
            useredit.ShowDialog();
            if (EditarUsuario.cerrar == 1)
            {

                Form reini = new MensajesCaritas.Form4();
                reini.ShowDialog();
                MensajeBienvenida.abrirOcerrar = 0;
                this.Hide();
                MensajeBienvenida bienve = new MensajeBienvenida();
                bienve.ShowDialog();
                LoginCaritas login = new LoginCaritas();
                login.Show();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconButton9_Click(object sender, EventArgs e)
        {
            //botonActivo(sender, Color.FromArgb(0, 149, 135));
            abrirformulario(new CambiarCargo(), sender);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void iconButton10_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorBoton(new Formato_Entrada());
            DesactivarBoton();
        }

        private void iconButton11_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorBoton(new FormatoSalida());
            DesactivarBoton();
        }

        private void iconButton12_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorBoton(new CRUDAliados());
            DesactivarBoton();
        }

        private void iconButton13_Click(object sender, EventArgs e)
        {
            AbrirFormularioPorBoton(new inventario());
            DesactivarBoton();
        }
        private void AbrirFormularioPorBoton(Form nuevoform)
        {
            if (Cerrar == true)
            {
                abrirformulario(nuevoform, iconButton1);
            }
            else
            {
                if (MessageBox.Show("Aun no se guarda la información actual,\n ¿Deseas Salir sin guardar?", "Salir", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    abrirformulario(nuevoform, iconButton1);
                }
            }
        }
    }
}
