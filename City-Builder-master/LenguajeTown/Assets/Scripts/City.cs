using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    public DateTime today = DateTime.Now;
    public int Cash { get; set; }
    public int Day { get; set; }
    public int Nivell { get; set; }
    public float PopulationCurrent { get; set; }
    public float PopulationCeiling { get; set; }
    public int JobsCurrent { get; set; }
    public int JobsCeiling { get; set; }
    public float Food { get; set; }
    public int[] buildingCount = new int[10]; //0 Road; 1-2-3 House; 4-5-6 Farm; 7-8-9 Factory; 10 School
    
    public float duracioDia = 20f;
    public Boolean esDeDia;

    public enum buildings
    {
        //ID 0 
        Road,
        //ID 1-2-3
        House,House2,House3,
        //ID 4-5-6
        Farm,Farm2,Farm3,
        //ID 7-8-9
        Factory,Factory2,Factory3,
        //ID 10
        School
    };
    private UIController uiController;
    // Use this for initialization
    void Start()
    {
        uiController = GetComponent<UIController>();
        Cash = 150;
        uiController.UpdateDayCount();
        uiController.UpdateCityData();
        uiController.UpdateTime();
        InvokeRepeating("EndTurn", duracioDia, duracioDia);
        InvokeRepeating("CicleDiaNit", duracioDia/2, duracioDia/2);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            EndTurn();
        }


    }
    private void CicleDiaNit() { 
    
            Debug.Log("Son les: " + today);
    }
    private void pujarNivell()
    {
        Nivell++;
    }

    public void EndTurn()
    {
        Day++;
        CalculateCash();
        CalculateJobs();
        CalculatePopulation();
        CalculateFood();
        uiController.UpdateDayCount();
        uiController.UpdateCityData();
        Debug.Log("Day ended.");
        Debug.LogFormat("Jobs: {0}/{1}, Cash: {2}, Pop: {3}/{4}, Food: {5}", JobsCurrent, JobsCeiling, Cash, PopulationCurrent, PopulationCeiling, Food);
    }

    void CalculateJobs()
    {
        JobsCeiling = buildingCount[(int)buildings.Factory] * 10;
        if (buildingCount[(int)buildings.Factory2] != 0) { JobsCeiling = buildingCount[(int)buildings.Factory2] * 20; }
        if (buildingCount[(int)buildings.Factory3] != 0) { JobsCeiling = buildingCount[(int)buildings.Factory3] * 50; }
        JobsCurrent = Mathf.Min((int)PopulationCurrent, JobsCeiling);
    }

    void CalculateCash()
    {
        Cash += JobsCurrent * 2;
    }

    public void DepositCash(int deposit)
    {
        Cash += deposit;
    }

    void CalculateFood()
    {
        Food += buildingCount[(int)buildings.Farm] * 2.5f;
        Food += buildingCount[(int)buildings.Farm2] * 5f;
        Food += buildingCount[(int)buildings.Farm3] * 10f;
        Food -= PopulationCurrent * 0.5f;
    }

    void CalculatePopulation()
    {
        PopulationCeiling = buildingCount[(int)buildings.House] * 5;
        if (Food >= PopulationCurrent * .5f && PopulationCurrent < PopulationCeiling)
        {
            //Food -= PopulationCurrent * .25f;
            PopulationCurrent = Mathf.Min(PopulationCurrent += Food * .25f, PopulationCeiling);
        }
        else if (Food < PopulationCurrent * 0.25f)
        {
            PopulationCurrent -= (PopulationCurrent - Food) * .5f;
        }
    }
}
