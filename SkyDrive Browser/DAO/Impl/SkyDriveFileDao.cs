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
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using SkyDriveBrowser.SkyDriveModels;
using Newtonsoft.Json;


namespace SkyDriveBrowser.DAO.Impl
{
    public class SkyDriveFileDao : ISkyDriveFileDao
    {

        private SkyDriveBrowser.SkyDriveModels.FileInfo currentFileInfo;
        public SkyDriveBrowser.SkyDriveModels.FileInfo CurrentFileInfo
        {
            get
            {
                return currentFileInfo;
            }
            set
            {
                if (value != currentFileInfo)
                {
                    currentFileInfo = value;
                }
            }
        }

        private ListBox filesListBox;
        public ListBox FilesListBox
        {
            get { return filesListBox; }
            set { filesListBox = value; }
        }
        private LiveConnectClient client;
        private LiveConnectSession session;
        private Files files;
        public Files Files
        {
            get { return files; }
            set { files = value; }
        }
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
                return Session;
            }
            set
            {
                Session = value;
            }
        }

        public SkyDriveFileDao()
        {
            if(session==null)
                Debug.WriteLine("Need to set a session.");
        }
        public SkyDriveFileDao(LiveConnectSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561489.aspx#dir
        /// </summary>
        public void TraverseDirectory()
        {
            string message;
            if (session == null)
            {
                message = "You must sign in first.";
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(TraverseDirectory_GetCompleted);
                    client.GetAsync("me/skydrive/files");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561489.aspx#dir
        /// </summary>
        public void TraverseDirectory(SkyDriveBrowser.SkyDriveModels.FileInfo fileInfo)
        {
            string message;
            if (session == null)
            {
                message = "You must sign in first.";
            }
            else
            {
                if (fileInfo != null)
                {
                    LiveConnectClient client = new LiveConnectClient(session);
                    client.GetCompleted +=
                        new EventHandler<LiveOperationCompletedEventArgs>(TraverseDirectory_GetCompleted);
                    client.GetAsync(fileInfo.Id + "/files");
                }
                else
                {
                    Debug.WriteLine("File Information is null.");
                    message = "File Information is null.";
                }
            }
        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561489.aspx#dir
        /// </summary>
        public void TraverseDirectory(string searchTerm)
        {
            string message;
            if (session == null)
            {
                message = "You must sign in first.";
            }
            else
            {
                if (searchTerm != null)
                {
                    LiveConnectClient client = new LiveConnectClient(session);
                    client.GetCompleted +=
                        new EventHandler<LiveOperationCompletedEventArgs>(TraverseDirectory_GetCompleted);
                    client.GetAsync(searchTerm + "/files");
                }
                else
                {
                    Debug.WriteLine("File Information is null.");
                    message = "File Information is null.";
                }
            }
        }




        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561489.aspx#dir
        /// </summary>
        public void TraverseParentDirectory(SkyDriveBrowser.SkyDriveModels.FileInfo fileInfo)
        {
            string message;
            if (session == null)
            {
                message = "You must sign in first.";
            }
            else
            {
                if (fileInfo != null)
                {
                    LiveConnectClient client = new LiveConnectClient(session);
                    client.GetCompleted +=
                        new EventHandler<LiveOperationCompletedEventArgs>(TraverseDirectory_GetCompleted);
                    client.GetAsync(fileInfo.ParentId + "/files");
                }
                else
                {
                    Debug.WriteLine("File Information is null.");
                    message = "File Information is null.";
                }
            }
        }







        void TraverseDirectory_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            string message;
            if (e.Error == null)
            {
                // Map query 
                files = JsonConvert.DeserializeObject<Files>(e.RawResult);
                Debug.WriteLine(e.RawResult);
                foreach (SkyDriveBrowser.SkyDriveModels.FileInfo fileInfo in files.data)
                {
          //          Debug.WriteLine(fileInfo.ToString());
                    Debug.WriteLine(fileInfo.Id.ToString());
                    Debug.WriteLine(fileInfo.Name.ToString());
                    Debug.WriteLine(fileInfo.Type.ToString());
                    //Debug.WriteLine(fileInfo.Description.ToString
                }
                filesListBox.ItemsSource = files.data;
            }
            else
            {
                message = "Error calling API: " + e.Error.Message;
                Debug.WriteLine("Error calling API: " + e.Error.Message);
            }
        }



        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#reading_folders
        /// The wl.skydrive scope is required.
        /// </summary>
        public void GetFolderProperties()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(GetFolderProperties_GetCompleted);
                client.GetAsync("folder.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!110");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#reading_folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetFolderProperties_GetCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                IDictionary<string, object> folder = e.Result;
                Debug.WriteLine("Folder name: " + folder["name"].ToString() +
                    "\nID: " + folder["id"].ToString());
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.Message);
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#deleting_folders
        /// The wl.skydrive_update scope is required.
        /// </summary>
        public void DeleteFileOrFolder()
        {

            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.DeleteCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(DeleteFileOrFolder_Completed);
                client.DeleteAsync("folder.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!147");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#deleting_folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DeleteFileOrFolder_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("File or folder deleted.");
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#creating_folders
        /// To add a new folder
        /// </summary>
        public void CreateFolder()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                Dictionary<string, object> folderData = new Dictionary<string, object>();
                folderData.Add("name", "A brand new folder");
                LiveConnectClient client = new LiveConnectClient(session);
                client.PostCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(CreateFolder_Completed);
                client.PostAsync("me/skydrive", folderData);
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#creating_folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CreateFolder_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Folder created.");
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#updating_folders
        /// The wl.skydrive_update scope is required.
        /// </summary>
        public void RenameFolder()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                Dictionary<string, object> folderData = new Dictionary<string, object>();
                folderData.Add("name", "This folder is renamed");
                LiveConnectClient client = new LiveConnectClient(session);
                client.PutCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(RenameFolder_Completed);
                client.PutAsync("folder.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!145", folderData);
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#updating_folders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RenameFolder_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Folder renamed.");
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#reading_files_props
        /// The wl.skydrive scope is required.
        /// </summary>
        public void GetFileProperties()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(getFileProperties_Completed);
                client.GetAsync("file.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!131");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#reading_files_props
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void getFileProperties_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                IDictionary<string, object> file = e.Result;
                Debug.WriteLine("File name: " + file["name"].ToString() +
                    "\nID: " + file["id"].ToString());
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#updating_files_props
        /// The wl.skydrive_update scope is required.
        /// </summary>
        public void RenameFile()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                Dictionary<string, object> fileData = new Dictionary<string, object>();
                fileData.Add("name", "Grocery List 2.docx");
                LiveConnectClient client = new LiveConnectClient(session);
                client.PutCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(RenameFile_Completed);
                client.PutAsync("file.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!119", fileData);
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#updating_files_props
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RenameFile_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("File renamed.");
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#downloading_files
        /// The wl.skydrive scope is required.
        /// </summary>
        public void DownloadFile()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.DownloadCompleted += new EventHandler<LiveDownloadCompletedEventArgs>(OnDownloadCompleted);
                //   client.DownloadAsync("file.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!131/picture?type=thumbnail");
                client.DownloadAsync("file.8c8ce076ca27823f.8C8CE076CA27823F!137/content");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#downloading_files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDownloadCompleted(object sender, LiveDownloadCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //imageFrame.Visibility = Visibility.Visible;
                //BitmapImage imgSource = new BitmapImage();
                //imgSource.SetSource(e.Result);
                //// imageFrame is a user-defined Image control.
                //imageFrame.Source = imgSource;
                //e.Result.Close();
                StreamReader sr = new StreamReader(e.Result);
                Debug.WriteLine(sr.ReadToEnd());
            }
            else
            {
                Debug.WriteLine("Error downloading image: " + e.Error.ToString());
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#uploading_files
        /// </summary>
        public void UploadFile()
        {
            throw new NotImplementedException();
        }

        public void UpdateUploadedFile()
        {
            throw new NotImplementedException();
        }





        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#file_links
        /// </summary>
        public void GetSharedLink()
        {

            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.GetCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(GetSharedLink_Completed);
                client.GetAsync("file.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!119/shared_read_link");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#file_links
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void GetSharedLink_Completed(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("Shared link = " + e.Result["link"].ToString());
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#move
        /// </summary>
        public void MoveFolderOrFile()
        {
            if (session == null)
            {
                Debug.WriteLine("You must sign in first.");
            }
            else
            {
                LiveConnectClient client = new LiveConnectClient(session);
                client.MoveCompleted +=
                    new EventHandler<LiveOperationCompletedEventArgs>(MoveFile_MoveCompleted);
                client.MoveAsync("file.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!123",
                    "folder.a6b2a7e8f2515e5e.A6B2A7E8F2515E5E!125");
            }
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#move
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MoveFile_MoveCompleted(object sender, LiveOperationCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Debug.WriteLine("File or folder move completed.");
            }
            else
            {
                Debug.WriteLine("Error calling API: " + e.Error.ToString());
            }
        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/live/hh561740.aspx#copy
        /// </summary>
        public void CopyFile()
        {
            throw new NotImplementedException();
        }
    }
}
