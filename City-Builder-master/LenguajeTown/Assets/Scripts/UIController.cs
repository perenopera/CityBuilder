using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private Text dayText;

    [SerializeField]
    private Text cityText;

    [SerializeField]
    private Text timeText;

    private City city;
    // Use this for initialization
    void Start()
    {
        city = GetComponent<City>();
    }

    public void UpdateCityData()
    {
        cityText.text = string.Format("Traballs: {0}/{1}\nDiners: {2}€ (+{6}€)\nPoblació: {3}/{4}\nMenjar: {5}",
            city.JobsCurrent,
            city.JobsCeiling,
            city.Cash,
            (int)city.PopulationCurrent,
            (int)city.PopulationCeiling,
            (int)city.Food,
            city.JobsCurrent * 2);
    }

    public void UpdateDayCount()
    {
        dayText.text = string.Format("Day: {0}   Nivell: {1}", city.Day, city.Nivell);
    }
    public void UpdateTime()
    {
        timeText.text = string.Format("Dia real: {0}", city.today);
    }
}
