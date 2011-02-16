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
        private IVsOutputWindowPane _debugPane;

        public KillCassiniPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }

        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
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
                DebugPane.OutputString("KillCassini: Mission Acquired. Hunting... \r\n");
                processesKilled.ForEach(p => DebugPane
                            .OutputString(String.Format("KillCassini: Killed {0}: Id= {1}, Handle= {2}.\r\n", 
                            p.Name, p.Id, p.Handle)));
                DebugPane.OutputString("KillCassini: Over and Out. \r\n");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
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
    }
}
