using Android.Content;
using Android.Webkit;
using Java.Interop;
using MichaelTCC.Domain;
using MichaelTCC.Domain.Protocol;
using MichaelTCC.Infrastructure.Protocol;

namespace MichaelTCC.WebInterface
{
    public class DadosWebInterface : BaseWebInterface
    {
        public DadosWebInterface(Context context) : base(context)
        {
        }

        [Export]
        [JavascriptInterface]
        public bool podeAtualiza()
        {
            return false;
        }

        [Export]
        [JavascriptInterface]
        public DataReceiveProtocol getDataReceive()
        {
            return Core.Instance.DataReceive;
        }
    }
}