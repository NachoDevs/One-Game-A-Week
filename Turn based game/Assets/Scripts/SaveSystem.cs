using UnityEngine;
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

        //Debug.Log("Saved in " + savePath);
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

        //Debug.Log("Save file not found in " + savePath);
        return null;
    }

    public static GameData GenerateGameData(List<GameObject> characters, List<int> t_charactersInvolvedInCombat, float[,] t_charsPosition)
    {
        GameData gd = new GameData();

        List<bool> charsDead = new List<bool>();
        List<bool> charsHaveMoved = new List<bool>();
        List<int> charsHealth = new List<int>();
        List<float> charsAttackDamage = new List<float>();

        foreach (GameObject character in characters)
        {
            Character characterC = character.GetComponent<Character>();
            charsDead.Add(characterC.isDead);
            charsHaveMoved.Add(characterC.hasMoved);
            charsHealth.Add(characterC.health);
            charsAttackDamage.Add(characterC.damageBoost);
        }

        int charaterCount = charsHealth.Count;
        float[,] charsPos = new float[2, charaterCount];

        foreach (GameObject character in characters)
        {
            Character characterC = character.GetComponent<Character>();
            charsPos[0, characterC.characterIndex] = character.transform.position.x;
            charsPos[1, characterC.characterIndex] = character.transform.position.y;
        }

        gd.charsPositions = charsPos;
        if (t_charsPosition != null)
        {
            gd.charsPositions = t_charsPosition;
        }

        gd.charsDead = charsDead.ToArray();
        gd.charsHaveMoved = charsHaveMoved.ToArray();
        gd.charsHealth = charsHealth.ToArray();
        gd.charsDamageBoost = charsAttackDamage.ToArray();

        if (t_charactersInvolvedInCombat != null)
        {
            gd.charactersInvolvedInCombat = t_charactersInvolvedInCombat.ToArray();
        }

        return gd;
    }
}
