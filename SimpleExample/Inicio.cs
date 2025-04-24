using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleExample
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void mercadrona_Click(object sender, EventArgs e)
        {
            Mercadrona mercadrona = new Mercadrona();
            mercadrona.Show();
        }

        private void simulación_Click(object sender, EventArgs e)
        {
            simpleexample simulacion = new simpleexample();
            simulacion.Show();
        }
    }
}

