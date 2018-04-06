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

        //internal static ListenerConfDto ToListenerConfDto(this ListenerPlainConf conf)
        //{
        //    return new ListenerConfDto
        //    {
        //        Id = conf.Id,
        //        EndPoint = conf.EndPoint.ToString(),
        //        IsHttps = conf.IsHttps,
        //        Hosts = conf.Hosts.ToDictionary(x => x.Key, x => x.Value.ToHostConfDto()),
        //    };
        //}

        //internal static HostConfDto ToHostConfDto(this HostPlainConf conf)
        //{
        //    return new HostConfDto
        //    {
        //        ExternalHostName = conf.ExternalHostName,
        //        DomesticHostName = conf.DomesticHostName,
        //        EndPoint = conf.EndPoint.ToString(),
        //        CertificateId = conf.CertificateId,
        //    };
        //}
    }
}
