﻿using Livet;

using SRNicoNico.Models.NicoNicoWrapper;


namespace SRNicoNico.ViewModels {

	public class SearchResultEntryViewModel : ViewModel {
	
		//検索結果
		public NicoNicoSearchResultNode Node { get; set; }

		//動画を開く
		public void OpenVideo() {

			new VideoViewModel("http://www.nicovideo.jp/watch/" + Node.cmsid);
		}
	}
}