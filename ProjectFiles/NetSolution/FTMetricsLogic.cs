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
    public void FTMetricsReadOEE(string SelectWorkCellName, string SelectOrderID)
    {
        // Get the different pieces we need to build the graph
        //IUANode myModelObject = Owner.Get("CarbonByShift");
        //var orderID = (String)Owner.GetVariable("SelectOrderID").Value;
        //var workCellName = (String)Owner.GetVariable("SelectWorkCellName").Value;
        
        //var nodata = Owner.GetVariable("NoData1");
        nodata = LogicObject.GetVariable("NoData1");
        //DateTime select_date = Owner.GetVariable("SelectDay").Value;
        //int year = select_date.Year;
        //int month = select_date.Month;
        //int day = select_date.Day;
        mystore = LogicObject.GetVariable("MyDatabase");
        Store storemyDbStore = InformationModel.Get<Store>(mystore.Value);
        //Store myDbStore = InformationModel.Get<Store>(Owner.GetVariable("MyDatabase").Value);
        /*string sqlQuery = $"SELECT Shift,SUM(RateToCarbon) AS Value FROM RecordShiftEnergy " +
            $"WHERE Group=\'{group}\' AND Year={year} AND Month={month} AND Day={day} GROUP BY Shift";*/

        string sqlQuery = $"SELECT dOEE,dAvailPcnt,dPerformancePcnt,dQualityPcnt,sFlex4String FROM OEEQWorkCellDetail " +
            $"WHERE sFlex4String=\'{SelectOrderID}\' AND sActivityAreaName='{SelectWorkCellName}'";

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
            // Delete all children from Object
            /*foreach (var children in myModelObject.Children)
            {
                children.Delete();
            }*/
            // For each column create an Object children
            for (int i = 0; i < ResultSet.GetLength(0); i++)
            {
                /*String columnName = "Shift_" + Convert.ToString(ResultSet[i, 0]);
                var myObj = InformationModel.MakeVariable(columnName, OpcUa.DataTypes.String);
                myObj.Value = Convert.ToDouble(ResultSet[i, 1]);
                myModelObject.Add(myObj);*/

                Log.Info(LogicObject.BrowseName, $"OEE = '{ResultSet[i, 0]}' - OrderId = '{ResultSet[i, 1]}'");

                //Project.Current.GetObject("Model/OEE").GetVariable("Data" + (i + 1)).Value = Convert.ToString(ResultSet[i, 0]);
                //Project.Current.GetObject("Model/Availability").GetVariable("Data" + (i + 1)).Value = Convert.ToString(ResultSet[i, 1]);
                //Project.Current.GetObject("Model/Performance").GetVariable("Data" + (i + 1)).Value = Convert.ToString(ResultSet[i, 2]);
                //Project.Current.GetObject("Model/Quality").GetVariable("Data" + (i + 1)).Value = Convert.ToString(ResultSet[i, 3]);

            }
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
