using System;
using UnityEngine;

public class SaveLoadController{
    private string _filePath = Application.persistentDataPath + @"\SaveFile.txt";

    public string ReturnJSONDataContainer(DataContainer dataContainer){
        return JsonUtility.ToJson(dataContainer);
    }

    public DataContainer GetDataContainerFromJSON(String json){
        if (json == null){
            return new DataContainer();
        }
        else{
            return JsonUtility.FromJson<DataContainer>(json);
        }
    }


    // public void WRITEJSONDATACONTAINERTOFILE(DataContainer dataContainer){
    //     string json = JsonUtility.ToJson(dataContainer);
    //     File.WriteAllText(_filePath, json);
    // }

    // public DataContainer GETDATACONTAINERFROMJSONEDITOR(){
    //     if (File.Exists(_filePath)){
    //         string jsonFile = File.ReadAllText(_filePath);
    //         return JsonUtility.FromJson<DataContainer>(jsonFile);
    //     }
    //     else{
    //         return new DataContainer();
    //     }
    // }
}