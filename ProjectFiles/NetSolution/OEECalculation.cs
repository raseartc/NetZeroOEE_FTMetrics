#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.NativeUI;
using FTOptix.Alarm;
using FTOptix.NetLogic;
using FTOptix.UI;
using FTOptix.WebUI;
using FTOptix.DataLogger;
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
using System.Threading;
using FTOptix.SQLiteStore;
using FTOptix.S7TCP;
using FTOptix.Modbus;
#endregion

public class OEECalculation : BaseNetLogic
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        myPeriodicTask = new PeriodicTask(OEECal, 2000, LogicObject);
        myPeriodicTask.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        myPeriodicTask.Dispose();
    }

    private void OEECal()
    {
        float oee, availability, performance, quality;
        float availableTime, runningTime, idealCT, goodParts, scrapParts, totalParts;

        //Project.Current.GetVariable("Model/OEE/OEE").Value = a+p+q;
        //Thread.Sleep(1);

        // Gather necessary data
        /*TimeSpan operatingTime = TimeSpan.FromHours(8); // Total operating time (e.g., 8 hours)
        TimeSpan downtime = TimeSpan.FromMinutes(30); // Total downtime (e.g., 30 minutes)
        TimeSpan idealCycleTime = TimeSpan.FromSeconds(20); // Ideal cycle time (e.g., 20 seconds)
        int totalCount = 1000; // Total count of units produced
        int goodCount = 950; // Count of good units produced*/

        availableTime = Project.Current.GetVariable("Model/OEE/AvailableTime").Value;
        runningTime = Project.Current.GetVariable("Model/OEE/RunningTime").Value;
        idealCT = Project.Current.GetVariable("Model/OEE/IdealCT").Value;
        goodParts = Project.Current.GetVariable("Model/OEE/GoodParts").Value;
        scrapParts = Project.Current.GetVariable("Model/OEE/ScrapParts").Value;
        totalParts = Project.Current.GetVariable("Model/OEE/TotalParts").Value;

        // Calculate OEE components
        /*double availability = CalculateAvailability(operatingTime, downtime);
        double performance = CalculatePerformance(idealCycleTime, totalCount, operatingTime);
        double quality = CalculateQuality(goodCount, totalCount);

        availability = CalculateAvailability(operatingTime, downtime);
        performance = CalculatePerformance(idealCycleTime, totalCount, operatingTime);
        quality = CalculateQuality(goodCount, totalCount);*/

        /*availability = CalculateAvailability(availableTime, runningTime);
        performance = CalculatePerformance(idealCT, totalParts, availableTime);
        quality = CalculateQuality(goodParts, totalParts);

        // Calculate OEE
        //double oee = CalculateOEE(availability, performance, quality);

        oee = CalculateOEE(availability, performance, quality);*/

        /*Console.WriteLine($"Availability: {availability:P}");
        Console.WriteLine($"Performance: {performance:P}");
        Console.WriteLine($"Quality: {quality:P}");
        Console.WriteLine($"OEE: {oee:P}");*/

        availability = (runningTime / 60000) / (availableTime / 60000); //ms to Min
        performance = (idealCT * totalParts) / (runningTime / 1000); //ms to Sec
        quality = goodParts / totalParts;
        oee = availability * performance * quality;

        Project.Current.GetVariable("Model/OEE/Availability").Value = availability * 100;
        Project.Current.GetVariable("Model/OEE/Performance").Value = performance * 100;
        Project.Current.GetVariable("Model/OEE/Quality").Value = quality * 100;
        Project.Current.GetVariable("Model/OEE/OEE").Value = oee * 100;

        Thread.Sleep(1);
    }

    /*static double CalculateAvailability(TimeSpan operatingTime, TimeSpan downtime)
    {
        double availability = (operatingTime.TotalMinutes - downtime.TotalMinutes) / operatingTime.TotalMinutes;
        return availability;
    }

    static double CalculatePerformance(TimeSpan idealCycleTime, int totalCount, TimeSpan operatingTime)
    {
        double performance = (idealCycleTime.TotalSeconds * totalCount) / operatingTime.TotalSeconds;
        return performance;
    }

    static double CalculateQuality(int goodCount, int totalCount)
    {
        double quality = (double)goodCount / totalCount;
        return quality;
    }

    static double CalculateOEE(double availability, double performance, double quality)
    {
        double oee = availability * performance * quality;
        return oee;
    }*/

    private PeriodicTask myPeriodicTask;
}
