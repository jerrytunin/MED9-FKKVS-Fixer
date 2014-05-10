using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FKKVSFixer
{
    /// <summary>
    /// Form to display FKKVS with changed areas
    /// </summary>
    public partial class DataViewForm : Form
    {
        DataTable fkkvs;
        int[,] changeMask;
        double[] pwAxis;
        public DataViewForm(DataTable fkkvs, int[,] changeMask, double[] pwAxis)
        {
            InitializeComponent();
            this.fkkvs = fkkvs;
            this.changeMask = changeMask;
            this.pwAxis = pwAxis;
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {
            fkkvsView.DataSource = fkkvs;
            //Parse change mask to color cells and show increases/decreases
            for (int i = 0; i < changeMask.GetLength(0); i++)
            {
                fkkvsView.Rows[i].HeaderCell.Value = pwAxis[i].ToString("#.##");
                for (int j = 0; j < changeMask.GetLength(1); j++)
                {
                    if (changeMask[i, j] > 0)
                    {
                        fkkvsView.Rows[i].Cells[j].Style.BackColor = Color.Red;
                    }
                    else if (changeMask[i, j] < 0)
                    {
                        fkkvsView.Rows[i].Cells[j].Style.BackColor = Color.Blue;
                    }
                }
            }
            fkkvsView.AutoResizeColumns();
            fkkvsView.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
        }
    }
}
