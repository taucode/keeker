using Keeker.Core.Data;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class EditHeaderDialog : Form
    {
        private string _name;
        private string _value;

        private string _caption;

        public EditHeaderDialog()
        {
            InitializeComponent();
        }

        public Dictionary<string, List<string>> HeaderValuesSource { get; set; }

        public HttpHeader CreateHeader()
        {
            _caption = "Create Header";
            var dres = this.ShowDialog();

            if (dres == DialogResult.OK)
            {
                var header = new HttpHeader(_name, _value);
                return header;
            }
            else
            {
                return null;
            }
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            _name = comboBoxName.Text;
            _value = comboBoxValue.Text;

            if (
                _name != _name.Trim() || 
                _value != _value.Trim() ||
                string.IsNullOrWhiteSpace(_name) ||
                string.IsNullOrWhiteSpace(_value))
            {
                MessageBox.Show("Wrong name or value", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void EditHeaderDialog_Load(object sender, EventArgs e)
        {
            this.Text = _caption;
        }
    }
}
