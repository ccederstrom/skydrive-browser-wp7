using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Live;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using SkyDriveBrowser.SkyDriveModels;
using Newtonsoft;
using Newtonsoft.Json;
using SkyDriveBrowser.Controller;
using Microsoft.Live.Controls;
using SkyDriveBrowser.Service;
using SkyDriveBrowser.Service.Impl;
using SkyDriveBrowser.DAO.Impl;
using System.Windows.Media.Imaging;



namespace SkyDriveBrowser
{
    /// <summary>
    /// Used to indicate what data is being returned by a particular HTTP request
    /// </summary>
    public enum Results { UserInfo, FileInfo };


    public partial class MainPage : PhoneApplicationPage
    {
        private string parentFolderId;
        FileInfo currentDirectoryFileInfo;
        Files files;
        FileService fileService;

        private LiveConnectClient client ;
        private LiveConnectSession session;

        SkyDriveController skyDriveController;
        SkyDriveFileDao skyDriveFileDao;

        #region Intialization
        public MainPage()
        {
            InitializeComponent();

            Uri uri = new Uri("http://appserver.m.bing.net/BackgroundImageService/TodayImageService.svc/GetTodayImage?dateOffset=0&urlEncodeHeaders=true&osName=wince&osVersion=7.10&orientation=480x800&deviceName=WP7Device&mkt=en-US&AppId=1", UriKind.Absolute);
            BackgroundImage.Source = new BitmapImage(uri);

            // Set the data context of the listbox control to the sample data
            DataContext = App.FileViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            // UI initalization
            SearchBox_LoseFocus(); // Hide search and give page focus

            // Data Access Object initalization
            files = new Files();
            skyDriveController = new SkyDriveController();
            skyDriveController.Status = infoTextBlock;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.FileViewModel.IsDataLoaded)
            {
                App.FileViewModel.LoadData();
            }
        }

        #endregion
        /// <summary>
        /// Initialize Components and UI.
        /// </summary>




        /// <summary>
        /// Login to SkyDrive account
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signInButton_SessionChanged(object sender, LiveConnectSessionChangedEventArgs e)
        {
            infoTextBlock.Text = skyDriveController.SignIn_SessionChanged(sender, e);
            // set the current client and session
            client = skyDriveController.Client;
            session = skyDriveController.Session;
        }



       /// <summary>
       /// Selecting an object in the ListBox
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void StackPanel_Tap(object sender, GestureEventArgs e)
        {
            //  var content = (TextBlock)sender;
            object item = ((StackPanel)sender).DataContext;
            FileInfo fileInfo = (FileInfo)item;
            currentDirectoryFileInfo = fileInfo;

            Debug.WriteLine("NAME: " + fileInfo.Name + "ID: " + fileInfo.Id);

            if ((session != null) && (DateTimeOffset.Now < session.Expires))
            {
             //   parentFolderId = fileInfo.ParentId;
                skyDriveFileDao = new SkyDriveFileDao(session);
                skyDriveFileDao.TraverseDirectory(fileInfo);
                skyDriveFileDao.FilesListBox = FirstListBox;
            }
            else
            {
                Debug.WriteLine("You must sign in first.");
            }
        }

        /// <summary>
        /// Phone Back Key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnBackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SearchBox_LoseFocus(); // Hide search box
            if (currentDirectoryFileInfo != null)
            {
                // display parent directory
                e.Cancel = true; // dont exit page/app
                if (session != null && (DateTimeOffset.Now < session.Expires))
                {
                    skyDriveFileDao = new SkyDriveFileDao(session);
                    skyDriveFileDao.TraverseParentDirectory(currentDirectoryFileInfo);
                    skyDriveFileDao.FilesListBox = FirstListBox;
                }
                else
                {
                    Debug.WriteLine("You must sign in first.");
                }
            }
            else
            {
                // exit page
            }
        }


        /// <summary>
        /// Hide Search Box and give page focus
        /// </summary>
        private void SearchBox_LoseFocus()
        {
            MainPagePivot.Focus(); // loose focus of SearchTextBox 
            SearchTextBox.Visibility = System.Windows.Visibility.Collapsed;
        }

        #region Application Bar Controls

        /// <summary>
        /// Application bar Home click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Home_Click(object sender, EventArgs e)
        {
            SearchBox_LoseFocus();

            if (session != null)
            {
                skyDriveFileDao = new SkyDriveFileDao(session);
                skyDriveFileDao.TraverseDirectory();
                skyDriveFileDao.FilesListBox = FirstListBox;
            }
        }

        /// <summary>
        /// Application bar Refresh click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Click(object sender, EventArgs e)
        {
            SearchBox_LoseFocus();

            if (session != null && (DateTimeOffset.Now < session.Expires))
            {
                skyDriveFileDao = new SkyDriveFileDao(session);
                skyDriveFileDao.TraverseDirectory(currentDirectoryFileInfo);
            }
            else
            {
                Debug.WriteLine("You must sign in first.");
            }
        }

        /// <summary>
        /// Application bar Search click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_Click(object sender, EventArgs e)
        {
            SearchTextBox.Visibility = System.Windows.Visibility.Visible;
            SearchTextBox.Focus();

        }
        #endregion

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search(SearchTextBox.Text);
                SearchBox_LoseFocus();
            }
        }


        private void Search()
        {
            if (session != null && (DateTimeOffset.Now < session.Expires))
            {
                skyDriveFileDao = new SkyDriveFileDao(session);
                // todo need to SEARCH
                skyDriveFileDao.TraverseDirectory(currentDirectoryFileInfo);
            }
            else
            {
                Debug.WriteLine("You must sign in first.");
            }
        }

        private void Search(string searchTerm)
        {
            if (session != null && (DateTimeOffset.Now < session.Expires))
            {
                skyDriveFileDao = new SkyDriveFileDao(session);
                // todo need to SEARCH
                skyDriveFileDao.TraverseDirectory(searchTerm);
            }
            else
            {
                Debug.WriteLine("You must sign in first.");
            }
        }
    }
}