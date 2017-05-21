using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Live;

namespace SkyDriveBrowser.Service
{
    interface IFileService
    {
        void TraverseDirectory();

        void GetFolderProperties();

        void DeleteFileOrFolder();
        void CreateFolder();

        void RenameFolder();

        void GetFileProperties();
        void RenameFile();
        void DownloadFile();
        void UploadFile();
        void UpdateUploadedFile();

        void GetSharedLink();
        void MoveFolderOrFile();
        void CopyFile();
    }
}
