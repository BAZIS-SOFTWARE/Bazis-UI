using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.MasterInterface.Interfaces
{
    public interface IFunctionsHandling : IMasterInterface
    {
        /// <summary>
        /// Событие запроса заполнения мастера функциями со стороны мастера для осуществуления инициализации
        /// </summary>
        event EventHandler<EventArgs> OnFunctionsRequested;

        /// <summary>
        /// Заполнение функций мастера
        /// </summary>
        /// <param name="functions">Названия функций</param>
        [Warning("Изменение набора функций приведет к удалению уже созданных строк для формирования граничных условий")]
        void SetFunctions(IEnumerable<string> functions);
    }
}
