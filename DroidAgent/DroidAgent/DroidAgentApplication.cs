using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SODA;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DroidAgent
{
    [Application]
    class DroidAgentApplication : Application
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="transfer"></param>
        public DroidAgentApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        public override void OnCreate ()
        {
            base.OnCreate ();


        }

    }
}