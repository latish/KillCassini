using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

namespace LatishSehgal.KillCassini
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [Guid(GuidList.guidKillCassiniPkgString)]
    public sealed class KillCassiniPackage : Package
    {
        protected override void Initialize()
        {
            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                var menuCommandID = new CommandID(GuidList.guidKillCassiniCmdSet, (int)PkgCmdIDList.cmdidKillCassini);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                var processesKilled = CassiniUtil.KillAllCassiniInstances();

                DebugPane.Activate();
                DebugPane.OutputString(string.Format("{0}: Mission Acquired. Hunting... \r\n",PackageName));
                processesKilled.ForEach(p => DebugPane
                            .OutputString(String.Format("{0}: Killed {1}: Id= {2}, Handle= {3}.\r\n",
                            PackageName,p.Name, p.Id, p.Handle)));
                TaskBarUtil.RefreshNotificationArea();
                DebugPane.OutputString(string.Format("{0}: Over and Out. \r\n",PackageName));
            }
            catch (Exception exception)
            {
                Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Exception: {0}", exception.Message));
            }
        }

        private IVsOutputWindowPane DebugPane
        {
            get
            {
                if (_debugPane == null)
                {
                    var outputWindow = GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
                    var debugPaneGuid = VSConstants.GUID_OutWindowDebugPane;
                    if (outputWindow != null)
                        outputWindow.GetPane(ref debugPaneGuid, out _debugPane);
                }
                return _debugPane;
            }
        }

        private IVsOutputWindowPane _debugPane;
        private const string PackageName = "KillCassini";
    }
}
