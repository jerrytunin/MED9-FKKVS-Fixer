using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FKKVSFixer
{
    public class FKKVSMap
    {
        public const double MAXRPM = 10000;
        public const double MAXPW = 24;
        public double[] lowerRPM { get; set; }
        public double[] upperRPM { get; set; }
        public double[] lowerPW { get; set; }
        public double[] upperPW { get; set; }

        public double[,] mapData { get; set; }
        public double[,] origData { get; set; }

        public double[,] correctionSumMap { get; set; }
        public int[,] numCorrections { get; set; }

        public double[] rpmAxis { get; set; }
        public double[] pwAxis { get; set; }

        public FKKVSMap(string[,] fullMapData)
        {
            lowerRPM = new double[16];
            upperRPM = new double[16];
            lowerPW = new double[16];
            upperPW = new double[16];
            mapData = new double[16, 16];
            origData = new double[16, 16];
            correctionSumMap = new double[16, 16];
            numCorrections = new int[16, 16];
            rpmAxis = new double[16];
            pwAxis = new double[16];
            parseMapData(fullMapData);
        }

        private void parseMapData(string[,] fullMapData)
        {
            if (fullMapData.GetLength(0) != 17 || fullMapData.GetLength(1) != 17)
            {
                throw new Exception();
            }
            //Calculate lower and upper range for RPM and PW values
            for (int i = 0; i < 16;)
            {
                lowerRPM[i] = (Double.Parse(fullMapData[0, i]) + Double.Parse(fullMapData[0, i + 1])) / 2;
                lowerPW[i] = (Double.Parse(fullMapData[i, 0]) + Double.Parse(fullMapData[i + 1, 0])) / 2;
                upperRPM[i++] = i >= 16 ? (Double.Parse(fullMapData[0, i]) + MAXRPM) / 2 : (Double.Parse(fullMapData[0, i]) + Double.Parse(fullMapData[0, i + 1])) / 2;
                upperPW[i - 1] = i >= 16 ? (Double.Parse(fullMapData[i, 0]) + MAXPW) / 2 : (Double.Parse(fullMapData[i, 0]) + Double.Parse(fullMapData[i + 1, 0])) / 2;
            }
            //Set axes arrays and parse map data into 2D double arrays
            for (int i = 1; i < 17; i++)
            {
                rpmAxis[i-1] = Double.Parse(fullMapData[0, i]);
                pwAxis[i-1] = Double.Parse(fullMapData[i, 0]);
                for (int j = 1; j < 17; j++)
                {
                    mapData[i - 1, j - 1] = Double.Parse(fullMapData[i, j]);
                    origData[i - 1, j - 1] = Double.Parse(fullMapData[i, j]);
                }
            }
        }

    }
}
