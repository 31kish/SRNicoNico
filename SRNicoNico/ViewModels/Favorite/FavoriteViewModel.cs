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

using SRNicoNico.Models.NicoNicoWrapper;
using System.Collections.ObjectModel;

namespace SRNicoNico.ViewModels {
    public class FavoriteViewModel : TabItemViewModel {


        #region FavoriteList変更通知プロパティ
        private DispatcherCollection<TabItemViewModel> _FavoriteList;

        public DispatcherCollection<TabItemViewModel> FavoriteList {
            get { return _FavoriteList; }
            set { 
                if(_FavoriteList == value)
                    return;
                _FavoriteList = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedList変更通知プロパティ
        private TabItemViewModel _SelectedList;

        public TabItemViewModel SelectedList {
            get { return _SelectedList; }
            set { 
                if(_SelectedList == value)
                    return;
                _SelectedList = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        private NicoNicoFavorite FavoriteInstance;

        public FavoriteViewModel() : base("お気に入り") {

            FavoriteInstance = new NicoNicoFavorite();
            FavoriteList = new DispatcherCollection<TabItemViewModel>(DispatcherHelper.UIDispatcher);

            FavoriteList.Add(new FavoriteUserViewModel(this, FavoriteInstance));


        }

        public void Reflesh() {


        }


    }
}
