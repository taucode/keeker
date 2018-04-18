using Keeker.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.InitMethods();
        }

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

            if (parsed)
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
    }
}
