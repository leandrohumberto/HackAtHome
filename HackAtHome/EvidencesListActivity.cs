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
using HackAtHome.Entities;
using HackAtHome.SAL;
using HackAtHome.Fragments;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/icon")]
    public class EvidencesListActivity : Activity
    {
        private string _fullName = string.Empty;
        private string _token = string.Empty;
        private EvidencesFragment _evidences;
        private readonly ServiceClient _serviceClient = new ServiceClient();

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EvidencesList);

            // Create your application here
            var authData = Intent.GetStringArrayListExtra("auth_data");

            if (authData != null && authData.Count >= 2)
            {
                _fullName = authData[0];
                _token = authData[1];
            }

            FindViewById<TextView>(Resource.Id.textViewFullName).Text = _fullName;
            var listView = FindViewById<ListView>(Resource.Id.listViewEvidences);
            _evidences = (EvidencesFragment)FragmentManager.FindFragmentByTag("Evidences");

            if (_evidences == null)
            {
                _evidences = new EvidencesFragment();
                _evidences.Evidences = await _serviceClient.GetEvidencesAsync(_token);
                var fragmentTransaction = FragmentManager.BeginTransaction();
                fragmentTransaction.Add(_evidences, "Evidences");
                fragmentTransaction.Commit();
            }

            listView.Adapter = new CustomAdapters.EvidencesAdapter(this, _evidences.Evidences, Resource.Layout.ListItem, 
                Resource.Id.textViewEvidenceTitle, Resource.Id.textViewEvidenceStatus);

            listView.ItemClick += async (IntentSender, e) =>
            {
                var evidenceDetail = await _serviceClient.GetEvidenceByIDAsync(_token, 
                    _evidences.Evidences[e.Position].EvidenceID);

                if (evidenceDetail != null)
                {
                    var activityIntent = new Intent(this, typeof(EvidenceDetailActivity));
                    activityIntent.PutStringArrayListExtra("evidence_data", new List<string>
                    {
                        _fullName,
                        _evidences.Evidences[e.Position].Title,
                        evidenceDetail.Description,
                        evidenceDetail.Url,
                        _evidences.Evidences[e.Position].Status,
                    });
                    StartActivity(activityIntent);
                }
            };
        }
    }
}