﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Livet;

namespace SRNicoNico.Models.NicoNicoWrapper {
	public class NicoNicoGetFlv : NotificationObject {
		/*
		 * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
		 */

		//getFlvAPIのURL
		public const string GetFlvURL = "http://flapi.nicovideo.jp/api/getflv/";

		//getFlvAPIからデータを取得
		public static NicoNicoGetFlvData GetFlv(string cmsid) {


			return NicoNicoWrapperMain.getSession().HttpClient.GetStringAsync(GetFlvURL + cmsid).ContinueWith<NicoNicoGetFlvData>(task => {

				string response = task.Result;
				Console.WriteLine(System.Web.HttpUtility.UrlDecode(response));

				Dictionary<string, string> data = response.Split(new char[] { '&' }).ToDictionary(source => source.Substring(0, source.IndexOf('=')),
				source => Uri.UnescapeDataString(source.Substring(source.IndexOf('=') + 1)));



				return new NicoNicoGetFlvData(data);
			}).Result;
		}


		public const string WatchURL = "http://www.nicovideo.jp/watch/";

		public static Stream GetFlvStream(string cmsid, Uri videoUrl) {

			var a = NicoNicoWrapperMain.getSession().HttpClient.GetAsync(WatchURL + cmsid).Result;
			Console.WriteLine(a);
			return NicoNicoWrapperMain.getSession().HttpClient.GetStreamAsync(videoUrl).Result;
		}
	}
}
