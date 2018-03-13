using System.Linq;
using System.Windows.Forms;

namespace Keeker.Gui
{
    internal static class Helper
    {
        internal static ListViewItem GetSingleSelectedItem(this ListView listView)
        {
            var indexes = listView.SelectedIndices
                .Cast<int>()
                .ToArray();

            if (indexes.Length == 0)
            {
                return null;
            }

            return listView.Items[indexes.First()];
        }
    }
}
