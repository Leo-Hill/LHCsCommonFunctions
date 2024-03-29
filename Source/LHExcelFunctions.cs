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
            qExcelChartObject.Width = LHCalculationFunctions.dCmToPt(25);
            qExcelChartObject.Height = LHCalculationFunctions.dCmToPt(15);

            //Plot area settings
            //Size and position of the plot area
            ExcelChart.PlotArea.Width = LHCalculationFunctions.dCmToPt(20);
            ExcelChart.PlotArea.Height = LHCalculationFunctions.dCmToPt(14);
            ExcelChart.PlotArea.Left = LHCalculationFunctions.dCmToPt(0.5);
            ExcelChart.PlotArea.Top = LHCalculationFunctions.dCmToPt(0.5);
            //Plot area border
            ExcelChart.PlotArea.Border.Color = Color.Black;
            ExcelChart.PlotArea.Border.Weight = Excel.XlBorderWeight.xlMedium;
            //Legend
            ExcelChart.Legend.Left = LHCalculationFunctions.dCmToPt(20);
            ExcelChart.Legend.Top = LHCalculationFunctions.dCmToPt(1);
            ExcelChart.Legend.Width = LHCalculationFunctions.dCmToPt(4);
            ExcelChart.Legend.Height = LHCalculationFunctions.dCmToPt(12);
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
            ExcelAxisX.MajorUnit = (DTAxisXMax.ToOADate() - DTAxisXMin.ToOADate()) / 4;

            //Axis-Y Min
            ExcelAxisY.MinimumScale = 0;
            //Axis-Y Max
            double dAxisYMax = ExcelAxisY.MaximumScale;
            double dAxisYMaxPow = Math.Floor(Math.Log10(dAxisYMax));                                //Get the pow of 10 of the maximum value
            dAxisYMax = Math.Floor(dAxisYMax / Math.Pow(10, dAxisYMaxPow));                         //Get the first digit of the max value
            dAxisYMax = (dAxisYMax + 1) * Math.Pow(10, dAxisYMaxPow);                               //Calculate the new Y-Max value
            ExcelAxisY.MaximumScale = dAxisYMax;                                                    //Set the new Y-Max value
            ExcelAxisY.MajorUnit = (ExcelAxisY.MaximumScale - ExcelAxisY.MinimumScale) / 4;
            if (dAxisYMax < 1)
            {
                int iNumOfDecimals = -(int)Math.Floor(Math.Log10(dAxisYMax));
                iNumOfDecimals = Math.Max(1, iNumOfDecimals);
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
