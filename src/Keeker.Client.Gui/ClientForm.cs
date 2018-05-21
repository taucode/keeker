using Keeker.Client.Gui.Data;
using Keeker.Core;
using Keeker.Core.Data;
using Keeker.Core.Streams;
using Keeker.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class ClientForm : Form
    {
        private bool _autoApplyUri;

        private readonly IHttpClient _client;
        //private HttpServerForm _serverForm;

        public ClientForm()
        {
            InitializeComponent();

            _autoApplyUri = true;
        }

        public ClientForm(IHttpClient client)
            : this()
        {
            _client = client;
            _client.RawDataSent += client_RawDataSent;
            _client.RawDataReceived += client_RawDataReceived;
            _client.ResponseReceived += client_ResponseReceived;
        }

        private void DoInvoke(Action action)
        {
            this.Invoke(action);
        }

        private void client_RawDataSent(byte[] buffer)
        {
            var time = DateTime.Now;
            this.DoInvoke(() => raceChartPackets.AddEntry(new RaceChartEntry(new Packet(buffer, time)), 0));
        }

        private void client_RawDataReceived(byte[] buffer)
        {
            var time = DateTime.Now;
            this.DoInvoke(() => raceChartPackets.AddEntry(new RaceChartEntry(new Packet(buffer, time)), 1));
        }

        private void client_ResponseReceived(HttpResponseMetadata metadata, byte[] content)
        {
            Trace.TraceInformation(metadata.ToString());
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            this.InitMethods();

            raceChartPackets.InitParticipants(2);
            raceChartPackets.EntryComparer = ComparePackets;

            //comboBoxUri.Text = "https://rho.me/";
            //buttonApply_Click(sender, e);

            try
            {
                this.LoadSettings();
            }
            catch
            {
                // dismiss
            }

            //_linkServer = new LinkHttpServer(new[] { "rho.me", }, 1488);
            //_serverForm = new HttpServerForm(_linkServer);
            //_serverForm.Show();

            //this.StartLinkListening();
            //Thread.Sleep(50);

            //buttonConnect_Click(sender, e);

            // ASSERT
            //this.DoSettingsAssert();

            //var timer = new Timer();
            //timer.Interval = 100;
            //timer.Tick += (senderArg, eArg) =>
            //{
            //    timer.Dispose();
            //    this.buttonCreateServer_Click(sender, e);
            //};


            //timer.Start();

            //Helper.DoLater(() => this.buttonCreateServer_Click(sender, e), 100);
            //Helper.DoLater(() => SendKeys.Send("{ENTER}"), 200);
        }

        private static int ComparePackets(RaceChartEntry e1, RaceChartEntry e2)
        {
            var packet1 = (Packet)e1.Data;
            var packet2 = (Packet)e2.Data;

            return packet1.Time.CompareTo(packet2.Time);
        }


        //private void StartLinkListening()
        //{
        //    _linkServer = new LinkHttpServer(1488);
        //    _linkServer.Start();
        //}


        private void InitMethods()
        {
            var methods = new[]
            {
                "GET",
                "POST",
                "PUT",
                "PATCH",
                "DELETE",
            };

            comboBoxMethod.DataSource = methods;
            comboBoxMethod.SelectedIndex = 0;
        }

        private void comboBoxUri_TextChanged(object sender, EventArgs e)
        {
            bool parsed = Uri.TryCreate(comboBoxUri.Text, UriKind.Absolute, out var uri);

            if (parsed && _autoApplyUri)
            {
                var host = uri.Authority;
                this.SetRequestHeader("Host", host, 0);
            }
        }

        private void SetRequestHeader(string name, string value, int? index = null)
        {
            var header = this.GetRequestHeader(name);
            if (header == null)
            {
                header = new HttpHeader(name, value);
                this.AddRequestHeader(header, index);
            }
            else
            {
                var item = this.GetRequestHeaderItem(name);
                item.SubItems[1].Text = value;
            }
        }

        private ListViewItem GetRequestHeaderItem(string headerName)
        {
            return listViewHeaders.Items
                .Cast<ListViewItem>()
                .SingleOrDefault(x => x.Text == headerName);

        }

        private void AddRequestHeader(HttpHeader header, int? index)
        {
            if (index.HasValue)
            {
                var headers = this.GetRequestHeaders();
                headers.Insert(index.Value, header);
                this.SetRequestHeaders(headers);
            }
            else
            {
                listViewHeaders.Items.Add(this.CreateListViewItemFromHeader(header));
            }
        }

        private void SetRequestHeaders(List<HttpHeader> headers)
        {
            listViewHeaders.Items.Clear();
            foreach (var header in headers)
            {
                var item = this.CreateListViewItemFromHeader(header);
                listViewHeaders.Items.Add(item);
            }
        }

        private List<HttpHeader> GetRequestHeaders()
        {
            return listViewHeaders.Items
                .Cast<ListViewItem>()
                .Select(CreateHeaderFromListViewItem)
                .ToList();
        }

        private HttpHeader CreateHeaderFromListViewItem(ListViewItem item)
        {
            var name = item.Text;
            var value = item.SubItems[1].Text;

            return new HttpHeader(name, value);
        }

        private ListViewItem CreateListViewItemFromHeader(HttpHeader header)
        {
            var item = new ListViewItem(header.Name);
            item.SubItems.Add(header.Value);
            return item;
        }

        private HttpHeader GetRequestHeader(string name)
        {
            var item = listViewHeaders.Items
                .Cast<ListViewItem>()
                .SingleOrDefault(x => x.Text == name);

            if (item == null)
            {
                return null;
            }
            else
            {
                return this.CreateHeaderFromListViewItem(item);
            }
        }

        private void buttonAddHeader_Click(object sender, EventArgs e)
        {
            var dlg = new EditHeaderDialog();
            var header = dlg.CreateHeader();

            if (header != null)
            {
                this.SetRequestHeader(header.Name, header.Value, null);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoApply();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DoApply()
        {
            var uri = new Uri(comboBoxUri.Text);
            var url = uri.PathAndQuery;

            var line = new HttpRequestLine(new HttpMethod(comboBoxMethod.Text), url);
            var headers = new HttpHeaderCollection(listViewHeaders.Items
                .Cast<ListViewItem>()
                .Select(item => new HttpHeader(
                    item.SubItems[0].Text,
                    item.SubItems[1].Text)));

            var metadata = new HttpRequestMetadata(line, headers);

            var metadataText = metadata.ToString();
            textBoxRequestText.Text = metadataText;
        }

        private void SaveSettings()
        {
            var appSettings = new AppSettings
            {
                LastMethod = comboBoxMethod.Text,
                LastUri = comboBoxUri.Text,
                LastHeaders = this.GetRequestHeaders()
                    .Select(Helper.HttpHeaderToDto)
                    .ToList(),
            };

            Program.Instance.SaveSettings(appSettings);
        }

        private void LoadSettings()
        {
            var appSettings = Program.Instance.LoadSettings();
            if (appSettings != null)
            {
                this.ApplySettings(appSettings);
            }
        }

        private void ApplySettings(AppSettings appSettings)
        {
            _autoApplyUri = false;

            if (appSettings.LastMethod != null)
            {
                comboBoxMethod.Text = appSettings.LastMethod;
            }

            if (appSettings.LastUri != null)
            {
                comboBoxUri.Text = appSettings.LastUri;
            }

            if (appSettings.LastHeaders != null)
            {
                foreach (var headerDto in appSettings.LastHeaders)
                {
                    var header = new HttpHeader(headerDto.Name, headerDto.Value);
                    this.AddRequestHeader(header, null);
                }
            }

            _autoApplyUri = true;
        }

        private static void MustBeTrue(bool assertion)
        {
            if (!assertion)
            {
                throw new Exception(":(");
            }
        }

        private void DoSettingsAssert()
        {
            var assert1 = comboBoxMethod.Text == "POST";
            MustBeTrue(assert1);

            var assert2 = comboBoxUri.Text == "https://rho.me/";
            MustBeTrue(assert2);

            var headers = this.GetRequestHeaders();
            var assert3 = headers.Count == 1;
            MustBeTrue(assert3);

            var assert4 = headers[0].Name == "Host";
            var assert5 = headers[0].Value == "rho.me";

            MustBeTrue(assert4 && assert5);
        }

        private void AddClient(HttpClient client)
        {
            var stream = client.Stream;
            if (stream is LinkStream)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSendText_Click(object sender, EventArgs e)
        {
            try
            {
                this.DoSend();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DoSend()
        {
            var buffer = textBoxRequestText.Text.ToAsciiBytes();

            var metadata = HttpRequestMetadata.Parse(buffer, 0);
            var len = metadata.ToString().Length;
            var content = buffer.Skip(len).ToArray();

            _client.Send(metadata, content);
        }

        //private static bool IsIPEndpoint(string endPoint)
        //{
        //    var parts = endPoint.Split(':');
        //    if (parts.Length != 2)
        //    {
        //        return false;
        //    }

        //    if (!IPAddress.TryParse(parts[0], out var dummyIpAddress))
        //    {
        //        return false;
        //    }

        //    return int.TryParse(parts[1], out var dummyPort);
        //}

        //private static bool IsLinkEndpoint(string endPoint)
        //{
        //    return Regex.IsMatch(endPoint, @"link:\d+");
        //}
    }
}
