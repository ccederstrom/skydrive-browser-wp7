using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkyDriveBrowser.DAO
{
    public interface ISkyDriveFileDao
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
