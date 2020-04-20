using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SequenceCardGame;

public static class SaveLoadManager
{
    public static void SavePlayerData(Data data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + ConstantString.SaveDataFile, FileMode.Create);
        bf.Serialize(stream, data);
        stream.Close();
    }
    public static int LoadPlayerData()
    {
        if (File.Exists(Application.persistentDataPath + ConstantString.SaveDataFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + ConstantString.SaveDataFile, FileMode.Open);
            Data data = bf.Deserialize(stream) as Data;
            stream.Close();
            return data.HighScore;
        }
        else
        {
            SavePlayerData(new Data(0));
            return 0;
        }

    }
}
