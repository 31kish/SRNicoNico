﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;

using Codeplex.Data;

using SRNicoNico.Models.NicoNicoWrapper;
using System.Xml;

namespace SRNicoNico.Models.NicoNicoViewer {
    public class UpdateCheck : NotificationObject {

#if DEBUG
        private static string CheckUrl = "https://mrtska.net/niconicowrapper/update";
#elif RELEASE
        private static string CheckUrl = "http://download.mrtska.net/DownloadCounter/Download?file=NicoNicoViewer/update";
#endif

        public static bool IsUpdateAvailable(double cur, ref string url) {

            try {

                var a = NicoNicoWrapperMain.Session.GetAsync(CheckUrl).Result;

                if(a.Length == 0) {

                    return false;
                }
                var json = DynamicJson.Parse(a);

                if(cur < json.version) {

                    url = json.url;
                    return true;
                }


                return false;
            } catch(Exception ex) when (ex is XmlException || ex is RequestTimeout) {

                return false;
            }
        }
    }
}
