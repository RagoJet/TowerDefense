using System.Collections.Generic;

public class SaveablesObjects{
    public List<ISaveable> _SaveablesList = new List<ISaveable>();

    private DataContainer _dataContainer;

    public SaveablesObjects(DataContainer dataContainer, params ISaveable[] saveable){
        _dataContainer = dataContainer;
        foreach (var iSaveable in saveable){
            _SaveablesList.Add(iSaveable);
            iSaveable.SetDataContainer(_dataContainer);
        }
    }

    public void WriteAllDataToContainer(){
        foreach (var saveable in _SaveablesList){
            saveable.WriteDataToContainer();
        }
    }

    public void LoadAllDataFromContainer(){
        foreach (var saveable in _SaveablesList){
            saveable.LoadDataFromContainer();
        }
    }
}