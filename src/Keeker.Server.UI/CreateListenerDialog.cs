using Keeker.Core;
using Keeker.Core.Listeners;
using System;
using System.Windows.Forms;

namespace Keeker.Server.UI
{
    public partial class CreateListenerDialog : Form
    {
        private string _endpoint;
        private IStreamListener _streamListener;

        public CreateListenerDialog()
        {
            InitializeComponent();
        }

        public IStreamListener ShowCreateListener(string initialEndpoint = null)
        {
            _endpoint = initialEndpoint;
            _streamListener = null;

            var dialogResult = this.ShowDialog();

            return dialogResult == DialogResult.OK ? _streamListener : null;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (_endpoint == null)
                {
                    throw new ApplicationException();
                }

                var ipEndPoint = CoreHelper.TryParseIPEndPoint(_endpoint);

                if (ipEndPoint != null)
                {
                    throw new NotImplementedException();
                }

                var linkEndpointPort = CoreHelper.TryParseLinkEndpoint(_endpoint);
                if (linkEndpointPort != null)
                {
                    _streamListener = new LinkStreamListener(linkEndpointPort.Value);
                    this.DialogResult = DialogResult.OK;
                    return;
                }

                throw new ApplicationException();
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
