﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

using Codeplex.Data;

namespace SRNicoNico.Models.NicoNicoWrapper {


	public class NicoNicoSearchResultNode {

		//ID sm9みたいな
		public string cmsid { get; internal set; }

		//タイトル
		public string title { get; internal set; }

		//再生回数
		public ulong view_counter { get; internal set; }

		//コメント数
		public ulong comment_counter { get; internal set; }

		//マイリスト数
		public ulong mylist_counter { get; internal set; }

		//サムネイルURL
		public string thumbnail_url { get; internal set; }

		//コンストラクタ
		internal NicoNicoSearchResultNode(string cmsid, string title, ulong view_counter, ulong comment_counter, ulong mylist_counter, string thumbnail_url) {

			this.cmsid = cmsid;
			this.title = title;
			this.view_counter = view_counter;
			this.comment_counter = comment_counter;
			this.mylist_counter = mylist_counter;
			this.thumbnail_url = thumbnail_url;

		}

	}

	public class NicoNicoSearchResult : NotificationObject {
		/*
		 * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
		 */


		//検索結果の総数
		public ulong total { get; internal set; }
		public List<NicoNicoSearchResultNode> list;

		//コンストラクタ使用不可
		private NicoNicoSearchResult() {

			this.list = new List<NicoNicoSearchResultNode>();

		}

		//NicoNicoSearchで取得したjsonをデシリアライズする
		public static NicoNicoSearchResult deserialize(string jsonString) {



			NicoNicoSearchResult result = new NicoNicoSearchResult();

			//取得したJsonの全体
			var json = DynamicJson.Parse(jsonString);





			//検索結果総数
			result.total = (ulong) json.count;

			//Jsonからリストを取得、データを格納
			foreach(var entry in json.list) {

				NicoNicoSearchResultNode node = new NicoNicoSearchResultNode(entry.id, entry.title, (ulong)entry.view_counter, (ulong)entry.num_res, (ulong)entry.mylist_counter, entry.thumbnail_url);

				result.list.Add(node);
			}




			return result;
		}

	}
}
