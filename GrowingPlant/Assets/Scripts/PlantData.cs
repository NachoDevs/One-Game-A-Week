﻿[System.Serializable]
public class PlantData
{
    public float growthState;
    public int[] dateTime;  

    public PlantData(Plant t_plant)
    {
        growthState = t_plant.growthState;

        dateTime = new int[6];
        dateTime[0] = t_plant.lastTimeConected.Year;
        dateTime[1] = t_plant.lastTimeConected.Month;
        dateTime[2] = t_plant.lastTimeConected.Day;
        dateTime[3] = t_plant.lastTimeConected.Hour;
        dateTime[4] = t_plant.lastTimeConected.Minute;
        dateTime[5] = t_plant.lastTimeConected.Second;
    }
}
