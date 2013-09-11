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
using SODA.CIMSystemService;

namespace DroidAgent
{
    [Activity(Label = "Droid Agent")]
    public class Agent : Activity
    {
        ServiceConnection _apiService = new ServiceConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.AgentLayout);


            Button statusButton = FindViewById<Button>(Resource.Id.status);
            Button logoutButton = FindViewById<Button>(Resource.Id.logout);
            Button takeButton = FindViewById<Button>(Resource.Id.take);

            statusButton.Click += OnStatusClick;
            logoutButton.Click += OnLogoutClick;
            takeButton.Click += OnTakeClick;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();

            bool result = BindService(new Intent(this, typeof(APIService)), _apiService, Bind.AutoCreate);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnStatusClick(object sender, EventArgs e)
        {

            if (_apiService.Binder != null)
            {
                SODAClient agent = _apiService.Binder.Service._agent;

                Button statusButton = FindViewById<Button>(Resource.Id.status);

                if (statusButton.Text == "Go Available")
                {
                    statusButton.Text = "Go To Break";
                    agent.CIM.changeAvailability(CIMAvailabilityState.AVAILABLE, true, "Ready");
                }
                else
                {
                    statusButton.Text = "Go Available";
                    agent.CIM.changeAvailability(CIMAvailabilityState.UNAVAILABLE, true, "Break");
                }


            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLogoutClick(object sender, EventArgs e)
        {

            if (_apiService.Binder != null)
            {

                SODAClient agent = _apiService.Binder.Service._agent;
                agent.CIM.logout();
                SetContentView(Resource.Layout.Main);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnTakeClick(object sender, EventArgs e)
        {

            if (_apiService.Binder != null)
            {
                SODAClient agent = _apiService.Binder.Service._agent;
                agent.CIM.takeNextInteraction();

            }

        }


    }
}