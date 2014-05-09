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
    public partial class DataViewForm : Form
    {
        DataTable fkkvs;
        int[,] changeMask;
        public DataViewForm(DataTable fkkvs, int[,] changeMask)
        {
            InitializeComponent();
            this.fkkvs = fkkvs;
            this.changeMask = changeMask;
        }

        private void DataViewForm_Load(object sender, EventArgs e)
        {
            fkkvsView.DataSource = fkkvs;
            fkkvsView.AutoResizeColumns();
            for (int i = 0; i < changeMask.GetLength(0); i++)
            {
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
        }
    }
}
