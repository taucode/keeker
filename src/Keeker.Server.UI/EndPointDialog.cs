using Keeker.Core;
using System;
using System.Windows.Forms;

namespace Keeker.Server.UI
{
    public partial class EndPointDialog : Form
    {
        private string _endpoint;

        public EndPointDialog()
        {
            InitializeComponent();
        }

        public string ShowEndPoint(string initialEndpoint = null)
        {
            _endpoint = initialEndpoint;

            var dialogResult = this.ShowDialog();

            return dialogResult == DialogResult.OK ? _endpoint : null;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                _endpoint = comboBoxEndpoint.Text;

                if (_endpoint == null)
                {
                    throw new ApplicationException();
                }

                var valid =
                    CoreHelper.IsIPEndPoint(_endpoint) ||
                    CoreHelper.IsLinkEndPoint(_endpoint);

                if (!valid)
                {
                    throw new ApplicationException();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateListenerDialog_Load(object sender, EventArgs e)
        {
            if (_endpoint != null)
            {
                comboBoxEndpoint.Text = _endpoint;
            }
        }
    }
}
