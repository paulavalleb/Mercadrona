using csDronLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace SimpleExample
{
    public partial class Parametros : Form
    {
        Dron dron;
        public Parametros(Dron dron)
        {
            InitializeComponent();
            this.dron = dron;
        }

        private void Leer_parametros ()
        {

            List<string> list = new List<string>();
            list.Add("RTL_ALT");
            list.Add("FENCE_ACTION");
            list.Add("FENCE_ENABLE");
            list.Add("FENCE_ALT_MAX");
            List<float> resultado = dron.LeerParametros(list);
            float altura = resultado[0];
            RTL_ALT_lbl.Text = altura.ToString();
            if (altura < 10)
                trackBar1.Value = (int) altura;

            int action = Convert.ToInt32(resultado[1]);
            if (action == 1)
                RTL_radio.Checked = true;
            else if (action == 2)
                Land_radio.Checked = true;
            else if (action == 4)
                Brake_radio.Checked = true;

            int enable = Convert.ToInt32(resultado[2]);
            if (enable == 1)
            {
                enableBtn.Text = "SI";
                enableBtn.BackColor = Color.Green;
                enableBtn.ForeColor = Color.White;
            }
            else
            {
                enableBtn.Text = "NO";
                enableBtn.BackColor = Color.Orange;
                enableBtn.ForeColor = Color.Black;
            }
            ALT_MAX_Box.Text = resultado[3].ToString();

        }

        private void Paramatros_Load(object sender, EventArgs e)
        {
            Leer_parametros();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Leer_parametros();

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int n = trackBar1.Value;

            RTL_ALT_lbl.Text = n.ToString();
        }
     

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void enableBtn_Click(object sender, EventArgs e)
        {
            if (enableBtn.Text == "NO")
            {
                enableBtn.Text = "SI";
                enableBtn.BackColor = Color.Green;
                enableBtn.ForeColor = Color.White;
            }
            else
            {
                enableBtn.Text = "NO";
                enableBtn.BackColor = Color.Orange;
                enableBtn.ForeColor = Color.Black;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<(string parametro, float valor)> parametros = new List < (string parametro, float valor) > ();
            double v = Convert.ToDouble(RTL_ALT_lbl.Text);
            parametros.Add(("RTL_ALT", (float) v));
            int action = 0;
            if (RTL_radio.Checked)
                action = 1;
            else if (Land_radio.Checked)
                action = 2;
            else if (Brake_radio.Checked)
                action = 4;
            parametros.Add(("FENCE_ACTION", (float) action));
            int enable = 0;
            if (enableBtn.Text == "SI")
                enable = 1;
        
            parametros.Add(("FENCE_ENABLE", (float)enable));
            v = Convert.ToDouble(ALT_MAX_Box.Text);
            parametros.Add(("FENCE_ALT_MAX", (float)v));

            dron.EscribirParametros(parametros);
        }
    }
}
