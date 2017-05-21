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
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.ComponentModel;

namespace SkyDriveBrowser.SkyDriveModels
{

    /// <summary>
    /// Represents a file or foldder in SkyDrive.
    /// </summary>
    [DataContract]
    public class FileInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        private string _id;
        /// <summary>
        /// Gets or sets the id of the album
        /// </summary>
        [DataMember(Name = "id")]
        public string Id
        {
            get { return _id; }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private From _from;
        /// <summary>
        /// Gets or sets the user who created the fo;e
        /// </summary>
        [DataMember(Name = "from")]
        public From From
        {
            get { return _from; }
            set
            {
                if (value != _from)
                {
                    _from = value;
                    NotifyPropertyChanged("From");
                }
            }
        }


        private string _name;
        /// <summary>
        /// Gets or sets the name of the file or folder
        /// </summary>
        [DataMember(Name = "name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _description;
        /// <summary>
        /// Gets or sets the description of the file
        /// </summary>
        [DataMember(Name = "description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        private string _parent_id;
        /// <summary>
        /// The parent of the file or folder in SkyDrive
        /// </summary>
        [DataMember(Name = "parent_id")]
        public string ParentId
        {
            get { return _parent_id; }
            set
            {
                if (value != _parent_id)
                {
                    _parent_id = value;
                    NotifyPropertyChanged("ParentId");
                }
            }
        }

        private string _upload_location;
        /// <summary>
        /// Location of file or folder in SkyDrive
        /// </summary>
        [DataMember(Name = "upload_location")]
        public string UploadLocation
        {
            get { return _upload_location; }
            set
            {
                if (value != _upload_location)
                {
                    _upload_location = value;
                    NotifyPropertyChanged("UploadLocation");
                }
            }
        }

        private string _is_embeddable;
        /// <summary>
        /// Is the file or folder embeddable
        /// </summary>
        [DataMember(Name = "is_embeddable")]
        public string IsEmbeddable
        {
            get { return _is_embeddable; }
            set
            {
                if (value != _is_embeddable)
                {
                    _is_embeddable = value;
                    NotifyPropertyChanged("IsEmbeddable");
                }
            }
        }

        private int _count;
        /// <summary>
        /// Gets or sets the number of files in the folder
        /// </summary>
        [DataMember(Name = "count")]
        public int Count
        {
            get { return _count; }
            set
            {
                if (value != _count)
                {
                    _count = value;
                    NotifyPropertyChanged("Count");
                }
            }
        }

        private string _link;
        /// <summary>
        /// Gets or sets the link to the file
        /// </summary>
        [DataMember(Name = "link")]
        public string Link
        {
            get { return _link; }
            set
            {
                if (value != _link)
                {
                    _link = value;
                    NotifyPropertyChanged("Link");
                }
            }
        }

        private string _type;
        /// <summary>
        /// Gets or sets the type of folder or file in SkyDrive
        /// </summary>
        [DataMember(Name = "type")]
        public string Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }

        private SharedWith _shared_with;
        /// <summary>
        /// Who has access to the file or folder
        /// </summary>
        [DataMember(Name = "shared_with")]
        public SharedWith SharedWith
        {
            get { return _shared_with; }
            set
            {
                if (value != _shared_with)
                {
                    _shared_with = value;
                    NotifyPropertyChanged("SharedWith");
                }
            }
        }


        private string _created_time;
        /// <summary>
        /// Gets or sets the created time of the file
        /// </summary>
        [DataMember(Name = "created_time")]
        public string CreatedTime
        {
            get { return _created_time; }
            set
            {
                if (value != _created_time)
                {
                    _created_time = value;
                    NotifyPropertyChanged("CreatedTime");
                }
            }
        }

        private string _updated_time;
        /// <summary>
        /// Gets or sets the updated time of the file or folder
        /// </summary>
        [DataMember(Name = "updated_time")]
        public string UpdatedTime
        {
            get { return _updated_time; }
            set
            {
                if (value != _updated_time)
                {
                    _updated_time = value;
                    NotifyPropertyChanged("UpdatedTime");
                }
            }
        }

    }
}
