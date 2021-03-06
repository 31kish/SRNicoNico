﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;
using System.Windows.Input;

namespace SRNicoNico.ViewModels {
    public class VideoViewViewModel : TabItemViewModel {




        #region VideoList変更通知プロパティ
        private DispatcherCollection<VideoViewModel> _VideoList = new DispatcherCollection<VideoViewModel>(DispatcherHelper.UIDispatcher);

        public DispatcherCollection<VideoViewModel> VideoList {
            get { return _VideoList; }
            set { 
                if(_VideoList == value)
                    return;
                _VideoList = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedList変更通知プロパティ
        private VideoViewModel _SelectedList;

        public VideoViewModel SelectedList {
            get { return _SelectedList; }
            set { 
                if(_SelectedList == value)
                    return;
                _SelectedList = value;
                if(value != null) {

                    Status = value.Status;
                }

                RaisePropertyChanged();
            }
        }
        #endregion



        public VideoViewViewModel() : base("動画") {



        }

        public void Add(VideoViewModel vm) {

            VideoList.Add(vm);
            SelectedList = vm;
        }

        //VideoViewから
        public void Remove(VideoViewModel vm) {

            if(VideoList.Contains(vm)) {

                VideoList.Remove(vm);
            }

        }

        public override void KeyDown(KeyEventArgs e) {
            base.KeyDown(e);

            SelectedList?.KeyDown(e);
        }

        public override bool CanShowHelp() {
            return true;
        }
        public override void ShowHelpView(InteractionMessenger Messenger) {

            Messenger.Raise(new TransitionMessage(typeof(Views.VideoHelpView), this, TransitionMode.NewOrActive));
        }
    }
}
