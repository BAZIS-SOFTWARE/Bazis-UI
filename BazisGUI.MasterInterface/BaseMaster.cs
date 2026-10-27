using BazisGUI.MasterInterface.Args;
using BazisGUI.MasterInterface.Interfaces;

namespace BazisGUI.MasterInterface
{
    public partial class BaseMaster : UserControl, IBaseMaster
    {
        /// <summary>
        /// Делегат для выполнения команд. Подключается к интерпретатору.
        /// </summary>
        private Func<string, Task<string>>? executeCommand;

        public event EventHandler<PrintInfoEventArgs> PrintInfoEvent;
        public event EventHandler<GenerateConditionsEventArgs> GenerateConditionsEvent;
        public event EventHandler<UpdateSceneEventArgs> UpdateSceneEvent;
        public event EventHandler<EventArgs> OnMasterLoaded;

        public virtual string MasterName { get; } = "BaseMaster";

        protected void RaiseGenerateConditionsEvent(string[] strings) =>
            GenerateConditionsEvent?.Invoke(this, new GenerateConditionsEventArgs(strings));

        protected void RaisePrintInfoEvent(string str, Color color) => 
            PrintInfoEvent?.Invoke(this, new PrintInfoEventArgs(str, color));
        

        protected void RaiseUpdateSceneEvent() =>
            UpdateSceneEvent?.Invoke(this, new UpdateSceneEventArgs());
        

        public void SetCommandExecutor(Func<string, Task<string>> executeCommand) => 
            this.executeCommand = executeCommand;
        

        public Task<string> SendCommandAsync(string command) =>
            executeCommand(command);
        
        public BaseMaster()
        {
            InitializeComponent();
            OnMasterLoaded?.Invoke(this, new EventArgs());
        }
    }
}
