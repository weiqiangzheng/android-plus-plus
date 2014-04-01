﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.Debugger.Interop;
using AndroidPlusPlus.Common;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace AndroidPlusPlus.VsDebugEngine
{
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  // 
  // IDebugProcess2:
  // This interface represents a process running on a port. 
  // If the port is the local port, then IDebugProcess2 usually represents a physical process on the local machine.
  // 

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class DebuggeeProcess : IDebugProcess3, IDebugProcessSecurity2
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Enumerator : DebugEnumerator<IDebugProcess2, IEnumDebugProcesses2>, IEnumDebugProcesses2
    {
      public Enumerator (List<IDebugProcess2> processes)
        : base (processes)
      {
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private readonly DebuggeePort m_debuggeePort;

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebuggeeProcess (DebuggeePort port, AndroidProcess androidProcess)
    {
      m_debuggeePort = port;

      DebuggeeProgram = new DebuggeeProgram (this);

      Guid = Guid.NewGuid ();

      NativeProcess = androidProcess;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Guid Guid { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public AndroidProcess NativeProcess { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebuggeeProgram DebuggeeProgram { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region IDebugProcess3 Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Attach (IDebugEventCallback2 pCallback, Guid [] rgguidSpecificEngines, uint celtSpecificEngines, int [] rghrEngineAttach)
    {
      // 
      // Attaches to the process.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebuggeeProgram.Attach (pCallback));

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int CanDetach ()
    {
      // 
      // Determines if the SDM can detach the process.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebuggeeProgram.CanDetach ());

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int CauseBreak ()
    {
      // 
      // Requests that the next program running code in this process stop.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebuggeeProgram.CauseBreak ());

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Continue (IDebugThread2 pThread)
    {
      // 
      // Continues running this process from a stopped state. Any previous execution state (such as a step) is preserved, and the process starts executing again.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Detach ()
    {
      // 
      // Detaches the debugger from the process.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebuggeeProgram.Detach ());

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int DisableENC (EncUnavailableReason reason)
    {
      // 
      // This method explicitly disables Edit and Continue on this process (and all programs it contains). A custom port supplier should always return E_NOTIMPL.
      // 

      LoggingUtils.PrintFunction ();

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int EnumPrograms (out IEnumDebugPrograms2 ppEnum)
    {
      // 
      // Enumerates the programs that are contained in this process.
      // 

      LoggingUtils.PrintFunction ();

      ppEnum = new DebuggeeProgram.Enumerator (new List<IDebugProgram2> { DebuggeeProgram });

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int EnumThreads (out IEnumDebugThreads2 ppEnum)
    {
      // 
      // Enumerates the threads running in the process.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebuggeeProgram.EnumThreads (out ppEnum));

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        ppEnum = null;

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Execute (IDebugThread2 pThread)
    {
      // 
      // Continues running this process from a stopped state. Any previous execution state (such as a step) is cleared and the process starts executing again.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Obsolete ("These methods are not called by the Visual Studio debugger.")]
    public int GetAttachedSessionName (out string pbstrSessionName)
    {
      // 
      // Gets the name of the session that is debugging the process. [DEPRECATED. SHOULD ALWAYS RETURN E_NOTIMPL.]
      // 

      LoggingUtils.PrintFunction ();

      pbstrSessionName = null;

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetDebugReason (enum_DEBUG_REASON [] pReason)
    {
      // 
      // Gets the reason that the process was launched for debugging.
      // 

      LoggingUtils.PrintFunction ();

      pReason [0] = enum_DEBUG_REASON.DEBUG_REASON_USER_LAUNCHED;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetENCAvailableState (EncUnavailableReason [] pReason)
    {
      // 
      // Get the ENC state for this process. A custom port supplier does not implement this method (it should always return E_NOTIMPL).
      // 

      LoggingUtils.PrintFunction ();

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetEngineFilter (GUID_ARRAY [] pEngineArray)
    {
      // 
      // Retrieves an array of unique identifiers for available debug engines.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetHostingProcessLanguage (out Guid pguidLang)
    {
      // 
      // Retrieves the language currently set for this process.
      // 

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetInfo (enum_PROCESS_INFO_FIELDS Fields, PROCESS_INFO [] infoArray)
    {
      // 
      // Gets a description of the process.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        infoArray [0] = new PROCESS_INFO ();

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_FILE_NAME) != 0)
        {
          LoggingUtils.RequireOk (GetName (enum_GETNAME_TYPE.GN_FILENAME, out infoArray [0].bstrFileName));

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_FILE_NAME;
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_BASE_NAME) != 0)
        {
          LoggingUtils.RequireOk (GetName (enum_GETNAME_TYPE.GN_BASENAME, out infoArray [0].bstrBaseName));

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_BASE_NAME;
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_TITLE) != 0)
        {
          LoggingUtils.RequireOk (GetName (enum_GETNAME_TYPE.GN_TITLE, out infoArray [0].bstrTitle));

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_TITLE;
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_PROCESS_ID) != 0)
        {
          AD_PROCESS_ID [] processId = new AD_PROCESS_ID [1];

          LoggingUtils.RequireOk (GetPhysicalProcessId (processId));

          infoArray [0].ProcessId = processId [0];

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_PROCESS_ID;
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_SESSION_ID) != 0)
        {
          // We currently don't support multiple sessions, so all processes are in session 1.

          infoArray [0].dwSessionId = 1;

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_SESSION_ID;
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_ATTACHED_SESSION_NAME) != 0)
        {
          // Oddly enough, SESSION_NAME is requested... even though the docs clearly state that it's deprecated.

          infoArray [0].bstrAttachedSessionName = "[Attached session name is deprecated]";

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_ATTACHED_SESSION_NAME;
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_CREATION_TIME) != 0)
        {
          // Not entirely clear how this should be implemented.

          /*Microsoft.VisualStudio.OLE.Interop.FILETIME filetime;

          infoArray [0].CreationTime = filetime;

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_CREATION_TIME;*/
        }

        if ((Fields & enum_PROCESS_INFO_FIELDS.PIF_FLAGS) != 0)
        {
          infoArray [0].Flags = 0;

          infoArray [0].Flags |= enum_PROCESS_INFO_FLAGS.PIFLAG_PROCESS_RUNNING;

          if (DebuggeeProgram.AttachedEngine != null)
          {
            infoArray [0].Flags |= enum_PROCESS_INFO_FLAGS.PIFLAG_DEBUGGER_ATTACHED;
          }

          //enum_PROCESS_INFO_FLAGS.PIFLAG_DEBUGGER_ATTACHED
          //enum_PROCESS_INFO_FLAGS.PIFLAG_PROCESS_STOPPED
          //enum_PROCESS_INFO_FLAGS.PIFLAG_PROCESS_RUNNING

          if (!NativeProcess.IsUserProcess)
          {
            infoArray [0].Flags |= enum_PROCESS_INFO_FLAGS.PIFLAG_SYSTEM_PROCESS;
          }

          infoArray [0].Fields |= enum_PROCESS_INFO_FIELDS.PIF_FLAGS;
        }

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetName (enum_GETNAME_TYPE gnType, out string pbstrName)
    {
      // 
      // Gets the title, friendly name, or file name of the process.
      // 

      LoggingUtils.PrintFunction ();

      pbstrName = NativeProcess.Name;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetPhysicalProcessId (AD_PROCESS_ID [] pProcessId)
    {
      // 
      // Gets the system process identifier.
      // 

      LoggingUtils.PrintFunction ();

      pProcessId [0] = new AD_PROCESS_ID ();

      pProcessId [0].dwProcessId = NativeProcess.Pid;

      pProcessId [0].guidProcessId = Guid;

      pProcessId [0].ProcessIdType = (uint)enum_AD_PROCESS_ID.AD_PROCESS_ID_SYSTEM;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetPort (out IDebugPort2 ppPort)
    {
      // 
      // Gets the port that this process is running on.
      // 

      LoggingUtils.PrintFunction ();

      ppPort = m_debuggeePort as IDebugPort2;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetProcessId (out Guid pguidProcessId)
    {
      // 
      // Gets a globally unique identifier for this process.
      // 

      LoggingUtils.PrintFunction ();

      pguidProcessId = Guid;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetServer (out IDebugCoreServer2 ppServer)
    {
      // 
      // Gets the instance of a machine server this process is running on.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int SetHostingProcessLanguage (ref Guid guidLang)
    {
      // 
      // Sets the hosting language so that the debug engine can load the appropriate expression evaluator.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Step (IDebugThread2 pThread, enum_STEPKIND sk, enum_STEPUNIT Step)
    {
      // 
      // Steps forward one instruction or statement in the process.
      // 

      LoggingUtils.PrintFunction ();

      throw new NotImplementedException ();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Terminate ()
    {
      // 
      // Terminates the process.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebuggeeProgram.Terminate ());

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        return DebugEngineConstants.E_FAIL;
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region IDebugProcess3 Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region IDebugProcessSecurity2 Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetUserName (out string pbstrUserName)
    {
      LoggingUtils.PrintFunction ();

      pbstrUserName = NativeProcess.User;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int QueryCanSafelyAttach ()
    {
      LoggingUtils.PrintFunction ();

      if (NativeProcess.IsUserProcess)
      {
        return DebugEngineConstants.S_OK;
      }

      return DebugEngineConstants.S_FALSE;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #endregion

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
