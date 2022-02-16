using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
public static class SaveSystem
{
    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "Continue";
        FileStream stream;
        if (File.Exists(path))
        {
            stream = new FileStream(path, FileMode.Open);
        }
        else
        {
            stream = new FileStream(path, FileMode.Create);
        }
        SavedDeck[] savedDecks = AllDecks.Instance.SaveDecks();
        Save save = new Save(savedDecks);
        formatter.Serialize(stream, save);
        stream.Close();
    }

    public static Save LoadGame()
    {
        string path = Application.persistentDataPath + "Continue";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            Save save = formatter.Deserialize(stream) as Save;

            stream.Close();
            return save;
        }
        else
        {
            Debug.LogWarning("NO SAVE DATA FOUND");
            return null;
        }
    }

    public static bool SaveExists()
    {
        string path = Application.persistentDataPath + "Continue";
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "Continue";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
