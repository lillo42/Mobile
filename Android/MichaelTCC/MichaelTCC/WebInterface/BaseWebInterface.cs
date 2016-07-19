using Android.Content;

namespace MichaelTCC.WebInterface
{
    public abstract class BaseWebInterface : Java.Lang.Object
    {
        private readonly Context _context;

        public BaseWebInterface(Context context)
        {
            _context = context;
        }

        protected Context Context
        {
            get
            {
                return _context;
            }
        }
    }
}