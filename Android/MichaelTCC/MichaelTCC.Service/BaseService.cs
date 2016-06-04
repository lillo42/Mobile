using System;

namespace MichaelTCC.Service
{
    public abstract class BaseService
    {
        public event EventHandler<string> OnNotifition;

        protected void InvokeOnNotifition(string e)
        {
            OnNotifition?.Invoke(this, e);
        }

        public virtual void Pause() { }
        public virtual void Start() { }
    }
}