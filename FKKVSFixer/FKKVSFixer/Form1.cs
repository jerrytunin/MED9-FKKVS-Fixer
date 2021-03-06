using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Utility.ModifyRegistry;

namespace FKKVSFixer
{
    /// <summary>
    /// Form to accept CSV inputs of log file and FKKVS and apply corrections and output CSV and graphical representation.
    /// </summary>
    public partial class Form1 : Form
    {

        private ModifyRegistry regedit;
        private readonly String APP_KEY = "Software\\" + Application.ProductName;
        private bool isAudiFile;

        CultureInfo culture;

        public Form1()
        {
            InitializeComponent();
            culture = CultureInfo.CreateSpecificCulture("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            isAudiFile = false;
            regedit = new ModifyRegistry();
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
            isAudiFile = false;
            setFormControlState(false);
            if (txtLog.Text.Equals(""))
                return;
            
            string[,] logFile = processCSV(txtLog.Text);

            LogDataItem[] logData = new LogDataItem[logFile.GetLength(0) - 1];
            int rpmCol = -1;
            int pwCol = -1;
            int corCol = -1;
            int wbafrCol = -1;
            int lamsbgCol = -1;
            int wpedCol = -1;

            bool narrowbandWOTCorrection = chkWBAFR.Checked;

            //Auto determine the location of RPM, PW, and corrections
            int dataBase = 0;
            int nameBase = 0;
            if (isAudiFile)
            {
                dataBase = 2;
                nameBase = 0;
                logData = new LogDataItem[logFile.GetLength(0) - 3];
            }
            for (int i = 0; i < logFile.GetLength(1); i++)
            {
                if (logFile[nameBase, i].Contains("nmot_w"))
                {
                    rpmCol = i;
                }
                else if (logFile[nameBase, i].Contains("nmot") && rpmCol == -1)
                {
                    rpmCol = i;
                }
                else if (logFile[nameBase, i].Contains("tevfakge_w") || logFile[nameBase, i].Contains("tevfa_w"))
                {
                    pwCol = i;
                }
                else if (logFile[nameBase, i].Contains("te_w") && pwCol == -1)
                {
                    pwCol = i;
                }
                else if (logFile[nameBase, i].Contains("fr_w"))
                {
                    corCol = i;
                }
                else if (logFile[nameBase, i].Contains("frm_w") && corCol == -1)
                {
                    corCol = i;
                }

                if (narrowbandWOTCorrection)
                {
                    if (logFile[nameBase, i].Contains("WBAFR"))
                    {
                        wbafrCol = i;
                    }
                    else if (logFile[nameBase, i].Contains("lamsbg_w"))
                    {
                        lamsbgCol = i;
                    }
                    else if (logFile[nameBase, i].Contains("wped_w"))
                    {
                        wpedCol = i;
                    }
                    else if (logFile[nameBase, i].Contains("wped") && wpedCol == -1)
                    {
                        wpedCol = i;
                    }
                }
                else
                {
                    wbafrCol = 0;
                    lamsbgCol = 0;
                    wpedCol = 0;
                }
            }

            if (rpmCol == -1 || pwCol == -1 || corCol == -1 || wbafrCol == -1 || lamsbgCol == -1 || wpedCol == -1)
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
                double wpedVal = Double.Parse(logFile[dataBase + i + 1, wpedCol]);
                if (narrowbandWOTCorrection && wpedVal > 80)
                {
                    double corrValue = 1;
                    double wbafrVal = Double.Parse(logFile[dataBase + i + 1, wbafrCol]);
                    double lamsbgAFRVal = Double.Parse(logFile[dataBase + i + 1, lamsbgCol]) * 14.7;

                    corrValue = 1 + ((wbafrVal - lamsbgAFRVal) / lamsbgAFRVal);

                    logData[i] = new LogDataItem(Double.Parse(logFile[dataBase + i + 1, rpmCol]), Double.Parse(logFile[dataBase + i + 1, pwCol]), corrValue);
                }
                else
                    logData[i] = new LogDataItem(Double.Parse(logFile[dataBase + i + 1, rpmCol]), Double.Parse(logFile[dataBase + i + 1, pwCol]), Double.Parse(logFile[dataBase + i + 1, corCol]));
			}
            isAudiFile = false;
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
                    if (l.correction != 1)
                    {
                        if (l.rpm >= fkkvs.lowerRPM[j] && l.rpm < fkkvs.upperRPM[j])
                            rpmVals[j].Add(l);
                        if (l.pw >= fkkvs.lowerPW[j] && l.pw < fkkvs.upperPW[j])
                            pwVals[j].Add(l);
                    }
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
                        fkkvs.mapData[j, i] *= avgCorrectionCell;
                    }
                }
            }

            for (int i = 0; i < numSmoothPasses.Value; i++)
            {
                fkkvs.mapData = mapSmoothing(fkkvs.mapData, (((double)trkSmoothing.Value) / 100.0));
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
            updateFKKVSView(fkkvs, chkDelta.Checked);
            setFormControlState(true);
        }

        private void setFormControlState(bool enabled)
        {
            chkWBAFR.Enabled = enabled;
            chkDelta.Enabled = enabled;
            cmdProcess.Enabled = enabled;
            cmdChooseFKKVS.Enabled = enabled;
            cmdLog.Enabled = enabled;
            numSmoothPasses.Enabled = enabled;
            trkSmoothing.Enabled = enabled;
        }

        /// <summary>
        /// Parse a CSV from a file
        /// </summary>
        /// <param name="fileName">The file to process</param>
        /// <returns>A 2D array containing the parsed CSV</returns>
        private string[,] processCSV(string fileName)
        {
            string lf = "";
                //lf = File.ReadAllText(fileName);
            using (var fs = new FileStream(fileName, FileMode.Open,
                FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(fs))
            {
                lf = sr.ReadToEnd();

                if (lf.Contains("ME7-Logger"))
                    isAudiFile = true;

                if (isAudiFile)
                {
                    lf = lf.Replace("\r", "");
                    int pos = lf.IndexOf("TimeStamp");
                    lf = lf.Substring(pos);
                }
            }
            return processCSVFromString(lf);
        }

        public static string deleteLines(string s, int linesToRemove)
        {
            return s.Split(Environment.NewLine.ToCharArray(),
                           linesToRemove + 1,
                           StringSplitOptions.RemoveEmptyEntries
                ).Skip(linesToRemove)
                .FirstOrDefault();
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
        private void updateFKKVSView(FKKVSMap fkkvs, bool delta)
        {
            int[,] changeMask = new int[16, 16];
            DataTable d = new DataTable();
            //Create and label columns with RPM values
            for (int i = 0; i < 16; i++)
            {
                d.Columns.Add(fkkvs.rpmAxis[i].ToString());
            }

            int rowCount = fkkvs.mapData.GetLength(0);
            int rowLength = fkkvs.mapData.GetLength(1);
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
            for (int i = 0; i < rowCount; i++)
            {
                DataRow row = d.NewRow();
                
                for (int j = 0; j < rowLength; j++)
                {
                    if (delta)
                        row[j] = (fkkvs.mapData[i, j] - fkkvs.origData[i, j]).ToString("0.000000");
                    else
                        row[j] = fkkvs.mapData[i, j].ToString("0.000000");
                }

                d.Rows.Add(row);
            }
            DataViewForm f = new DataViewForm(d, changeMask, fkkvs.pwAxis);
            f.Text = "FKKVS";
            f.Show();
        }


        private static double[,] mapSmoothing(double[,] initMapData, double smoothingFactor)
        {
            double[,] smoothedData = new double[16, 16];
            smoothingFactor = smoothingFactor > 1 ? 1 : smoothingFactor;
            smoothingFactor = smoothingFactor < 0 ? 0 : smoothingFactor;
            for (int i = 0; i < smoothedData.GetLength(0); i++)
            {
                for (int j = 0; j < smoothedData.GetLength(1); j++)
                {
                    bool leftCell = true;
                    bool rightCell = true;
                    bool upCell = true;
                    bool downCell = true;

                    if (i == 0)
                        upCell = false;
                    else if (i == smoothedData.GetLength(0) - 1)
                        downCell = false;

                    if (j == 0)
                        leftCell = false;
                    else if (j == smoothedData.GetLength(1) - 1)
                        rightCell = false;

                    if (rightCell && downCell && !upCell && !leftCell)
                    {
                        double rightCellVal = initMapData[i, j + 1];
                        double downCellVal = initMapData[i + 1, j];
                        double avg = (rightCellVal + downCellVal) / 2;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (rightCell && downCell && leftCell && !upCell)
                    {
                        double rightCellVal = initMapData[i, j + 1];
                        double downCellVal = initMapData[i + 1, j];
                        double leftCellVal = initMapData[i, j - 1];
                        double avg = (rightCellVal + downCellVal + leftCellVal) / 3;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (leftCell && downCell && !rightCell && !upCell)
                    {
                        double downCellVal = initMapData[i + 1, j];
                        double leftCellVal = initMapData[i, j - 1];
                        double avg = (downCellVal + leftCellVal) / 2;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (upCell && rightCell && downCell && !leftCell)
                    {
                        double rightCellVal = initMapData[i, j + 1];
                        double upCellVal = initMapData[i - 1, j];
                        double downCellVal = initMapData[i + 1, j];
                        double avg = (rightCellVal + upCellVal + downCellVal) / 3;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;

                    }
                    else if (upCell && leftCell && downCell && rightCell)
                    {
                        double leftCellVal = initMapData[i, j - 1];
                        double rightCellVal = initMapData[i, j + 1];
                        double upCellVal = initMapData[i - 1, j];
                        double downCellVal = initMapData[i + 1, j];
                        double avg = (leftCellVal + rightCellVal + upCellVal + downCellVal) / 4;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (upCell && leftCell && downCell && !rightCell)
                    {
                        double leftCellVal = initMapData[i, j - 1];
                        double upCellVal = initMapData[i - 1, j];
                        double downCellVal = initMapData[i + 1, j];
                        double avg = (leftCellVal + upCellVal + downCellVal) / 3;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (upCell && !leftCell && !downCell && rightCell)
                    {
                        double rightCellVal = initMapData[i, j + 1];
                        double upCellVal = initMapData[i - 1, j];
                        double avg = (rightCellVal + upCellVal) / 2;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (upCell && leftCell && !downCell && rightCell)
                    {
                        double leftCellVal = initMapData[i, j - 1];
                        double rightCellVal = initMapData[i, j + 1];
                        double upCellVal = initMapData[i - 1, j];
                        double avg = (leftCellVal + rightCellVal + upCellVal) / 3;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else if (upCell && leftCell && !downCell && !rightCell)
                    {
                        double leftCellVal = initMapData[i, j - 1];
                        double upCellVal = initMapData[i - 1, j];
                        double avg = (leftCellVal + upCellVal) / 2;
                        double origCellWeight = 2 - smoothingFactor;
                        smoothedData[i, j] = (initMapData[i, j] * origCellWeight + avg * smoothingFactor) / 2;
                    }
                    else
                    {
                        Console.WriteLine("You forgot something:");
                        Console.WriteLine("Up: " + upCell.ToString());
                        Console.WriteLine("Down: " + downCell.ToString());
                        Console.WriteLine("Left: " + leftCell.ToString());
                        Console.WriteLine("Right: " + rightCell.ToString());
                    }

                }
            }

            return smoothedData;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            regedit.BaseRegistryKey = Registry.CurrentUser;
            regedit.SubKey = APP_KEY;
            try
            {
                numSmoothPasses.Value = Int32.Parse(regedit.Read("smoothingPasses"));
                trkSmoothing.Value = Int32.Parse(regedit.Read("smoothingFactor"));
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            regedit.BaseRegistryKey = Registry.CurrentUser;
            regedit.SubKey = APP_KEY;
            try
            {
                regedit.Write("smoothingPasses", numSmoothPasses.Value.ToString());
                regedit.Write("smoothingFactor", trkSmoothing.Value.ToString());
            }
            catch (Exception ex)
            {

            }
        }
    }
}
