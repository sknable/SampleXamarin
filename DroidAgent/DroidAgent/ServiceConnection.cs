using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DroidAgent
{
    class ServiceConnection : Java.Lang.Object, IServiceConnection
    {
        ServiceBinder<APIService> _binder;
        /// <summary>
        /// 
        /// </summary>
        public ServiceBinder<APIService> Binder
        {
            get
            {
                return _binder;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="service"></param>
        public void OnServiceConnected(ComponentName name, IBinder service)
        {
            var serviceBinder = service as ServiceBinder<APIService>;
            _binder = serviceBinder;
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void OnServiceDisconnected(ComponentName name)
        {

        }
    }
}