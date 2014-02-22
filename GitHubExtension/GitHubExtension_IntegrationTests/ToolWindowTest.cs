﻿using System;
using System.ComponentModel.Design;
using Alteridem.GitHub.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VsSDK.IntegrationTestLibrary;
using Microsoft.VSSDK.Tools.VsIdeTesting;

namespace GitHubExtension_IntegrationTests
{

    [TestClass()]
    public class ToolWindowTest
    {
        private delegate void ThreadInvoker();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for showing the toolwindow
        ///</summary>
        [TestMethod()]
        [HostType("VS IDE")]
        public void ShowToolWindow()
        {
            UIThreadInvoker.Invoke((ThreadInvoker)delegate()
            {
                CommandID toolWindowCmd = new CommandID(GuidList.guidGitHubExtensionCmdSet, (int)PkgCmdIDList.cmdidIssues);

                TestUtils testUtils = new TestUtils();
                testUtils.ExecuteCommand(toolWindowCmd);

                Assert.IsTrue(testUtils.CanFindToolwindow(new Guid(GuidList.guidToolWindowPersistanceString)));

            });
        }

    }
}
