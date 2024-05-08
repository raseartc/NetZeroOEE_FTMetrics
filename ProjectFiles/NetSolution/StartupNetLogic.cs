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
using FTOptix.Report;
using FTOptix.RAEtherNetIP;
using FTOptix.Retentivity;
using FTOptix.CoreBase;
using FTOptix.CommunicationDriver;
using FTOptix.UI;
using FTOptix.Core;
using FTOptix.NativeUI;
using FTOptix.SQLiteStore;
using FTOptix.S7TCP;
using FTOptix.Modbus;
using FTOptix.OPCUAClient;
using FTOptix.OPCUAServer;
using FTOptix.MelsecQ;
#endregion

public class StartupNetLogic : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        //var project = FTOptix.HMIProject.Project.Current;
        //var project = Project.Current;
        //var myIF1eChart = project.GetObject("UI/ProjectPanels/InstantFizzLine1").Get<NetLogicObject>("eChartLogic");
        //var myIF1eChart = project.Get<NetLogicObject>("UI/ProjectPanels/InstantFizzLine1/eChartLogic");
        
        //var myIF1eChart = LogicObject.Get("UI/ProjectPanels/InstantFizzLine1/eChartLogic");
        
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
