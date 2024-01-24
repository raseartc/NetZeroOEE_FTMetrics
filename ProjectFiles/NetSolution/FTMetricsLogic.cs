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
using MQTTnet.Server.Internal;
using System.Reflection;
using FTOptix.NativeUI;
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
        FTMetricsReadOrderInfo(WorkCellName);
        FTMetricsReadMachineState(WorkCellName);
        FTMetricsReadEventsHist(WorkCellName);
        FTMetricsReadMachineFaultedSec(WorkCellName);
    }

    public void FTMetricsReadOEE(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData1");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        string select_date = DateTime.UtcNow.ToString("yyyyMMdd");
        select_date = "2024-1-3";

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT SUM(dGoodParts) AS GoodParts, SUM(dTotalParts) AS TotalParts, SUM(dScrapParts) AS ScrapParts, AVG(dIdealCycleTime) AS IdealCycleTime ,SUM(dTotalTime) AS TotalTimeSec, SUM(dAvailSec) AS AvailSec, SUM(dRunSec) AS RunSec, SUM(dDownSec) AS DownSec, AVG(dGoodPartsPcnt) AS GoodPartsPcnt, AVG(dScrapPartsPcnt) AS ScrapPartsPcnt, sShiftDescription FROM OEEQWorkCellDetail " +
            $"WHERE sDescription = '{SelectWorkCellName}' AND tStartTime >= '{select_date}' GROUP BY sShiftDescription " +
            $"ORDER BY sShiftDescription DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
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

                double GoodParts = Convert.ToDouble(ResultSet[0, 0]);
                myModelObject.GetVariable("GoodParts").Value = GoodParts;
                double TotalParts = Convert.ToDouble(ResultSet[0, 1]);
                myModelObject.GetVariable("TotalParts").Value = TotalParts;
                myModelObject.GetVariable("ScrapParts").Value = Convert.ToDouble(ResultSet[0, 2]);
                double IdealCycleTime = Convert.ToDouble(ResultSet[0, 3]);
                myModelObject.GetVariable("TotalTimeSec").Value = Convert.ToDouble(ResultSet[0, 4]);
                double AvailSec = Convert.ToDouble(ResultSet[0, 5]);
                myModelObject.GetVariable("AvailSec").Value = AvailSec;
                double RunSec = Convert.ToDouble(ResultSet[0, 6]);
                UpTime = RunSec;
                myModelObject.GetVariable("RunSec").Value = RunSec;

                myModelObject.GetVariable("DownSec").Value = Convert.ToDouble(ResultSet[0, 7]);              
                myModelObject.GetVariable("GoodPartsPcnt").Value = Convert.ToDouble(ResultSet[0, 8]);
                myModelObject.GetVariable("ScrapPartsPcnt").Value = Convert.ToDouble(ResultSet[0, 9]);

                double Availability = RunSec / AvailSec;
                double Performance = (TotalParts * IdealCycleTime) / RunSec;
                double Quality = GoodParts / TotalParts;
                double OEE = Availability * Performance * Quality;

                myModelObject.GetVariable("OEE").Value = OEE * 100;
                myModelObject.GetVariable("Availability").Value = Availability * 100;
                myModelObject.GetVariable("Performance").Value = Performance * 100;
                myModelObject.GetVariable("Quality").Value = Quality * 100;


            //Log.Info(LogicObject.BrowseName, $"OEE = '{OEE * 100}' - Availability = '{Availability * 100}' - Performance = '{Performance * 100}' - Quality = '{Quality * 100}'");

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void FTMetricsReadOrderInfo(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData2");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT sFlex4String AS OrderID, sFlex1String AS Operator, sPartId AS PartId, sShiftDescription AS Shift FROM OEEQWorkCellDetail " +
            $"WHERE sDescription = '{SelectWorkCellName}'" +
            $"ORDER BY lOEEWorkCellId DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
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

                myModelObject.GetVariable("Order").Value = Convert.ToString(ResultSet[0, 0]);
                myModelObject.GetVariable("Operator").Value = Convert.ToString(ResultSet[0, 1]);
                myModelObject.GetVariable("Part").Value = Convert.ToString(ResultSet[0, 2]);
                myModelObject.GetVariable("Shift").Value = Convert.ToString(ResultSet[0, 3]);

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

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT sStateDescription, SUM(dDurationSeconds) as StatesCount FROM OEEQStateData " +
            $"WHERE sWorkcellDescription = '{SelectWorkCellName}' AND tStart >= '{select_date}' GROUP BY sStateDescription " +
            $"ORDER BY StatesCount DESC";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
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
                myModelObject.GetVariable("MCStateCnt" + (i + 1)).Value = Convert.ToDouble(ResultSet[i, 1]);

                if (i > 3) break;
            }

            Log.Info(LogicObject.BrowseName, $"No. of Record = '{ResultSet.GetLength(0)}' ");

            if (ResultSet.GetLength(0) < 4)
            {
                myModelObject.GetVariable("MCStateDesc4").Value = "";
                myModelObject.GetVariable("MCStateCnt4").Value = 0;
            }

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

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT sReportingValue, COUNT(*) as FaultsCount FROM OEEQEventHistory " +
            $"WHERE sDescription = '{SelectWorkCellName}' AND sCategory = 'Machine Faults' AND dReportingValue > 0 AND tStart >= '{select_date}' GROUP BY sReportingValue ";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
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
                myModelObject.GetVariable("MCFaultCnt" + (i + 1)).Value = Convert.ToDouble(ResultSet[i, 1]);

                if (i > 5) break;
            }

            Log.Info(LogicObject.BrowseName, $"No. of Record = '{ResultSet.GetLength(0)}' ");

            /*if (ResultSet.GetLength(0) < 6)
            {
                myModelObject.GetVariable("MCStateDesc4").Value = "";
                myModelObject.GetVariable("MCStateCnt4").Value = 0;
            }*/

        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }
    }

    public void FTMetricsReadMachineFaultedSec(string SelectWorkCellName)
    {
        nodata = LogicObject.GetVariable("NoData5");
        IUANode myModelObject = Project.Current.Get("Model/FTMetrics/FTMDataCurrentShift");

        string select_date = DateTime.UtcNow.ToString("yyyyMMdd");
        select_date = "2024-1-3";

        mystore = LogicObject.GetVariable("MyDatabase");
        Store myDbStore = InformationModel.Get<Store>(mystore.Value);

        string sqlQuery = $"SELECT SUM(OEEFaultMetricData.lFaultCount) AS FaultCount, SUM(OEEFaultMetricData.dFaultSeconds) AS FaultSec FROM OEEFaultMetricData INNER JOIN OEEQWorkCell ON OEEFaultMetricData.lOEEWorkCellId = OEEQWorkCell.lOEEWorkCellId " +
            $"WHERE OEEQWorkCell.sDescription = '{SelectWorkCellName}' AND OEEQWorkCell.tStart >= '{select_date}' GROUP BY OEEFaultMetricData.lOEEConfigWorkCellId, OEEQWorkCell.sDescription ";

        // Prepare SQL Query
        // Execute query and check result
        try
        {
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

            myModelObject.GetVariable("MCFaultedCount").Value = Convert.ToDouble(ResultSet[0, 0]);
            myModelObject.GetVariable("MCFaultedSec").Value = Convert.ToDouble(ResultSet[0, 1]);

            myModelObject.GetVariable("MTTR").Value = myModelObject.GetVariable("MCFaultedSec").Value / myModelObject.GetVariable("MCFaultedCount").Value;
            myModelObject.GetVariable("MTBF").Value = UpTime / myModelObject.GetVariable("MCFaultedCount").Value;
        }
        catch (Exception ex)
        {
            Log.Error(LogicObject.BrowseName, ex.Message);
            return;
        }

    }

    private IUAVariable nodata;
    private IUAVariable mystore;
    double UpTime;
}
