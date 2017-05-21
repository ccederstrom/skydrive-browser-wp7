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
using System.Collections.Generic;
using System.IO;
using SkyDriveBrowser.DAO;

namespace SkyDriveBrowser.Service.Impl
{
    /// <summary>
    /// http://msdn.microsoft.com/en-us/live/hh561740.aspx
    /// </summary>
    public class FileService : IFileService
    {
        private ISkyDriveFileDao iSkyDriveFileDao;
        public ISkyDriveFileDao ISkyDriveFileDao
        {
            get { return iSkyDriveFileDao;}
            set { iSkyDriveFileDao = value; }
        }



        public void TraverseDirectory()
        {
            iSkyDriveFileDao.TraverseDirectory();
        }

        public void GetFolderProperties()
        {
            iSkyDriveFileDao.GetFolderProperties();
        }

        public void DeleteFileOrFolder()
        {
            iSkyDriveFileDao.DeleteFileOrFolder();
        }

        public void CreateFolder()
        {
            iSkyDriveFileDao.CreateFolder();
        }

        public void RenameFolder()
        {
            iSkyDriveFileDao.RenameFile();
        }

        public void GetFileProperties()
        {
            iSkyDriveFileDao.GetFileProperties();
        }

        public void RenameFile()
        {
            iSkyDriveFileDao.RenameFile();
        }

        public void DownloadFile()
        {
            iSkyDriveFileDao.DownloadFile();
        }

        public void UploadFile()
        {
            iSkyDriveFileDao.UploadFile();
        }

        public void UpdateUploadedFile()
        {
            iSkyDriveFileDao.UpdateUploadedFile();
        }

        public void GetSharedLink()
        {
            iSkyDriveFileDao.GetSharedLink();
        }

        public void MoveFolderOrFile()
        {
            iSkyDriveFileDao.MoveFolderOrFile();
        }

        public void CopyFile()
        {
            iSkyDriveFileDao.CopyFile();
        }
    }
}
