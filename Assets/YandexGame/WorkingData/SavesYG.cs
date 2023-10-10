using System.Collections.Generic;

namespace YG{
    [System.Serializable]
    public class SavesYG{
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1; // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        public int currentLevel = 1;

        public List<CellsInformation> cellsInformation = new List<CellsInformation>();

        public int gold = 20;
        public int priceWeapon = 15;
        public int priceHealth = 10;

        public int maxHealthKing = 12;
        public int levelShopWeapon = 1;
        public int maxLevelOfCreatedWeapon = 1;

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG(){
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            openLevels[1] = true;
        }
    }
}