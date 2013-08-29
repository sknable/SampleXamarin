using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using SODA;


namespace DroidAgent
{
	[Activity (Label = "Droid Agent", MainLauncher = true)]
	public class MainActivity : Activity
	{
        ServiceConnection apiService = new ServiceConnection();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bundle"></param>
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);


			// Get our button from the layout resource,
			// and attach an event to it
			Button loginButton = FindViewById<Button> (Resource.Id.login);
            loginButton.Click += OnLoginClick;

            ComponentName name = StartService(new Intent(this, typeof(APIService)));


		}

        protected override void OnStart ()
        {
            base.OnStart();

            bool result = BindService(new Intent(this, typeof(APIService)), apiService, Bind.AutoCreate);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLoginClick(object sender, EventArgs e)
        {

            if (apiService.Binder != null)
            {
                SODAClient agent = apiService.Binder.Service._agent;
                if(agent.LoginAgent("", "", "", ""))
                {
                    apiService.Binder.Service.StartUpdating();
                    StartActivity(typeof(Agent));
                }

                
            }

        }
	}



}


