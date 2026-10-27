using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazisGUI.MasterInterface.Interfaces
{
    public interface IMaterialsHandling : IMasterInterface
    {
        /// <summary>
        /// Событие запроса заполнения мастера материалами со стороны мастера для осуществления инициализации
        /// </summary>
        event EventHandler<EventArgs> OnMaterialsRequested;

        /// <summary>
        /// Заполнение материалов мастера
        /// </summary>
        /// <param name="materials">Имена материалов</param>
        [Warning("Изменение набора материалов приведет к удалению уже созданных строк для формирования граничных условий")]
        void SetMaterials(IEnumerable<string> materials);
    }
}
