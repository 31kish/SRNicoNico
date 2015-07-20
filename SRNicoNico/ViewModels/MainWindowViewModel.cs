﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using SRNicoNico.Views.Contents;
using SRNicoNico.Views.Contents.Search;
using SRNicoNico.Models;
using SRNicoNico.Models.NicoNicoWrapper;

namespace SRNicoNico.ViewModels {
	public class MainWindowViewModel : ViewModel {


		#region Title変更通知プロパティ
		private string _Title = "NicoNicoViewer ";

		public string Title {
			get { return _Title; }
			set { 
				if(_Title == value)
					return;
				_Title = value;
				RaisePropertyChanged();
			}
		}
		#endregion



		#region Content変更通知プロパティ
		private ViewModel _Content;

		public ViewModel Content {
			get { return _Content; }
			set {
				if(_Content == value)
					return;
				_Content = value;
				RaisePropertyChanged();
			}
		}
		#endregion



		public SignInDialogViewModel SignIn { get; private set; }
		public SearchViewModel Search { get; private set; }
		public SearchResultViewModel SearchResult { get; private set; }
		public VideoViewModel Video { get; private set; }

		public MainWindowViewModel() {

			this.SignIn = new SignInDialogViewModel();

			this.Search = new SearchViewModel();

			this.SearchResult = new SearchResultViewModel();

			this.Video = new VideoViewModel();

			this.Content = this;
		}


		public void Initialize() {


			//Modelsを初期化
			Task.Run(() => {

				NicoNicoWrapperMain.Instance = new NicoNicoWrapperMain();


				if(File.Exists(NicoNicoUtil.CurrentDirectory.DirectoryName + @"\session")) {

					//セッション情報を取得する
					StreamReader reader = new StreamReader(NicoNicoUtil.CurrentDirectory.DirectoryName + @"\session");
							
					string key = reader.ReadLine().Split(':')[1];
					DateTimeOffset expire = DateTimeOffset.Parse(reader.ReadLine().Replace("Expire:", ""));

					reader.Close();

					//セッションが有効か比較する
					if(DateTimeOffset.Compare(expire, DateTimeOffset.Now) < 0) {

						//セッションが有効期限切れ
						this.SignIn.StateText = "有効期限が切れています。\n再度ログインしてください。";
						this.SignIn.AutoLogin = true;

						//ログインダイアログ表示
						Messenger.Raise(new TransitionMessage(typeof(Views.SignInDialog), this.SignIn, TransitionMode.Modal));
						return;
					}


					//セッションが有効だった場合
					NicoNicoWrapperMain.Instance.Session = new NicoNicoSession(key, expire);
					if(NicoNicoWrapperMain.Instance.Session.SignInInternal() != SigninStatus.Success) {

						//ログイン失敗
						this.SignIn.StateText = "ログインに失敗しました。";
						this.SignIn.AutoLogin = true;

						//ログインダイアログ表示
						Messenger.Raise(new TransitionMessage(typeof(Views.SignInDialog), this.SignIn, TransitionMode.Modal));
						return;
					}

					//ログイン成功
					NicoNicoWrapperMain.Instance.init();



				//手動ログイン
				} else {

					//セッションを確立
					NicoNicoWrapperMain.Instance.Session = new NicoNicoSession();
					Messenger.Raise(new TransitionMessage(typeof(Views.SignInDialog), this.SignIn, TransitionMode.Modal));
					return;
				}

			});
		}

		//終了処理
		protected override void Dispose(bool disposing) {

			if(disposing) {

				NicoNicoWrapperMain.getSession().HttpHandler.Dispose();
				NicoNicoWrapperMain.getSession().HttpClient.Dispose();

				Video.DisposePlayer();
				;
			}



			base.Dispose(disposing);
		}

	}
}
