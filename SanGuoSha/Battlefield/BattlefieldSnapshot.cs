using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using SanGuoSha.BaseClass;


namespace SanGuoSha.Battlefield
{
    public partial class Battlefield
    {
        public string Snapshot
        {
            get;
            protected set;
        } = string.Empty;

        public override void UpdateSnapshot(AskAnswer? aAsk = null)
        {
            Snapshot = string.Empty;
        }
    }
}
