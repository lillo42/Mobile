using Android.Content;
using Android.Webkit;
using Java.Interop;
using MichaelTCC.Domain;

namespace MichaelTCC.WebInterface
{
    public class DadosWebInterface : BaseWebInterface
    {
        public DadosWebInterface(Context context) : base(context)
        {
        }

        [Export]
        [JavascriptInterface]
        public bool podeAtualizar()
        {
            return Core.Instance.JoystickBuilder.JoystickDTO.Triangle;
        }

        [Export]
        [JavascriptInterface]
        public string getDataReceive()
        {
            return Core.Instance.DataReceive.GetJsonDataProtocol();
        }
    }
}