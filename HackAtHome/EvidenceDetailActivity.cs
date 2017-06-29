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
using Android.Webkit;
using Android.Text;

namespace HackAtHome
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = false, Icon = "@drawable/icon")]
    public class EvidenceDetailActivity : Activity
    {
        private string _fullName = string.Empty;
        private string _title = string.Empty;
        private string _description = string.Empty;
        private string _url = string.Empty;
        private string _status = string.Empty;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.EvidenceDetail);

            // Create your application here
            var evidenceData = Intent.GetStringArrayListExtra("evidence_data");

            if (evidenceData != null && evidenceData.Count >= 5)
            {
                _fullName = evidenceData[0];
                _title = evidenceData[1];
                _description = evidenceData[2];
                _url = evidenceData[3];
                _status = evidenceData[4];
            }

            FindViewById<TextView>(Resource.Id.textViewFullNameDetail).Text = _fullName;
            FindViewById<TextView>(Resource.Id.textViewEvidenceTitleDetail).Text = _title;
            FindViewById<TextView>(Resource.Id.textViewEvidenceStatusDetail).Text = _status;
            FindViewById<TextView>(Resource.Id.textViewEvidenceDescriptionDetail).Text = _description;
            Koush.UrlImageViewHelper.SetUrlDrawable(FindViewById<ImageView>(Resource.Id.imageViewEvidence), _url);
        }
    }
}