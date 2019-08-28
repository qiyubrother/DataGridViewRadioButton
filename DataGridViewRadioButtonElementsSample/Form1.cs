using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DataGridViewRadioButtonElements;

namespace DataGridViewRadioButtonElementsSample
{
    public partial class Form1 : Form
    {
        private List<City> cities = new List<City>(); 

        public Form1()
        {
            InitializeComponent();
            ((DataGridViewRadioButtonColumn)this.dataGridView1.Columns[0]).NotifyItemsCollectionChanged();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cities.Add(new City("Bellevue", 98004));
            cities.Add(new City("Bothell", 98011));
            cities.Add(new City("Edmonds", 98020));
            cities.Add(new City("Kirkland", 98033));
            cities.Add(new City("Redmond", 98052));
            cities.Add(new City("Seattle", 98101));
            cities.Add(new City("Woodinville", 98077));
            ((DataGridViewRadioButtonColumn)this.dataGridView1.Columns[1]).DataSource = cities;

            this.dataGridView1.Rows.Add("Orange", 98004);
            this.dataGridView1.Rows.Add("Yellow", 98011);
            this.dataGridView1.AutoResizeRows();
        }
    }

    public class City
    {
        private string name;
        private int zipCode;

        public City(string name, int zipCode)
        {
            this.name = name;
            this.zipCode = zipCode;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public int ZipCode
        {
            get
            {
                return this.zipCode;
            }
            set
            {
                this.zipCode = value;
            }
        }
    }
}