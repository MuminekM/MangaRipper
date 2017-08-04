using MangaRipper.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MangaRipper.Forms
{
    public partial class SearchResult : Form
    {
        public SearchResult()
        {
            InitializeComponent();
        }

        public void SetResult(IEnumerable<Site> sites)
        {/*
            var source = new BindingSource();
            source.DataSource = sites;
            dataGridView1.DataSource = source;*/
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("URL", "URL");
            dataGridView1.Columns.Add("Last", "Last");
            foreach (var site in sites){
                DataGridViewCell row = new DataGridViewCell(site.Titles);
                dataGridView1.Rows,Add(site.Titles).
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
