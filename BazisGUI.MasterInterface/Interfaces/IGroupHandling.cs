namespace BazisGUI.MasterInterface.Interfaces
{
    public interface IGroupHandling// : IMasterInterface
    {
        /// <summary>
        /// Событие, вызывающее заполнение мастера группами.
        /// Должно срабатывать при запуске мастера
        /// </summary>
        event EventHandler<EventArgs> OnGroupsRequested;

        /// <summary>
        /// Обработка события добавления группы. Добавление в словарь группы, после ее создания при открытом мастере.
        /// Необходимо для корректного использования новой группы в процессе создания строк для формирования граничных условий
        /// </summary>
        /// <param name="type">Тип группы</param>
        /// <param name="number">Номер группы</param>
        /// <param name="groupName">Имя группы</param>
        void AddGroup(GroupType type, int number, string groupName);

        /// <summary>
        /// Обработка события переименования группы. Изменение в словаре и в сформированных строках определенной группы (переименование) при открытом мастере.
        /// Необходимо для корректного использования измененной группы в процессе создания строк для формирования граничных условий
        /// </summary>
        /// <param name="type">Тип группы</param>
        /// <param name="number">Номер группы</param>
        /// <param name="newName">Новое имя группы</param>
        void RenameGroup(GroupType type, int number, string newName);

        /// <summary>
        /// Обработка события удаления группы. Удаление из словаря и из сформированных строк определенной группы при открытом мастере.
        /// Необходимо для избавления от упоминаний удаленной группы в процессе создания строк для формирования граничных условий
        /// </summary>
        /// <param name="type">Тип группы</param>
        /// <param name="number">Номер группы</param>
        void DeleteGroup(GroupType type, int number);

        /// <summary>
        /// Обработка события удаления всех групп. Удаление из словаря всех названий и все сформированные строки мастера.
        /// </summary>
        void DeleteAllGroups();

        /// <summary>
        /// Стартовое заполнение группами из проекта.
        /// Необходимо для инициализации мастераы
        /// </summary>
        /// <param name="groups">Словарь с группами (индекс, имя), разделенными по типу</param>
        void InitialGroupFilling(Dictionary<GroupType, Dictionary<int, string>> groups);
    }
}
