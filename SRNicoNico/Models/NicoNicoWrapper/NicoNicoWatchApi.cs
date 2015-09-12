﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Http;

using Livet;


using HtmlAgilityPack;
using Fizzler.Systems.HtmlAgilityPack;

using Codeplex.Data;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using SRNicoNico.ViewModels;

namespace SRNicoNico.Models.NicoNicoWrapper {


    //動画を見るうえで必要な情報をこのAPIだけで全て取得できるヤバいAPI
    public sealed class NicoNicoWatchApi : NotificationObject {



        //動画ページを指定
        public static WatchApiData GetWatchApiData(string videoPage) {

            //動画ページのhtml取得
            HttpResponseMessage response = NicoNicoWrapperMain.GetSession().HttpClient.GetAsync(videoPage).Result;

            //チャンネル、公式動画
            if(response.StatusCode == HttpStatusCode.MovedPermanently) {

                
                response = NicoNicoWrapperMain.GetSession().HttpClient.GetAsync(response.Headers.Location).Result;
            }

            //削除された動画
            if(response.StatusCode == HttpStatusCode.NotFound) {

                
                return null;
            }

            //混雑中
            if(response.StatusCode == HttpStatusCode.ServiceUnavailable) {

                return null;
            }



            var cookie = response.Headers.GetValues("Set-Cookie");
            foreach(string s in cookie) {

                foreach(string ss in s.Split(';')) {

                    App.SetCookie(new Uri("http://nicovideo.jp/"), ss);
                }

            }



            string html = response.Content.ReadAsStringAsync().Result;


            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml2(html);

            //htmlからAPIデータだけを綺麗に抜き出す すごい
            string data = doc.DocumentNode.QuerySelector("#watchAPIDataContainer").InnerHtml;

            //html特殊文字をデコードする
            data = System.Web.HttpUtility.HtmlDecode(data);

            //jsonとしてAPIデータを展開していく
            var json = DynamicJson.Parse(data);

            //GetFlvの結果
            string flv = json.flashvars.flvInfo;

            //2重にエンコードされてるので二回
            flv = System.Web.HttpUtility.UrlDecode(flv);
            flv = System.Web.HttpUtility.UrlDecode(flv);


            WatchApiData ret = new WatchApiData();

            //&で繋がれているので剥がす
            Dictionary<string, string> getFlv = flv.Split(new char[] { '&' }).ToDictionary(source => source.Substring(0, source.IndexOf('=')),
            source => Uri.UnescapeDataString(source.Substring(source.IndexOf('=') + 1)));

            ret.GetFlv = new NicoNicoGetFlvData(getFlv);
            

            //動画情報
            var videoDetail = json.videoDetail;

            //---情報を詰める---
            ret.Cmsid = videoDetail.id;
            ret.Title = videoDetail.title;
            ret.Thumbnail = videoDetail.thumbnail;
            ret.Description = videoDetail.description_original;
            ret.PostedAt = videoDetail.postedAt;
            ret.Length = (int) videoDetail.length;
            ret.ViewCounter = (int) videoDetail.viewCount;
            ret.CommentCounter = (int) videoDetail.commentCount;
            ret.MylistCounter = (int) videoDetail.mylistCount;
            ret.YesterdayRank = videoDetail.yesterday_rank;
            ret.HighestRank = videoDetail.highest_rank;

            Console.WriteLine(ret.Description);

            //動画
            Regex regex = new Regex(@"(.?)((sm|watch/|nm)\d+)");//
            ret.Description = regex.Replace(ret.Description, new MatchEvaluator((match) => {


                if(match.Groups[1].Value.Length != 0 && (match.Groups[1].Value == "/" || match.Groups[1].Value == ">" || match.Groups[1].Value == "\"")) {

                    return match.Value;
                }


                return match.Groups[1].Value + "<a href=\"" + match.Groups[2].Value + "\">" + match.Groups[2].Value + "</a>";
            }));

            //生放送
            Regex live = new Regex(@"(.?)(lv\d+)");
            ret.Description = live.Replace(ret.Description, new MatchEvaluator((match) => {

                if(match.Groups[1].Value.Length != 0 && (match.Groups[1].Value == "/" || match.Groups[1].Value == ">" || match.Groups[1].Value == "\"")) {

                    return match.Value;
                }

                return "<a href=\"http://live.nicovideo.jp/watch/" + match.Value + "\">" + match.Value + "</a>";
            }));

            //普通のURLだったら
            Regex url = new Regex(@"(.?)(https?://[\w/:%#\$&\?\(\)~\.=\+\-]+)");
            ret.Description = url.Replace(ret.Description, new MatchEvaluator(Match));
            
            //TwitterのIDだった場合
            Regex twitter = new Regex(@"@[A-z|0-9|_]+");
            ret.Description = twitter.Replace(ret.Description, new MatchEvaluator((match) => {

                return "<a href=\"https://twitter.com/" + match.Value.Substring(1) + "\">" + match.Value + "</a>";
            }));

            ret.TagList = new ObservableCollection<VideoTagViewModel>();

            foreach(var tag in videoDetail.tagList) {

                NicoNicoTag entry = new NicoNicoTag() {

                    Id = tag.id,
                    Tag = tag.tag,
                    Dic = tag.dic(),
                    Lck = tag.lck == "1" ? true : false,
                    Cat = tag.cat()
                };
                ret.TagList.Add(new VideoTagViewModel(entry)); 
            }
            //------
            Console.WriteLine("# " + ret.Description);

            return ret;
        }


        private static string Match(Match match) {

            //aタグが書いてあったらそのまま返す
            if(match.Groups[1].Value.Length != 0 && (match.Groups[1].Value == "/" || match.Groups[1].Value == ">" || match.Groups[1].Value == "\"")) {

                return match.Value;
            }

            return "<a href=\"" + match.Groups[2].Value.Trim() + "\">" + match.Groups[2].Value.Trim() + "</a>";

        }




    }


    public class WatchApiData : NotificationObject {

        //GetFlvAPIの結果
        public NicoNicoGetFlvData GetFlv { get; set; }

        public string Cmsid { get; set; }

        //動画タイトル
        public string Title { get; set; }

        //動画サムネイルURL
        public string Thumbnail { get; set; }

        //動画説明文
        public string Description { get; set; }

        //動画投稿日時
        public string PostedAt { get; set; }

        //動画時間
        public int Length { get; set; }

        //再生数
        public int ViewCounter { get; set; }

        //コメント数
        public int CommentCounter { get; set; }

        //マイリスト数
        public int MylistCounter { get; set; }

        //昨日のランキング
        public string YesterdayRank { get; set; }

        //最高ランキング
        public string HighestRank { get; set; }

        //タグリスト
        #region TagList変更通知プロパティ
        private ObservableCollection<VideoTagViewModel> _TagList;

        public ObservableCollection<VideoTagViewModel> TagList {
            get { return _TagList; }
            set { 
                if(_TagList == value)
                    return;
                _TagList = value;
                RaisePropertyChanged();
            }
        }
        #endregion



    }

    //タグ情報
    public class NicoNicoTag : NotificationObject {

        //タグID
        public string Id { get; set; }

        //タグ名
        public string Tag { get; set; }

        //大百科が存在するか
        public bool Dic { get; set; }

        //ロックされているか
        public bool Lck { get; set; }

        //カテゴリタグか否か
        public bool Cat { get; set; }
    }


    public class NicoNicoGetFlvData : NotificationObject {

        //getflvAPIで取得できるデータ

        //スレッドID
        public string ThreadID { get; internal set; }

        //長さ
        public uint Length { get; internal set; }

        //動画URL
        public string VideoUrl { get; internal set; }

        //コメントサーバーURL
        public Uri CommentServerUrl { get; internal set; }

        //サブコメントサーバーURL
        public Uri SubCommentServerUrl { get; internal set; }

		//ユーザーID
		public string UserId { get; internal set; }

		//プレミアムか否か
		public bool IsPremium { get; internal set; }

        //非公開理由
        public int ClosedReason { get; internal set; }

        public NicoNicoGetFlvData(Dictionary<string, string> wwwData) {

            ThreadID = wwwData["thread_id"];
            Length = uint.Parse(wwwData["l"]);
            VideoUrl = wwwData["url"];
            CommentServerUrl = new Uri(wwwData["ms"]);
            SubCommentServerUrl = new Uri(wwwData["ms_sub"]);
			UserId = wwwData["user_id"];
			IsPremium = wwwData["is_premium"] == "1" ? true : false;
        }

    }
}
