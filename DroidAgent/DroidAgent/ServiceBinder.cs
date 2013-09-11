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
    public class ServiceBinder<T> : Binder where T : Service
    {
        /// <summary>
        /// Gets a reference to the Service
        /// </summary>
        /// <value>The service.</value>
        public T Service
        {
            get { return this.service; }
        }

        protected T service;

        /// <summary>
        /// Whether or not the service has been connected/bound.
        /// </summary>
        /// <value><c>true</c> if this instance is bound; otherwise, <c>false</c>.</value>
        public bool IsBound { get; set; }

        public ServiceBinder(T service)
        {
            this.service = service;
        }
    }
}