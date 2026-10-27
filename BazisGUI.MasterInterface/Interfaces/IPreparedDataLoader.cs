using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.MasterInterface.Interfaces
{
    [Warning("Пока не использовать, так как заполнение данных это мат. , фун. и группы")]
    public interface IPreparedDataLoader : IMasterInterface
    {
        /// <summary>
        /// Событие, необходимое для заполнения мастера при его загрузке
        /// </summary>
        event EventHandler<EventArgs> PreparedDataLoaded;

        /// <summary>
        /// Заполнение мастера данными условий из проекта
        /// </summary>
        /// <param name="conditions">Набор строковых представлений условий</param>
        void SetDataFromConditionsStrings(IEnumerable<string> conditions);
    }
}
