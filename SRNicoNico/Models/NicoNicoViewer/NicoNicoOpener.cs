﻿using SRNicoNico.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SRNicoNico.Models.NicoNicoViewer {
    public class NicoNicoOpener : Exception {


        
        private readonly Regex UrlRegex = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?");

        //引数の文字列が正しいURLだったらOpenする
        public static TabItemViewModel TryOpen(string maybeUrl) {

            //URLだったら
            var url = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?");

            if(url.IsMatch(maybeUrl)) {

                return Open(maybeUrl);
            }

            return null;
        }





        //シンタックスシュガー的な何か シンタックスじゃないけど
        public static TabItemViewModel Open(Uri uri) {

            return Open(uri.OriginalString);
        }

        //URLから適当なViewを開く
        public static TabItemViewModel Open(string url) {


            if(url.StartsWith("http://www.nicovideo.jp/watch/")) {

                var vm = new VideoViewModel(url);
                App.ViewModelRoot.MainContent.AddVideoView(vm);
                return vm;
            }
            if(url.StartsWith("http://www.nicovideo.jp/user/")) {

                var vm = new UserViewModel(url);
                App.ViewModelRoot.MainContent.AddUserTab(vm);
                return vm;
            }
            if(url.StartsWith("http://www.nicovideo.jp/mylist/")) {

                var vm = new PublicMylistViewModel(url);
                App.ViewModelRoot.MainContent.AddUserTab(vm);
                return vm;
            }
            if(url.StartsWith("http://com.nicovideo.jp/community/")) {

                var vm = new CommunityViewModel(url);
                App.ViewModelRoot.MainContent.AddUserTab(vm);
                return vm;
            } else {

                App.ViewModelRoot.AddWebViewTab(url, true);
                return null;
            }
        }


        //ニコニコのURLが何を指しているかを返す
        public static NicoNicoUrlType GetType(Uri url) {

            return GetType(url.OriginalString);
        }
        public static NicoNicoUrlType GetType(string url) {

            if(url.StartsWith("http://www.nicovideo.jp/watch/")) {

                return NicoNicoUrlType.Video;
            }
            if(url.StartsWith("http://www.nicovideo.jp/user/")) {

                return NicoNicoUrlType.User;
            }
            if(url.StartsWith("http://www.nicovideo.jp/mylist/")) {

                return NicoNicoUrlType.Mylist;
            }
            if(url.StartsWith("http://com.nicovideo.jp/community/")) {

                return NicoNicoUrlType.Community;
            }
            /*if(url.StartsWith("http://live.nicovideo.jp/watch/")) {

                return NicoNicoUrlType.Live;
            }*/

            return NicoNicoUrlType.Other;
        }
    }

    public enum NicoNicoUrlType {

        Video,
        Live,
        Mylist,
        User,
        Community,
        Other

    }

}
