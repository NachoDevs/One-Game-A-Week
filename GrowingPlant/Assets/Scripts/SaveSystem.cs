using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string savePath = Application.persistentDataPath + "/saveFile.grw";

    public static void SaveGame(Plant t_plant, GameManager t_gm)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(savePath, FileMode.Create);

        PlantData data = new PlantData(t_plant, t_gm);

        formatter.Serialize(stream, data);
        stream.Close();

        Debug.Log("Saved in " + savePath);
    }

    public static PlantData LoadGame()
    {
        if(File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(savePath, FileMode.Open);

            PlantData data = formatter.Deserialize(stream) as PlantData;
            stream.Close();

            return data;
        }

        Debug.Log("Save file not found in " + savePath);
        return null;
    }
}
