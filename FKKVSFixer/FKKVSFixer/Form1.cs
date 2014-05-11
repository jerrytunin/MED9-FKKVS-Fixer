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
    /// <summary>
    /// Form to accept CSV inputs of log file and FKKVS and apply corrections and output CSV and graphical representation.
    /// </summary>
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
        /// <summary>
        /// Process the CSV file and generate output
        /// </summary>
        private void cmdProcess_Click(object sender, EventArgs e)
        {
            if (txtLog.Text.Equals(""))
                return;
            
            string[,] logFile = processCSV(txtLog.Text);
            LogDataItem[] logData = new LogDataItem[logFile.GetLength(0) - 1];
            int rpmCol = -1;
            int pwCol = -1;
            int corCol = -1;
            //Auto determine the location of RPM, PW, and corrections
            for (int i = 0; i < logFile.GetLength(1); i++)
            {
                if (logFile[0, i].Contains("nmot"))
                {
                    rpmCol = i;
                }
                else if (logFile[0, i].Contains("te"))
                {
                    pwCol = i;
                }
                else if (logFile[0, i].Contains("fr"))
                {
                    corCol = i;
                }
            }
            if (rpmCol == -1 || pwCol == -1 || corCol == -1)
            {
                MessageBox.Show("Unable to detect correct parameters in log file. Please try again with a different log.", "Log Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV Files (*.csv)|*.csv";
            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            string saveFile = sfd.FileName;
            //Assign data to 2D array and parse to double
            for (int i = 0; i < logData.Length; i++)
			{
                logData[i] = new LogDataItem(Double.Parse(logFile[i + 1, rpmCol]), Double.Parse(logFile[i + 1, pwCol]), Double.Parse(logFile[i + 1, corCol]));
			}
            string[,] fkkvsFile = processCSV(txtFKKVS.Text);
            FKKVSMap fkkvs = new FKKVSMap(fkkvsFile);
            //Create axis holders for log data values to be deposited into
            List<LogDataItem>[] rpmVals = new List<LogDataItem>[16];
            List<LogDataItem>[] pwVals = new List<LogDataItem>[16];
            for (int i = 0; i < rpmVals.Length; i++)
            {
                rpmVals[i] = new List<LogDataItem>();
                pwVals[i] = new List<LogDataItem>();
            }
            //Determine which column and row the LogDataItem should be in
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
            //Update the FKKVS mapData for the values at the specific RPM and PW values
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
                    fkkvs.correctionSumMap[x, y] += l.correction;
                    fkkvs.numCorrections[x, y]++;
                }
            }

            for (int i = 0; i < fkkvs.mapData.GetLength(0); i++)
            {
                for (int j = 0; j < fkkvs.mapData.GetLength(1); j++)
                {
                    if (fkkvs.numCorrections[i, j] > 0)
                    {
                        double avgCorrectionCell = fkkvs.correctionSumMap[i, j] / fkkvs.numCorrections[i, j];
                        fkkvs.mapData[i, j] *= avgCorrectionCell;
                    }
                }
            }

            //Create CSV Output
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
            //Display FKKVS Map
            updateFKKVSView(output, fkkvs);
        }
        /// <summary>
        /// Parse a CSV from a file
        /// </summary>
        /// <param name="fileName">The file to process</param>
        /// <returns>A 2D array containing the parsed CSV</returns>
        private string[,] processCSV(string fileName)
        {
            string lf = File.ReadAllText(fileName);
            return processCSVFromString(lf);
        }

        /// <summary>
        /// Parse a CSV from a string
        /// </summary>
        /// <param name="data">The string containing the CSV data</param>
        /// <returns>A 2D array containing the parsed CSV</returns>
        private string[,] processCSVFromString(string data)
        {
            data = data.TrimEnd('\r', '\n');
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

        /// <summary>
        /// Update the fkkvs view and display form
        /// </summary>
        /// <param name="dIn">The updated fkkvs data</param>
        /// <param name="fkkvs">The FKKVS map</param>
        private void updateFKKVSView(string dIn, FKKVSMap fkkvs)
        {
            string[,] data = processCSVFromString(dIn);
            int[,] changeMask = new int[16, 16];
            DataTable d = new DataTable();
            //Create and label columns with RPM values
            for (int i = 0; i < 16; i++)
            {
                d.Columns.Add(fkkvs.rpmAxis[i].ToString());
            }

            int rowCount = data.GetLength(0);
            int rowLength = data.GetLength(1);
            //Calculate change mask for coloring of cells
            for (int i = 0; i < changeMask.GetLength(0); i++)
            {
                for (int j = 0; j < changeMask.GetLength(1); j++)
                {
                    if (fkkvs.origData[i, j] > fkkvs.mapData[i, j])
                    {
                        changeMask[i, j] = -1;
                    }
                    else if (fkkvs.origData[i, j] < fkkvs.mapData[i, j])
                    {
                        changeMask[i, j] = 1;
                    }
                }
            }

            //Parse data from the updated map to doubles and assign to cells
            for (int i = 1; i < rowCount; i++)
            {
                DataRow row = d.NewRow();
                
                for (int j = 1; j < rowLength; j++)
                {
                    row[j-1] = Double.Parse(data[i, j]).ToString("#.######");
                }

                d.Rows.Add(row);
            }
            DataViewForm f = new DataViewForm(d, changeMask, fkkvs.pwAxis);
            f.Text = "FKKVS";
            f.Show();
        }
    }
}
