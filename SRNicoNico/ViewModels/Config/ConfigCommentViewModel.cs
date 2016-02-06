﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using SRNicoNico.Models;
using System.Windows;

namespace SRNicoNico.ViewModels {
    public class ConfigCommentViewModel : TabItemViewModel {


        #region Hide3DSComment変更通知プロパティ
        public bool Hide3DSComment {
            get { return Properties.Settings.Default.Hide3DSComment; }
            set { 
                if(Properties.Settings.Default.Hide3DSComment == value)
                    return;
                Properties.Settings.Default.Hide3DSComment = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        #endregion


        #region HideWiiUComment変更通知プロパティ
        public bool HideWiiUComment {
            get { return Properties.Settings.Default.HideWiiUComment; }
            set { 
                if(Properties.Settings.Default.HideWiiUComment == value)
                    return;
                Properties.Settings.Default.HideWiiUComment = value;
                Properties.Settings.Default.Save();
                RaisePropertyChanged();
            }
        }
        #endregion


        #region NGSharedLevel変更通知プロパティ
        private string _NGSharedLevel;

        public string NGSharedLevel {
            get { return _NGSharedLevel; }
            set { 
                if(_NGSharedLevel == value)
                    return;
                _NGSharedLevel = value;
                RaisePropertyChanged();
            }
        }
        #endregion




        public ConfigCommentViewModel() : base("コメント") {

        }
    }
}
