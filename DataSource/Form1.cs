using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataSource
{
    public partial class Form1 : Form
    {
        DataLayer datalayer;
        public Form1()
        {
            InitializeComponent();
            datalayer = new DataLayer(@"JOUD\INSTANCE_2K19_01", "Northwind");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if((txt_Command.Text.Length> 0))
            {
                txt_value.Text = (datalayer.GetValue(txt_Command.Text)).ToString();
            }
        }
        DataTable CategoriesDt, ProductsDt, OrdersDt;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (datalayer.Valid)
            {
                CategoriesDt = datalayer.GetData("SELECT CategoryId,CategoryName From Categories", "categories");
                categories.DataSource = CategoriesDt;
                categories.DisplayMember = "CategoryName";
                categories.ValueMember = "CategoryId";
                categories.SelectedIndexChanged += Categories_SelectedIndexChanged;
            }
        }

        bool IsProductsFilled = false;

        private void button2_Click(object sender, EventArgs e)
        {
            if (OrdersDt != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "XML Files | *.xml";
                saveFileDialog.DefaultExt = "xml";

                if(saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    OrdersDt.WriteXml(saveFileDialog.FileName);

                }
            }
        }

        private void products_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txt_value_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (OrdersDt != null)
            {
                Form2 frm = new Form2(OrdersDt);
                frm.Show();
            }
        }

        private void Categories_SelectedIndexChanged(object? sender, EventArgs e)
        {
            ProductsDt = datalayer.GetData("SELECT ProductId,ProductName FROM Products WHERE CategoryId = " + categories.SelectedValue.ToString(), "Products");
            products.DataSource = ProductsDt;
            products.DisplayMember = "ProductName";
            products.ValueMember = "ProductId";

            if (!IsProductsFilled)
            {
                products.SelectedIndexChanged += Products_SelectedIndexChanged;
                IsProductsFilled = true;
            }
        }

        private void Products_SelectedIndexChanged(object? sender, EventArgs e)
        {
            OrdersDt = datalayer.GetData("SELECT OrderId,UnitPrice,Qunatity,Discount FROM [Order Details] WHERE ProductId = " + products.SelectedValue.ToString(), "Orders");
            DataColumn dc = new DataColumn("OrderDate", typeof(DateTime));
            OrdersDt.Columns.Add(dc);
            dc = new DataColumn("Customer", typeof(string));
            OrdersDt.Columns.Add(dc);

            foreach (DataRow dr in OrdersDt.Rows)
            {
                string oId = dr["orderId"].ToString();
                DataTable dt = datalayer.GetData("SELECT CustomerId,OrderDate FROM OrderId = " + oId, "data");
                dr["OrderDate"] = dt.Rows[0]["OrderDate"];
                string cid = dt.Rows[0]["CustomerId"].ToString();
                string CustomerName = datalayer.GetValue("SELECT CompanyName FROM Customers WHERE CustomerId = '" + cid + "'").ToString();
                dr["customer"] = CustomerName;
            }
            orders.DataSource = OrdersDt;
        }
    }
}
