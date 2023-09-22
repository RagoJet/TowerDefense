using System.IO;
using UnityEngine;

public class SaveLoadController{
    private string _filePath = Application.persistentDataPath + @"\SaveFile.txt";

    public void SaveDataContainer(DataContainer dataContainer){
        string json = JsonUtility.ToJson(dataContainer);

        File.WriteAllText(_filePath, json);
    }

    public DataContainer GetDataContainer(){
        if (File.Exists(_filePath)){
            string jsonFile = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<DataContainer>(jsonFile);
        }
        else{
            return new DataContainer();
        }
    }
}