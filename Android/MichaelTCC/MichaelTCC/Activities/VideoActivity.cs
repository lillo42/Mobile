
using Android.App;
using Android.Net;
using Android.OS;
using Android.Widget;

namespace MichaelTCC.Activities
{
    [Activity(Label = "VideoActivity")]
    public class VideoActivity : Activity
    {
        private string _url;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                _url = savedInstanceState.GetString("Url");
            }
            catch
            {
                _url = string.Empty;
            }

            SetContentView(Resource.Layout.fragment_video);


            if (!string.IsNullOrEmpty(_url))
            {
                Uri uri = Uri.Parse(_url);

                var v1 = FindViewById<VideoView>(Resource.Id.videoView1);
                v1.SetVideoURI(uri);

                var v2 = FindViewById<VideoView>(Resource.Id.videoView2);
                v2.SetVideoURI(uri);

                v1.Start();
                v2.Start();
            }
        }
    }
}