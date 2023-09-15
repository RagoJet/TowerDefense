public interface ISaveable{
    void WriteDataToContainer();
    void LoadDataFromContainer();

    void SetDataContainer(DataContainer dataContainer);
}