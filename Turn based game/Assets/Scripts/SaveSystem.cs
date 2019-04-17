﻿using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/saveFile.tbsg";

    public static void SaveGame(GameData t_gd)
    {
        // We create a save file and add serializable data (so it is binary)
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        GameData data = t_gd;

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved in " + savePath);
    }

    public static GameData LoadGame()
    {
        if (File.Exists(savePath))   // Check if the file exists
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return data;
        }

        Debug.Log("Save file not found in " + savePath);
        return null;
    }

    public static GameData GenerateGameData(List<int> t_partyHealt, List<int> t_partyAttackDamage, float[,] t_partyPosition)
    {
        GameData gd = new GameData();

        gd.partyPositions = t_partyPosition;

        gd.partyHealth = t_partyHealt.ToArray();
        gd.partyAttackDamage = t_partyAttackDamage.ToArray();

        return gd;
    }
}