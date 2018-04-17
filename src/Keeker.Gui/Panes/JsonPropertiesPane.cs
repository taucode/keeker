using Newtonsoft.Json;

namespace Keeker.Gui.Panes
{
    public partial class JsonPropertiesPane : Pane
    {
        public JsonPropertiesPane()
        {
            InitializeComponent();
        }

        public JsonPropertiesPane(object value)
            : this()
        {
            this.Value = value;
        }

        public object Value { get; set; }

        private void JsonPropertiesPane_Load(object sender, System.EventArgs e)
        {
            var json = JsonConvert.SerializeObject(this.Value, Formatting.Indented);
            textBoxJson.Text = json;
        }
    }
}
