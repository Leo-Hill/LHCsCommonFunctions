using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common excel interop functions
    * 
    **********************************************************************************************/
    public static class LHExcelFunctions
    {
        //This function combines multiple excel files to one file separated as sheets
        public static void vCombineExcelFiles(String[] qasFilePathsSource, String qsFilePathDestination)
        {
            //Excel objects
            Excel.Application ExcelApp;
            Excel.Workbook ExcelWorkBookSource = null, ExcelWorkBookDestination = null;
            Excel.Worksheet ExcelWorkSheetSource = null, ExcelWorkSheetDestination = null;

            //Primitive
            int iDestinationSheetCnt = 0;                                                           //Counter for the destination sheet

            ExcelApp = new Excel.Application();                                                     //Create a new excel application
            ExcelApp.DisplayAlerts = false;
            ExcelWorkBookDestination = ExcelApp.Workbooks.Add();                                    //Create a new workbook

            foreach (String actFilePath in qasFilePathsSource)
            {
                if (File.Exists(actFilePath))
                {
                    ExcelWorkBookSource = ExcelApp.Workbooks.Open(actFilePath);
                    for (int iSourceSheetCnt = 1; iSourceSheetCnt <= ExcelWorkBookSource.Worksheets.Count; iSourceSheetCnt++)
                    {
                        iDestinationSheetCnt++;
                        ExcelWorkSheetSource = (Excel.Worksheet)ExcelWorkBookSource.Worksheets[iSourceSheetCnt];
                        ExcelWorkSheetSource.Copy(ExcelWorkBookDestination.Worksheets[iDestinationSheetCnt]);
                    }
                }
            }
            ExcelWorkBookDestination.Activate();
            ExcelWorkSheetDestination = ExcelWorkBookDestination.Worksheets[iDestinationSheetCnt + 1];
            ExcelWorkSheetDestination.Delete();

            //Save the excel file
            ExcelWorkBookDestination.SaveAs(qsFilePathDestination, Excel.XlFileFormat.xlWorkbookDefault);
            ExcelWorkBookDestination.Close(true);
            ExcelWorkBookSource.Close(false);
            ExcelApp.Quit();

            //Clean up excel objects
            Marshal.ReleaseComObject(ExcelWorkBookSource);
            Marshal.ReleaseComObject(ExcelWorkBookDestination);
            Marshal.ReleaseComObject(ExcelWorkSheetSource);
            Marshal.ReleaseComObject(ExcelWorkSheetDestination);
            Marshal.ReleaseComObject(ExcelApp);
        }

        //This function styles a line chart
        public static void vStyleLineChart(Excel.ChartObject qExcelChartObject, DateTime qDTMinTimestamp, DateTime qDTMaxTimestamp)
        {
            //Excel objects
            Excel.Chart ExcelChart;
            Excel.Axis ExcelAxisX, ExcelAxisY;

            ExcelChart = qExcelChartObject.Chart;                                                   //Chart of the chart object

            //General chart settings
            ExcelChart.ChartType = Excel.XlChartType.xlXYScatterSmoothNoMarkers;                    //Chart type
            ExcelChart.DisplayBlanksAs = Excel.XlDisplayBlanksAs.xlInterpolated;                    //Interpolate empty cells
            ExcelChart.HasTitle = false;
            //Size of the chart
            qExcelChartObject.Width = LHCalculationFunctions.CmToPt(25);
            qExcelChartObject.Height = LHCalculationFunctions.CmToPt(15);

            //Plot area settings
            //Size and position of the plot area
            ExcelChart.PlotArea.Width = LHCalculationFunctions.CmToPt(20);
            ExcelChart.PlotArea.Height = LHCalculationFunctions.CmToPt(14);
            ExcelChart.PlotArea.Left = LHCalculationFunctions.CmToPt(0.5);
            ExcelChart.PlotArea.Top = LHCalculationFunctions.CmToPt(0.5);
            //Plot area border
            ExcelChart.PlotArea.Border.Color = Color.Black;
            ExcelChart.PlotArea.Border.Weight = Excel.XlBorderWeight.xlMedium;
            //Legend
            ExcelChart.Legend.Left = LHCalculationFunctions.CmToPt(20);
            ExcelChart.Legend.Top = LHCalculationFunctions.CmToPt(1);
            ExcelChart.Legend.Width = LHCalculationFunctions.CmToPt(4);
            ExcelChart.Legend.Height = LHCalculationFunctions.CmToPt(12);
            ExcelChart.Legend.Font.Size = 12;
            ExcelChart.Legend.Font.FontStyle = "Bold";

            //Axis settings
            ExcelAxisX = (Excel.Axis)ExcelChart.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary);
            ExcelAxisY = (Excel.Axis)ExcelChart.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary);
            //FontSize
            ExcelAxisX.TickLabels.Font.Size = 14;
            ExcelAxisY.TickLabels.Font.Size = 14;
            ExcelAxisX.TickLabels.Font.FontStyle = "Bold";
            ExcelAxisY.TickLabels.Font.FontStyle = "Bold";
            ExcelAxisX.MajorTickMark = Excel.XlTickMark.xlTickMarkNone;
            ExcelAxisY.MajorTickMark = Excel.XlTickMark.xlTickMarkNone;
            //Axis range
            //Axis-X
            ExcelAxisX.TickLabels.NumberFormat = "hh:mm";
            //Axis-X Min
            DateTime DTAxisXMin = qDTMinTimestamp;                                                  //Minumum timestanp of the values
            DTAxisXMin = DTAxisXMin.AddMinutes(-DTAxisXMin.Minute);                                 //Set the minute to 0
            DTAxisXMin = DTAxisXMin.AddSeconds(-DTAxisXMin.Second);                                 //Set the minute to 0
            DTAxisXMin = DTAxisXMin.AddMilliseconds(-DTAxisXMin.Millisecond);                       //Set the milliseconds to 0
            ExcelAxisX.MinimumScale = DTAxisXMin.ToOADate();
            //Axis-X Max
            DateTime DTAxisXMax = qDTMaxTimestamp;                                                  //Maximum timestamp of the values
            DTAxisXMax = DTAxisXMax.AddHours(1);                                                    //Set the hour to the next hour
            DTAxisXMax = DTAxisXMax.AddMinutes(-DTAxisXMax.Minute);                                 //Set the minute to 0
            DTAxisXMax = DTAxisXMax.AddSeconds(-DTAxisXMax.Second);                                 //Set the minute to 0
            DTAxisXMax = DTAxisXMax.AddMilliseconds(-DTAxisXMax.Millisecond);                       //Set the milliseconds to 0
            ExcelAxisX.MaximumScale = DTAxisXMax.ToOADate();

            const int iNumOfXMajorUnits = 4;
            ExcelAxisX.MajorUnit = (DTAxisXMax.ToOADate() - DTAxisXMin.ToOADate()) / iNumOfXMajorUnits;


            //Axis-Y Max
            double dAxisYMax = 0;
            if (ExcelAxisY.MaximumScale > 0)
            {
                dAxisYMax = ExcelAxisY.MaximumScale;
                double dAxisYMaxPow = Math.Floor(Math.Log10(dAxisYMax));                                //Get the pow of 10 of the maximum value
                dAxisYMax = Math.Floor(dAxisYMax / Math.Pow(10, dAxisYMaxPow));                         //Get the first digit of the max value
                dAxisYMax = (dAxisYMax + 1) * Math.Pow(10, dAxisYMaxPow);                               //Calculate the new Y-Max value
            }

            //Axis-Y Min
            double dAxisYMin = 0;
            if (ExcelAxisY.MinimumScale < 0)
            {
                dAxisYMin = -ExcelAxisY.MinimumScale;
                double dAxisYMinPow = Math.Floor(Math.Log10(dAxisYMin));                                //Get the pow of 10 of the minimum value
                dAxisYMin = Math.Floor(dAxisYMin / Math.Pow(10, dAxisYMinPow));                         //Get the first digit of the max value
                dAxisYMin = -(dAxisYMin + 1) * Math.Pow(10, dAxisYMinPow);                               //Calculate the new Y-Max value
            }

            //There are the following scenarios for scaling
            //1) and 2) If there are only positive (negative) values we want the YMin (YMax) to be 0. These cases are already handled above
            //3) If we have negative and positive values, we want the absolute of YMin and YMax to be equal
            double dMaxAbsoluteY = Math.Max(Math.Abs(dAxisYMax), Math.Abs(dAxisYMin));
            if (dAxisYMin != 0 && dAxisYMax != 0)
            {
                dAxisYMax = dMaxAbsoluteY;
                dAxisYMin = -dMaxAbsoluteY;
            }


            ExcelAxisY.MinimumScale = dAxisYMin;
            ExcelAxisY.MaximumScale = dAxisYMax;
            ExcelAxisY.Crosses = Excel.XlAxisCrosses.xlAxisCrossesCustom;
            ExcelAxisY.CrossesAt = ExcelAxisY.MinimumScale;

            const int iNumOfYMajorUnits = 4;
            ExcelAxisY.MajorUnit = (ExcelAxisY.MaximumScale - ExcelAxisY.MinimumScale) / iNumOfYMajorUnits;


            //We want to prevent the Y major tick labels showing the same value twice, so in case the max/min value is smaller than the major units, 
            //we show an additional decimal.
            double dMinFirstDigitWithoutExtraDecimals = iNumOfYMajorUnits;
            if (dAxisYMin != 0 && dAxisYMax != 0)
            {
                dMinFirstDigitWithoutExtraDecimals /= 2;
            }

            double dFirstDigitOfAbsoluteMax = Math.Floor(dMaxAbsoluteY / Math.Pow(10, Math.Floor(Math.Log10(dMaxAbsoluteY))));
            int iNumOfDecimals = 0;
            if (dFirstDigitOfAbsoluteMax < dMinFirstDigitWithoutExtraDecimals && dMaxAbsoluteY < 10)
            {
                iNumOfDecimals++;
            }

            //Show decimals in case the max value is below 1
            if (dMaxAbsoluteY < 1)
            {
                iNumOfDecimals += -(int)Math.Floor(Math.Log10(dMaxAbsoluteY));
                iNumOfDecimals = Math.Max(1, iNumOfDecimals);
            }

            if (iNumOfDecimals > 0)
            {
                ExcelAxisY.TickLabels.NumberFormat = "0,";
                for (; iNumOfDecimals > 0; iNumOfDecimals--)
                {
                    ExcelAxisY.TickLabels.NumberFormat += "0";
                }
            }
            else
            {
                ExcelAxisY.TickLabels.NumberFormat = "0";
            }

            //Grid
            //Major X
            ExcelAxisX.HasMajorGridlines = true;
            ExcelAxisX.MajorGridlines.Border.Color = Color.Black;
            ExcelAxisX.MajorGridlines.Border.LineStyle = Excel.XlLineStyle.xlDash;
            ExcelAxisX.MajorGridlines.Border.Weight = Excel.XlBorderWeight.xlHairline;
            //Major Y
            ExcelAxisY.HasMajorGridlines = true;
            ExcelAxisY.MajorGridlines.Border.Color = Color.Black;
            ExcelAxisY.MajorGridlines.Border.LineStyle = Excel.XlLineStyle.xlDash;
            ExcelAxisY.MajorGridlines.Border.Weight = Excel.XlBorderWeight.xlHairline;

            //Series settings
            foreach (Excel.Series series in ExcelChart.FullSeriesCollection())                      //Loop through all series
            {
                series.Format.Line.Weight = 2.25f;
            }


            //Clean up excel objects
            Marshal.ReleaseComObject(ExcelAxisX);
            Marshal.ReleaseComObject(ExcelAxisY);
            Marshal.ReleaseComObject(ExcelChart);
        }
    }
}
