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
using SODA;

namespace DroidAgent
{
    [Service]
    class APIService : Service
    {

        public SODAClient _agent;
        /// <summary>
        /// 
        /// </summary>
        public override void OnCreate()
        {
            base.OnCreate();
            _agent = new SODAClient();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intent"></param>
        /// <param name="flags"></param>
        /// <param name="startId"></param>
        /// <returns></returns>
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="intent"></param>
        /// <returns></returns>
        public override IBinder OnBind(Intent intent)
        {

            return new ServiceBinder<APIService>(this);
        }
        /// <summary>
        /// 
        /// </summary>
        public void StartUpdating()
        {

            _agent.Run();

        }
        /// <summary>
        /// 
        /// </summary>
        public void StopUpdating()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDestroy()
        {
            
            base.OnDestroy();
            // cleanup code
        }

    }
}