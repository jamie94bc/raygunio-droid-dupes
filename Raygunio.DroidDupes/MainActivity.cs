using System;
using Android.App;
using Android.Runtime;
using Android.Widget;
using Android.OS;
using Android.Util;

namespace Raygunio.DroidDupes {
    [Activity(Label = "Raygunio.DroidDupes", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        // EXPECTED OUTPUT IN LOGCAT:
        // D/RaygunDroidDupes: 'System.Exception' caught from AndroidEnvironment
        // D/RaygunDroidDupes: 'Java.Lang.RuntimeException' caught from AppDomain

        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // What Raygun does in RaygunClient.Attach
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;

            SetContentView(Resource.Layout.Main);

            this.FindViewById<Button>(Resource.Id.Button).Click += MainActivity_Click;
        }

        private void MainActivity_Click(object sender, EventArgs e) {
            throw new Exception("I'm a test exception");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            var ex = e.ExceptionObject as Exception;
            if (ex != null) {
                LogException("AppDomain", ex);
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e) {
            if (e.Exception != null) {
                LogException("AndroidEnvironment", e.Exception);
            }
        }

        private void LogException(string from, Exception ex) {
            Log.Debug("RaygunDroidDupes", $"'{ex.GetType()}' caught from {from}");
        }
    }
}

