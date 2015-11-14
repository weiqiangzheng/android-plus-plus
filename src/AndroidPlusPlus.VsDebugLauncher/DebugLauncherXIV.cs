﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnvDTE;
using Microsoft.Build.Framework.XamlTypes;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.VCProjectEngine;
using Microsoft.VisualStudio.ProjectSystem;
using Microsoft.VisualStudio.ProjectSystem.Debuggers;
using Microsoft.VisualStudio.ProjectSystem.Utilities;
using Microsoft.VisualStudio.ProjectSystem.Utilities.DebuggerProviders;
using Microsoft.VisualStudio.ProjectSystem.VS.Debuggers;

using AndroidPlusPlus.Common;
using AndroidPlusPlus.VsDebugCommon;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.VsDebugLauncher
{

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  [ExportDebugger("AndroidPlusPlusDebugger")]

  [PartMetadata(ProjectCapabilities.Requires, ProjectCapabilities.VisualC)]

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class DebugLauncherXII : DebugLaunchProviderBase 
  {
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Import]
    private Rules.RuleProperties DebuggerProperties { get; set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private static IDebugLauncher s_debugLauncher;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static IDebugLauncher GetDebugLauncher (IServiceProvider serviceProvider)
    {
      if (s_debugLauncher == null)
      {
        s_debugLauncher = new DebugLauncher (serviceProvider);
      }

      return s_debugLauncher;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override async Task<bool> CanLaunchAsync (DebugLaunchOptions launchOptions)
    {
      LoggingUtils.PrintFunction ();

      IDebugLauncher debugLauncher = null;

      try
      {
        debugLauncher = GetDebugLauncher (ServiceProvider);

        return debugLauncher.CanLaunch ((int) launchOptions);
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        string description = string.Format ("'CanLaunchAsync' failed:\n[Exception] {0}", e.Message);

#if DEBUG
        description += "\n[Exception] Stack trace:\n" + e.StackTrace;
#endif

        if (debugLauncher != null)
        {
          LoggingUtils.RequireOk (debugLauncher.GetConnectionService ().LaunchDialogUpdate (description, true));
        }

        VsShellUtilities.ShowMessageBox (ServiceProvider, description, "Android++ Debugger", OLEMSGICON.OLEMSGICON_CRITICAL, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
      }

      return false;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public override async Task<IReadOnlyList<IDebugLaunchSettings>> QueryDebugTargetsAsync (DebugLaunchOptions launchOptions)
    {
      LoggingUtils.PrintFunction ();

      IDebugLauncher debugLauncher = null;

      DebugLaunchSettings debugLaunchSettings = new DebugLaunchSettings (launchOptions);

      try
      {
        debugLauncher = GetDebugLauncher (ServiceProvider);

        debugLauncher.PrepareLaunch ();

        Dictionary<string, string> projectProperties = await DebuggerProperties.ProjectPropertiesToDictionary ();

        projectProperties.Add ("ConfigurationGeneral.ProjectDir", Path.GetDirectoryName (DebuggerProperties.GetConfiguredProject ().UnconfiguredProject.FullPath));

        LaunchConfiguration launchConfig = debugLauncher.GetLaunchConfigurationFromProjectProperties (projectProperties);

        LaunchProps [] launchProps = debugLauncher.GetLaunchPropsFromProjectProperties (projectProperties);

        if (launchOptions.HasFlag (DebugLaunchOptions.NoDebug))
        {
          debugLaunchSettings = (DebugLaunchSettings) debugLauncher.StartWithoutDebugging ((int) launchOptions, launchConfig, launchProps, projectProperties);
        }
        else
        {
          debugLaunchSettings = (DebugLaunchSettings) debugLauncher.StartWithDebugging ((int) launchOptions, launchConfig, launchProps, projectProperties);
        }
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        string description = string.Format ("'QueryDebugTargetsAsync' failed:\n[Exception] {0}", e.Message);

#if DEBUG
        description += "\n[Exception] Stack trace:\n" + e.StackTrace;
#endif

        if (debugLauncher != null)
        {
          LoggingUtils.RequireOk (debugLauncher.GetConnectionService ().LaunchDialogUpdate (description, true));
        }

        VsShellUtilities.ShowMessageBox (ServiceProvider, description, "Android++ Debugger", OLEMSGICON.OLEMSGICON_CRITICAL, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
      }

      return new IDebugLaunchSettings [] { debugLaunchSettings };
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
