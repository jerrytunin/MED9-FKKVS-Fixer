using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FKKVSFixer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmdLog_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Log CSV Files (*.csv)|*.csv";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLog.Text = ofd.FileName;
            }
        }

        private void cmdProcess_Click(object sender, EventArgs e)
        {
            if (txtLog.Text.Equals(""))
                return;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string saveFile = sfd.FileName;
            //RPM, PW, Correction
            string[,] logFile = processCSV(txtLog.Text);
            LogDataItem[] logData = new LogDataItem[logFile.GetLength(0) - 1];
            for (int i = 0; i < logData.Length; i++)
			{
                logData[i] = new LogDataItem(Double.Parse(logFile[i + 1, 0]), Double.Parse(logFile[i + 1, 1]), Double.Parse(logFile[i + 1, 2]));
			}
            string[,] fkkvsFile = processCSV(txtFKKVS.Text);
            FKKVSMap fkkvs = new FKKVSMap(fkkvsFile);
            List<LogDataItem>[] rpmVals = new List<LogDataItem>[16];
            List<LogDataItem>[] pwVals = new List<LogDataItem>[16];
            for (int i = 0; i < rpmVals.Length; i++)
            {
                rpmVals[i] = new List<LogDataItem>();
                pwVals[i] = new List<LogDataItem>();
            }

            for (int i = 0; i < logData.Length; i++)
            {
                for (int j = 0; j < fkkvs.lowerRPM.Length; j++)
                {
                    LogDataItem l = logData[i];
                    if (l.rpm >= fkkvs.lowerRPM[j] && l.rpm < fkkvs.upperRPM[j])
                        rpmVals[j].Add(l);
                    if (l.pw >= fkkvs.lowerPW[j] && l.pw < fkkvs.upperPW[j])
                        pwVals[j].Add(l);
                }
            }
            for (int i = 0; i < rpmVals.Length; i++)
            {
                int x = i;
                int y = -1;
                foreach (LogDataItem l in rpmVals[i])
                {
                    for (int j = 0; j < pwVals.Length; j++)
                    {
                        if (pwVals[j].Contains(l))
                            y = j;
                    }
                    if (y < 0)
                        continue;
                    fkkvs.mapData[x, y] = (fkkvs.mapData[x, y] + l.correction) / 2;
                }
            }
            string output = "";
            for (int i = 0; i < 17; i++)
            {
                if (i >= 16)
                {
                    output += fkkvsFile[0, i];
                }
                else
                {
                    output += fkkvsFile[0, i] + ",";
                }
            }
            output += "\r\n";
            for (int i = 0; i < fkkvs.mapData.GetLength(0); i++)
            {
                output += fkkvsFile[i+1, 0] + ",";
                for (int j = 0; j < fkkvs.mapData.GetLength(1); j++)
                {
                    if (j >= 15)
                    {
                        output += fkkvs.mapData[i, j].ToString();
                    }
                    else
                    {
                        output += fkkvs.mapData[i, j].ToString() + ",";
                    }
                }
                output += "\r\n";
            }
            File.WriteAllText(saveFile, output);
            updateFKKVSView(output, fkkvs);
        }

        private string[,] processCSV(string fileName)
        {
            string lf = File.ReadAllText(fileName);
            return processCSVFromString(lf);
        }

        private string[,] processCSVFromString(string data)
        {
            string[] rows = Regex.Split(data, "\r\n|\r|\n");
            int numRows = rows.Length;
            int numCols = rows[0].Split(',').Length;
            string[,] log = new string[numRows, numCols];
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCols; j++)
                {
                    if (rows[i].Split(',').Length > 1)
                    {
                        log[i, j] = rows[i].Split(',')[j];
                    }
                }
            }
            return log;
        }

        private void cmdChooseFKKVS_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "FKKVS CSV File (*.csv)|*.csv";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFKKVS.Text = ofd.FileName;
            }
        }

        private void updateFKKVSView(string dIn, FKKVSMap fkkvs)
        {
            string[,] data = processCSVFromString(dIn);
            int[,] changeMask = new int[17, 17];
            DataTable d = new DataTable();

            for (int i = 0; i < 17; i++)
            {
                d.Columns.Add();
            }

            int rowCount = data.GetLength(0) - 1;
            int rowLength = data.GetLength(1);

            for (int i = 1; i < changeMask.GetLength(0); i++)
            {
                for (int j = 1; j < changeMask.GetLength(1); j++)
                {
                    if (fkkvs.origData[i - 1, j - 1] > fkkvs.mapData[i - 1, j - 1])
                    {
                        changeMask[i, j] = -1;
                    }
                    else if (fkkvs.origData[i - 1, j - 1] < fkkvs.mapData[i - 1, j - 1])
                    {
                        changeMask[i, j] = 1;
                    }
                }
            }

            for (int j = 0; j < rowCount; j++)
            {
                // create a DataRow using .NewRow()
                DataRow row = d.NewRow();

                // iterate over all columns to fill the row
                for (int i = 0; i < rowLength; i++)
                {
                    row[i] = Double.Parse(data[j, i]).ToString("#.######");
                }

                // add the current row to the DataTable
                d.Rows.Add(row);
            }
            DataViewForm f = new DataViewForm(d, changeMask);
            f.ShowDialog();
        }
    }
}
