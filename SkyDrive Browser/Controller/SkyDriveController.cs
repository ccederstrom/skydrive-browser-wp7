using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Live;
using Microsoft.Live.Controls;

namespace SkyDriveBrowser.Controller
{
    public class SkyDriveController
    {
        private TextBlock status;
        public TextBlock Status
        {
            get { return status; }
            set { status = value; }
        }
        private LiveConnectClient client;
        private LiveConnectSession session;
        
        public LiveConnectClient Client
        {
            get
            {
                return client;
            }
            set
            {
                client = value;
            }
        }
        public LiveConnectSession Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
            }
        }

        string message;

        public SkyDriveController(){}
        public SkyDriveController(LiveConnectClient client)
        {
            Client = client;
        }

        public string SignIn_SessionChanged(object sender, LiveConnectSessionChangedEventArgs e)
        {
            if (e.Status == LiveConnectSessionStatus.Connected)
            {
                session = e.Session;
                client = new LiveConnectClient(e.Session);
                message = "Signed in.";
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(OnSignIn_GetCompleted);
                client.GetAsync("me", null);
            }
            else
            {
                message = "Not signed in.";
                client = null;
            }
            return message;
        }

        void OnSignIn_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.ContainsKey("first_name") &&
                    e.Result.ContainsKey("last_name"))
                {
                    if (e.Result["first_name"] != null &&
                        e.Result["last_name"] != null)
                    {
                        message =
                            "Hello, " +
                            e.Result["first_name"].ToString() + " " +
                            e.Result["last_name"].ToString() + "!";
                    }
                }
                else
                {
                    message = "Hello, signed-in user!";
                }
            }
            else
            {
                message = "Error calling API: " +
                    e.Error.ToString();
            }
            status.Text = message;
        }


        void CheckPermissions_LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            string message;
            if (e.Status == LiveConnectSessionStatus.Connected)
            {
                //session = e.Session;
                //client = new LiveConnectClient(e.Session);
                message = "Signed in.";
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(CheckPermissions_GetCompleted);
                client.GetAsync("me/permissions");
            }
            else if (e.Error != null)
            {
                message = "Error signing in: " + e.Error.ToString();
            }
        }

        void CheckPermissions_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            string message;
            if (e.Error == null)
            {
                message = e.RawResult;
            }
            else
            {
                message = "Error calling API: " + e.Error.ToString();
            }
        }
    }
}
