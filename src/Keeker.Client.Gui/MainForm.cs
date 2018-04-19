﻿using Keeker.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Keeker.Client.Gui
{
    public partial class MainForm : Form
    {
        private bool _autoApplyUri;

        public MainForm()
        {
            InitializeComponent();

            _autoApplyUri = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.InitMethods();

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

            // ASSERT
            //this.DoSettingsAssert();
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
                this.SaveSettings();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
    }
}