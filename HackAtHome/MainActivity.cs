using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.Entities;
using HackAtHome.SAL;
using Android.Content;
using System.Collections.Generic;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.buttonValidate).Click += async (sender, e) =>
            {
                var email = FindViewById<TextView>(Resource.Id.editTextEmail).Text;
                var password = FindViewById<TextView>(Resource.Id.editTextPassword).Text;
                ServiceClient client = new ServiceClient();
                var result = await client.AutenticateAsync(email, password);

                if (result.Status == Status.AllSuccess || result.Status == Status.Success)
                {
                    SendEvidence(email);
                    var activityIntent = new Intent(this, typeof(EvidencesListActivity));
                    activityIntent.PutStringArrayListExtra("auth_data", new List<string>
                    {
                        result.FullName, 
                        result.Token,
                    });
                    StartActivity(activityIntent);
                }
                else
                {
                    var dialog = new AlertDialog.Builder(this).Create();
                    dialog.SetButton("OK", delegate { });
                    dialog.SetMessage(result.Status.ToString());
                    dialog.SetTitle("Error");
                    dialog.SetIcon(Android.Resource.Drawable.StatNotifyError);
                    dialog.Show();
                }
            };
        }

        private async void SendEvidence(string email)
        {
            MicrosoftServiceClient client = new MicrosoftServiceClient();
            await client.SendEvidence(new LabItem { Email = email, Lab = "Hack@Home", DeviceId =
                Android.Provider.Settings.Secure.GetString(ContentResolver, Android.Provider.Settings.Secure.AndroidId)});
        }
    }
}

