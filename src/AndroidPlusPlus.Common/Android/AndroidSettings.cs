﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security;
using Microsoft.Win32;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.Common
{

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class AndroidSettings
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public enum VersionCode
    {
      BASE = 1,
      BASE_1_1,
      CUPCAKE,
      DONUT,
      ECLAIR,
      ECLAIR_0_1,
      ECLAIR_MR1,
      FROYO,
      GINGERBREAD,
      GINGERBREAD_MR1,
      HONEYCOMB,
      HONEYCOMB_MR1,
      HONEYCOMB_MR2,
      ICE_CREAM_SANDWICH,
      ICE_CREAM_SANDWICH_MR1,
      JELLY_BEAN,
      JELLY_BEAN_MR1,
      JELLY_BEAN_MR2,
      KITKAT,
      KITKAT_WATCH,
      L_PREVIEW = KITKAT_WATCH,
      LOLLIPOP,
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public enum SdkTools
    {
      SdkManager,
      AvdManager,
      Aapt,
      Adb,
      Aidl,
      Dx,
      Android,
      Ddms,
      Monitor,
      Zipalign
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private static string [] defaultSdkToolPaths =
    {
      @"SDK Manager.exe",
      @"AVD Manager.exe",
      @"platform-tools\aapt.exe",
      @"platform-tools\adb.exe",
      @"platform-tools\aidl.exe",
      @"platform-tools\dx.bat",
      @"tools\android.bat",
      @"tools\ddms.bat",
      @"tools\monitor.bat",
      @"tools\zipalign.exe"
    };

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static string SdkRoot
    {
      get
      {
        // 
        // Probe for possible Android SDK installation directories.
        // 

        HashSet<string> androidSdkPossibleLocations = new HashSet<string> (); 

        try
        {
          string [] environmentPaths = new string [] 
          {
            Environment.GetEnvironmentVariable ("ANDROID_SDK"),
            Environment.GetEnvironmentVariable ("ANDROID_SDK_ROOT"),
            Environment.GetEnvironmentVariable ("ANDROID_HOME")
          };

          foreach (string possiblePath in environmentPaths)
          {
            if ((!string.IsNullOrEmpty (possiblePath)) && (!androidSdkPossibleLocations.Contains (possiblePath)))
            {
              androidSdkPossibleLocations.Add (possiblePath);
            }
          }
        }
        catch (SecurityException e)
        {
          LoggingUtils.Print (string.Format ("Failed retrieving ANDROID_SDK_* environment variables: {0}", e.Message));

          LoggingUtils.HandleException (e);
        }

        using (RegistryKey localMachineAndroidSdkTools = Registry.LocalMachine.OpenSubKey (@"SOFTWARE\Android SDK Tools\"))
        {
          if (localMachineAndroidSdkTools != null)
          {
            androidSdkPossibleLocations.Add (localMachineAndroidSdkTools.GetValue ("Path") as string);
          }
        }

        using (RegistryKey currentUserAndroidSdkTools = Registry.CurrentUser.OpenSubKey (@"SOFTWARE\Android SDK Tools\"))
        {
          if (currentUserAndroidSdkTools != null)
          {
            androidSdkPossibleLocations.Add (currentUserAndroidSdkTools.GetValue ("Path") as string);
          }
        }

        // 
        // Search specified path the default 'SDK Manager' executable.
        // 

        foreach (string location in androidSdkPossibleLocations)
        {
          if (location != null)
          {
            if (File.Exists (Path.Combine(location, GetSdkToolPathFromRoot (SdkTools.SdkManager))))
            {
              return location;
            }
          }
        }

        return null;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static List <VersionCode> SdkInstalledPlatforms
    {
      get
      {
        // 
        // Evaluate which 'platforms' are supported by this distribution. (This implemenation is crude, should use Android.bat).
        // 

        List <VersionCode> installedSdkPlatforms = new List <VersionCode> ();

        string platformSrcPath = SdkRoot + @"\platforms";

        if (File.Exists (platformSrcPath))
        {
          string [] platformDirs = Directory.GetDirectories (platformSrcPath);

          for (uint i = 0; i < platformDirs.Length; ++i)
          {
            if (platformDirs [i].StartsWith ("android-"))
            {
              VersionCode versionCode = (VersionCode) uint.Parse (platformDirs [i].Substring ("android-".Length - 1));

              installedSdkPlatforms.Add (versionCode);
            }
          }
        }

        if (installedSdkPlatforms.Count == 0)
        {
          throw new InvalidOperationException ();
        }

        return installedSdkPlatforms;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static string GetSdkToolPathFromRoot (SdkTools tool)
    {
      return defaultSdkToolPaths [(uint)tool];
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static string NdkRoot
    {
      get
      {
        // 
        // Probe for possible Android NDK installation directories.
        // 

        List <string> androidNdkPossibleLocations = new List <string> (2); 

        try
        {
          androidNdkPossibleLocations.Add (Environment.GetEnvironmentVariable ("ANDROID_NDK"));

          androidNdkPossibleLocations.Add (Environment.GetEnvironmentVariable ("ANDROID_NDK_ROOT"));
        }
        catch (SecurityException e)
        {
          LoggingUtils.Print (string.Format ("Failed retrieving ANDROID_NDK_* environment variables: {0}", e.Message));

          LoggingUtils.HandleException (e);
        }

        // 
        // Search specified path the default 'ndk-build' script.
        // 

        foreach (string location in androidNdkPossibleLocations)
        {
          if (location != null)
          {
            if (File.Exists (Path.Combine (location, "ndk-build")))
            {
              return location;
            }
          }
        }

        return null;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public static List <VersionCode> NdkInstalledPlatforms
    {
      get
      {
        // 
        // Evaluate which 'platforms' are supported by this distribution. (This implemenation is crude, should use Android.bat).
        // 

        List <VersionCode> installedNdkPlatforms = new List <VersionCode> ();

        string platformSrcPath = NdkRoot + @"\platforms";

        if (File.Exists (platformSrcPath))
        {
          string [] platformDirs = Directory.GetDirectories (platformSrcPath);

          for (uint i = 0; i < platformDirs.Length; ++i)
          {
            if (platformDirs [i].StartsWith ("android-"))
            {
              VersionCode versionCode = (VersionCode) uint.Parse (platformDirs [i].Substring ("android-".Length - 1));

              installedNdkPlatforms.Add (versionCode);
            }
          }
        }

        if (installedNdkPlatforms.Count == 0)
        {
          throw new InvalidOperationException ();
        }

        return installedNdkPlatforms;
      }
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
