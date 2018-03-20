using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class PlayerControllerTest {
    PlayerController playerController;
    GameObject stationAobj = new GameObject();
    GameObject stationBobj = new GameObject();
    GameObject stationCobj = new GameObject();

    [SetUp]
    public void Setup()
    {
        playerController = new PlayerController
        {
            isControllerEnabled = true
        };

        stationAobj.AddComponent<SmallStation>().stopName="A";
        stationBobj.AddComponent<SmallStation>().stopName = "B";
        stationCobj.AddComponent<SmallStation>().stopName = "C";
    }

    [Test]
    public void SetAndGetTravelsTest()
    {
        playerController.setTravels(4);
        Assert.AreEqual(4,playerController.getTravels());
    }

    [Test]
    public void CommitMovesTest()
    {
        playerController.GetSelectedStations().Push(stationAobj);
        playerController.GetSelectedStations().Push(stationBobj);
        playerController.GetSelectedStations().Push(stationCobj);

        playerController.commitMoves();



        Assert.AreEqual(3,playerController.GetCommitedStations().Count);
        Assert.AreEqual("A", playerController.GetCommitedStations().Pop().GetComponent<SmallStation>().stopName);
        Assert.AreEqual("B", playerController.GetCommitedStations().Pop().GetComponent<SmallStation>().stopName);
        Assert.AreEqual("C", playerController.GetCommitedStations().Pop().GetComponent<SmallStation>().stopName);
    }

    [Test]
    public void SetCurrentSelectedStationTest()
    {
        playerController.SetStartingStaion(stationAobj);

        playerController.GetSelectedStations().Push(stationBobj);
        playerController.setcurrentSelectedStation();
        Assert.AreEqual("B",playerController.GetCurrentSelectedStation().GetComponent<SmallStation>().stopName);
        playerController.GetSelectedStations().Push(stationCobj);
        playerController.setcurrentSelectedStation();
        Assert.AreEqual("C", playerController.GetCurrentSelectedStation().GetComponent<SmallStation>().stopName);

        playerController.GetSelectedStations().Clear();
        playerController.setcurrentSelectedStation();
        Assert.AreEqual("A", playerController.GetCurrentSelectedStation().GetComponent<SmallStation>().stopName);

    }


}