using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

//codezine.jp �l���
namespace SRNicoNico.Models.NicoNicoViewer {
    public delegate int MapUrlToZoneEventHandler(String pwszUrl, out int pdwZone, int dwFlags);

    public delegate int ProcessUrlActionEventHandler(String pwszUrl, int dwAction, out byte pPolicy, int cbPolicy, byte pContext, int cbContext, int dwFlags, int dwReserved);

    /// <summary>
    /// Form1 �̊T�v�̐����ł��B
    /// </summary>
    public class InternetSecurityManagerHelper : WebBrowserAPI.IServiceProvider, WebBrowserAPI.IInternetSecurityManager {
        public event MapUrlToZoneEventHandler MapUrlToZone;
        public event ProcessUrlActionEventHandler ProcessUrlAction;

        public InternetSecurityManagerHelper() {
        }

        public void Attach(object activeXInstance) {
            // Microsoft Web Browser �R���g���[���� ActiveX �R���g���[���{�̂��擾
            //object ocx = axWebBrowser.GetOcx();

            // Microsoft Web Browser �R���g���[������ IServiceProvider ���擾
            WebBrowserAPI.IServiceProvider ocxServiceProvider = activeXInstance as WebBrowserAPI.IServiceProvider;

            // IServiceProvider.QueryService() ���g���� IProfferService ���擾
            IntPtr profferServicePtr;
            ocxServiceProvider.QueryService(ref WebBrowserAPI.SID_SProfferService, ref WebBrowserAPI.IID_IProfferService, out profferServicePtr);
            WebBrowserAPI.IProfferService profferService = Marshal.GetObjectForIUnknown(profferServicePtr) as WebBrowserAPI.IProfferService;

            // IProfferService.ProfferService() ���g���Ď����� IInternetSecurityManager �Ƃ��Ē�
            int cookie = 0;
            profferService.ProfferService(ref WebBrowserAPI.IID_IInternetSecurityManager, this, ref cookie);

        }

        #region IServiceProvider �����o

        int WebBrowserAPI.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject) {
            ppvObject = IntPtr.Zero;
            if(guidService == WebBrowserAPI.IID_IInternetSecurityManager) {
                // �������� IID_IInternetSecurityManager �� QueryInterface ���ĕԂ�
                IntPtr punk = Marshal.GetIUnknownForObject(this);
                return Marshal.QueryInterface(punk, ref riid, out ppvObject);
            }
            return HRESULT.E_NOINTERFACE;
        }

        #endregion

        #region IInternetSecurityManager �����o

        int WebBrowserAPI.IInternetSecurityManager.SetSecuritySite(WebBrowserAPI.IInternetSecurityMgrSite pSite) {
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.GetSecuritySite(WebBrowserAPI.IInternetSecurityMgrSite pSite) {
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.MapUrlToZone(String pwszUrl, out int pdwZone, int dwFlags) {
            pdwZone = 0;
            if(this.MapUrlToZone != null) {
                return this.MapUrlToZone(pwszUrl, out pdwZone, dwFlags);
            }
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.GetSecurityId(string pwszUrl, byte[] pbSecurityId, ref uint pcbSecurityId, uint dwReserved) {
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.ProcessUrlAction(String pwszUrl, int dwAction, out byte pPolicy, int cbPolicy, byte pContext, int cbContext, int dwFlags, int dwReserved) {
            pPolicy = 0;
            if(this.ProcessUrlAction != null) {
                return this.ProcessUrlAction(pwszUrl, dwAction, out pPolicy, cbPolicy, pContext, cbContext, dwFlags, dwReserved);
            }
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.QueryCustomPolicy(String pwszUrl, ref Guid guidKey, byte ppPolicy, int pcbPolicy, byte pContext, int cbContext, int dwReserved) {
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.SetZoneMapping(int dwZone, String lpszPattern, int dwFlags) {
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        int WebBrowserAPI.IInternetSecurityManager.GetZoneMappings(int dwZone, out IEnumString ppenumString, int dwFlags) {
            ppenumString = null;
            return WebBrowserAPI.INET_E_DEFAULT_ACTION;
        }

        #endregion
    }
}
