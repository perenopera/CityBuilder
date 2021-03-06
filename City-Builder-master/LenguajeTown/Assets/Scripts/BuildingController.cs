﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{

    [SerializeField] private City city;
    [SerializeField] private UIController uiController;
    [SerializeField] private Building[] buildings;
    [SerializeField] private Board board;

    private enum Action
    {
        create,
        delete,
        Update
    };
    private Building selectedBuilding;
    private Building nextBuilding;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftShift) && selectedBuilding != null)
        {
            InteractWithBoard(Action.create);
        }
        else if (Input.GetMouseButtonDown(0) && selectedBuilding != null)
        {
            InteractWithBoard(Action.create);
        }
        else if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift))
        {
            InteractWithBoard(Action.delete);
        }
        if (Input.GetMouseButton(1))
        {
            InteractWithBoard(Action.delete);
        }
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))
        {
            InteractWithBoard(Action.Update);
        }
    }

    void InteractWithBoard(Action action)
    {
        Ray charles = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(charles, out hit))
        {
            //Debug.Log("Into if Physics.Raycast");
            Vector3 gridPosition = board.CalculateGridPosition(hit.point);
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (action == Action.create && board.CheckForBuildingAtPosition(gridPosition) == null)
                {
                    //Debug.Log("Into if board.CheckForBuildingAtPosition");
                    if (city.Cash >= selectedBuilding.cost)
                    {
                        //Debug.Log("Into if city.Cash >= selectedBuilding.cost");
                        city.DepositCash(-selectedBuilding.cost);
                        uiController.UpdateCityData();
                        city.buildingCount[selectedBuilding.id]++;
                        board.AddBuilding(selectedBuilding, gridPosition);
                    }
                }
                else if (action == Action.delete && board.CheckForBuildingAtPosition(gridPosition) != null)
                {
                    city.DepositCash(board.CheckForBuildingAtPosition(gridPosition).cost / 2);
                    city.buildingCount[selectedBuilding.id]--;
                    board.RemoveBuilding(gridPosition);
                    uiController.UpdateCityData();
                }
                else if (action == Action.Update && board.CheckForBuildingAtPosition(gridPosition) != null)
                {
                    selectedBuilding = board.CheckForBuildingAtPosition(gridPosition);
                    if (selectedBuilding.id == 0 || selectedBuilding.id == 3 || selectedBuilding.id == 6 || selectedBuilding.id == 9)
                    {
                        return;
                    }
                    if (city.Cash >= selectedBuilding.cost)
                    {
                        board.RemoveBuilding(gridPosition);
                        city.buildingCount[selectedBuilding.id]--;
                        board.AddBuilding(buildings[selectedBuilding.id + 1], gridPosition);
                        city.buildingCount[selectedBuilding.id + 1]++;
                        city.DepositCash(-buildings[selectedBuilding.id + 1].cost);
                        uiController.UpdateCityData();
                    }
                    else
                    {
                        Debug.Log("No tens pasta");
                    }
                }
            }
        }
    }

    public void EnableBuilder(int buildingId)
    {
        Debug.Log(buildingId);
        selectedBuilding = buildings[buildingId];
        Debug.Log("Selected Building: " + selectedBuilding.buildingName);
    }
}
