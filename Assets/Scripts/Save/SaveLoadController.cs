using System;
using UnityEngine;

public class SaveLoadController{
    private string _filePath = Application.persistentDataPath + @"\SaveFile.txt";

    public string ReturnJSONDataContainer(DataContainer dataContainer){
        return JsonUtility.ToJson(dataContainer);
    }

    public DataContainer GetDataContainerFromJSON(String json){
        // if (File.Exists(_filePath)){
        //     string jsonFile = File.ReadAllText(_filePath);
        //     return JsonUtility.FromJson<DataContainer>(jsonFile);
        // }
        // else{
        //     return new DataContainer();
        // }
        if (json == null){
            return new DataContainer();
        }
        else{
            return JsonUtility.FromJson<DataContainer>(json);
        }
    }
}