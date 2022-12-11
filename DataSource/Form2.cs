using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dotnetCHARTING.WinForms;

namespace DataSource
{
    public partial class Form2 : Form
    {
        DataTable orders;
        public Form2(DataTable orders )
        {
            InitializeComponent();
            this.orders = orders;
            Chart chart = new Chart();  
            chart.Dock = DockStyle.Fill;
            chart.Use3D = true;
            this.Controls.Add(chart);
            chart.Type = ChartType.Combo;

            chart.ContextMenuStrip = contextMenuStrip1;

            foreach(DataRow dr in orders.Rows)
            {
                Element element = new Element(dr["Customer"].ToString() + " - " + ((DateTime)dr["OrderDate"]).ToString("yy"), double.Parse(dr["Quantity"].ToString())); 
                element.Annotation = new Annotation(dr["Quantity"].ToString());
                ElementCollection elementCollection = new ElementCollection();  
                elementCollection.Add(element);
                Series serie = new Series(dr["Customer"].ToString() + " - " + ((DateTime)dr["OrderDate"]).ToString("yy"),elementCollection);
                chart.SeriesCollection.Add(serie); 

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
