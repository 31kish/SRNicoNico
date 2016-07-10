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
using SRNicoNico.Models.NicoNicoViewer;

namespace SRNicoNico.ViewModels {
    public class RankingEntryViewModel : TabItemViewModel {

        private readonly NicoNicoRanking RankingInstance;


        private readonly string Category;

        #region RankingItemList変更通知プロパティ
        private DispatcherCollection<RankingItem> _RankingItemList = new DispatcherCollection<RankingItem>(DispatcherHelper.UIDispatcher);

        public DispatcherCollection<RankingItem> RankingItemList {
            get { return _RankingItemList; }
            set { 
                if(_RankingItemList == value)
                    return;
                _RankingItemList = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Page変更通知プロパティ
        private string _Page;

        public string Page {
            get { return _Page; }
            set { 
                if(_Page == value)
                    return;
                _Page = value;
                RaisePropertyChanged();
                Refresh();
            }
        }
        #endregion


        public RankingEntryViewModel(string title, string category, NicoNicoRanking instance) : base(title) {

            Category = category;
            RankingInstance = instance;
        }

        public async void Initialize(int page) {

            IsActive = true;

            var a = await RankingInstance.GetRankingAsync(Category, page);

            RankingItemList.Clear();

            foreach(var item in a.ItemList) {

                RankingItemList.Add(item);
            }

            IsActive = false;


        }

        public void Open(RankingItem item) {

            NicoNicoOpener.Open(item.VideoUrl);
        }

        public void Refresh() {

            Initialize(TransPage());
        }

        private int TransPage() {

            switch(Page) {
                case "1-100":
                    return 1;
                case "101-200":
                    return 2;
                case "201-300":
                    return 3;
                case "301-400":
                    return 4;
                case "401-500":
                    return 5;
                case "501-600":
                    return 6;
                case "601-700":
                    return 7;
                case "701-800":
                    return 8;
                case "801-900":
                    return 9;
                case "901-1000":
                    return 10;
                default:
                    return 1;
            }
        }

    }
}
