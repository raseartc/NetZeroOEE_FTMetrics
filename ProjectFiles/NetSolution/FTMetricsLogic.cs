#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.DataLogger;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.Alarm;
using FTOptix.EventLogger;
using FTOptix.Store;
using FTOptix.ODBCStore;
using FTOptix.SQLiteStore;
using FTOptix.Report;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.UI;
using FTOptix.Core;
using System.Text.RegularExpressions;
#endregion

public class FTMetricsLogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void FTMetricsReadData(string WorkCellName)
    {
        FTMetricsReadOEE(WorkCellName);
        FTMetricsReadOEELastUpdate(WorkCellName);
        FTMetricsReadMachineState(WorkCellName);
        FTMetricsReadEventsHist(WorkCellName);
    }

    public void FTMetricsReadOEE(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData1");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        string select_date = DateTime.UtcNow.ToString("yyyyMMdd");
        select_date = "2024-1-3";

        //Store myDbStore = InformationModel.Get<Store>(Owner.GetVariable("MyDatabase").Value);
        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT AVG(dOEE) AS OEE, AVG(dAvailPcnt) AS Availability, AVG(dPerformancePcnt) AS Performance, AVG(dQualityPcnt) AS Quality, SUM(dGoodParts) AS GoodParts, SUM(dScrapParts) AS ScrapParts, SUM(dTotalParts) AS TotalParts, SUM(dTotalTime) AS TotalTimeMin, SUM(dAvailSec) AS AvailMin, SUM(dRunSec) AS RunMin, SUM(dDownSec) AS DownMin, AVG(dGoodPartsPcnt) AS GoodPartsPcnt, AVG(dScrapPartsPcnt) AS ScrapPartsPcnt, sShiftDescription FROM OEEQWorkCellDetail " +
            $"WHERE sDescription = '{SelectWorkCellName}' AND dOEE > 0 AND dAvailPcnt > 0 AND dPerformancePcnt > 0 AND dQualityPcnt > 0 AND tStartTime > '{select_date}' GROUP BY sShiftDescription " +
            $"ORDER BY sShiftDescription DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            //PieChart myChart = (PieChart)Owner.GetObject("CarbonByShiftChart");
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            // For each column create an Object children
            //for (int i = 0; i < ResultSet.GetLength(0); i++)
            //{
            //Log.Info(LogicObject.BrowseName, $"OEE = '{ResultSet[i, 0]}' - Availability = '{ResultSet[i, 1]}'");

                myModelObject.GetVariable("OEE").Value = Convert.ToString(ResultSet[0, 0]);
                myModelObject.GetVariable("Availability").Value = Convert.ToString(ResultSet[0, 1]);
                myModelObject.GetVariable("Performance").Value = Convert.ToString(ResultSet[0, 2]);
                myModelObject.GetVariable("Quality").Value = Convert.ToString(ResultSet[0, 3]);
                myModelObject.GetVariable("GoodParts").Value = Convert.ToString(ResultSet[0, 4]);
                myModelObject.GetVariable("ScrapParts").Value = Convert.ToString(ResultSet[0, 5]);
                myModelObject.GetVariable("TotalParts").Value = Convert.ToString(ResultSet[0, 6]);
                myModelObject.GetVariable("TotalTimeMin").Value = Convert.ToString(ResultSet[0, 7]);
                myModelObject.GetVariable("AvailMin").Value = Convert.ToString(ResultSet[0, 8]);
                myModelObject.GetVariable("RunMin").Value = Convert.ToString(ResultSet[0, 9]);
                myModelObject.GetVariable("DownMin").Value = Convert.ToString(ResultSet[0, 10]);
                myModelObject.GetVariable("GoodPartsPcnt").Value = Convert.ToString(ResultSet[0, 11]);
                myModelObject.GetVariable("ScrapPartsPcnt").Value = Convert.ToString(ResultSet[0, 12]);

            //}
            //myChart.Refresh();
        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void FTMetricsReadOEELastUpdate(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData2");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        //Store myDbStore = InformationModel.Get<Store>(Owner.GetVariable("MyDatabase").Value);
        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT sFlex4String AS OrderID, sFlex1String AS Operator, sPartId AS PartId, sShiftDescription AS Shift FROM OEEQWorkCellDetail " +
            $"WHERE sDescription = '{SelectWorkCellName}'" +
            $"ORDER BY lOEEWorkCellId DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            //PieChart myChart = (PieChart)Owner.GetObject("CarbonByShiftChart");
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            // For each column create an Object children
            //for (int i = 0; i < ResultSet.GetLength(0); i++)
            //{
            //Log.Info(LogicObject.BrowseName, $"OEE = '{ResultSet[i, 0]}' - Availability = '{ResultSet[i, 1]}'");

                myModelObject.GetVariable("Order").Value = Convert.ToString(ResultSet[0, 0]);
                myModelObject.GetVariable("Operator").Value = Convert.ToString(ResultSet[0, 1]);
                myModelObject.GetVariable("Part").Value = Convert.ToString(ResultSet[0, 2]);
                myModelObject.GetVariable("Shift").Value = Convert.ToString(ResultSet[0, 3]);

            //}
            //myChart.Refresh();
        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void FTMetricsReadMachineState(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData3");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        string select_date = DateTime.UtcNow.ToString("yyyyMMdd");
        select_date = "2024-1-3";

        //Store myDbStore = InformationModel.Get<Store>(Owner.GetVariable("MyDatabase").Value);
        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT sStateDescription, SUM(dDurationSeconds) as StatesCount FROM OEEQStateData " +
            $"WHERE sWorkcellDescription = '{SelectWorkCellName}' AND tStart > '{select_date}' GROUP BY sStateDescription " +
            $"ORDER BY StatesCount DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            //PieChart myChart = (PieChart)Owner.GetObject("CarbonByShiftChart");
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            // For each column create an Object children
            for (int i = 0; i < ResultSet.GetLength(0); i++)
            {
                Log.Info(LogicObject.BrowseName, $"MCStateDesc = '{ResultSet[i, 0]}' - MCStateCnt = '{ResultSet[i, 1]}'");

                myModelObject.GetVariable("MCStateDesc" + (i + 1)).Value = Convert.ToString(ResultSet[i, 0]);
                myModelObject.GetVariable("MCStateCnt" + (i + 1)).Value = Convert.ToString(ResultSet[i, 1]);

                if (i > 3) break;
            }

            Log.Info(LogicObject.BrowseName, $"No. of Record = '{ResultSet.GetLength(0)}' ");

            if (ResultSet.GetLength(0) < 4)
            {
                myModelObject.GetVariable("MCStateDesc4").Value = "";
                myModelObject.GetVariable("MCStateCnt4").Value = 0;
            }

            //myChart.Refresh();
        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void FTMetricsReadEventsHist(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData4");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        string select_date = DateTime.UtcNow.ToString("yyyyMMdd");
        select_date = "2024-1-3";

        //Store myDbStore = InformationModel.Get<Store>(Owner.GetVariable("MyDatabase").Value);
        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT sReportingValue, COUNT(*) as FaultsCount FROM OEEQEventHistory " +
            $"WHERE sDescription = '{SelectWorkCellName}' AND sCategory = 'Machine Faults' AND dReportingValue > 0 AND tStart > '{select_date}' GROUP BY sReportingValue ";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
            //PieChart myChart = (PieChart)Owner.GetObject("CarbonByShiftChart");
            Object[,] ResultSet;
            String[] Header;
            myDbStore.Query(sqlQuery, out Header, out ResultSet);
            if (ResultSet.GetLength(0) < 1)
            {
                nodata.Value = true;
                Log.Error(LogicObject.BrowseName, "Input query returned less than one line");
                return;
            }
            nodata.Value = false;

            // For each column create an Object children
            for (int i = 0; i < ResultSet.GetLength(0); i++)
            {
                Log.Info(LogicObject.BrowseName, $"MCStateDesc = '{ResultSet[i, 0]}' - MCStateCnt = '{ResultSet[i, 1]}'");

                myModelObject.GetVariable("MCFaultDesc" + (i + 1)).Value = Convert.ToString(ResultSet[i, 0]);
                myModelObject.GetVariable("MCFaultCnt" + (i + 1)).Value = Convert.ToString(ResultSet[i, 1]);

                if (i > 5) break;
            }

            Log.Info(LogicObject.BrowseName, $"No. of Record = '{ResultSet.GetLength(0)}' ");

            /*if (ResultSet.GetLength(0) < 6)
            {
                myModelObject.GetVariable("MCStateDesc4").Value = "";
                myModelObject.GetVariable("MCStateCnt4").Value = 0;
            }*/

            //myChart.Refresh();
        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    private IUAVariable nodata;
    private IUAVariable mystore;
}
