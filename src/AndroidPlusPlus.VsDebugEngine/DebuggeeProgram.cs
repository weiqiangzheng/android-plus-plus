﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
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
  // A debug engine (DE) or a custom port supplier implements this interface to represent a 'program' that can be debugged. 
  // A 'program' is a thread container running in a particular run-time architecture, while a process is made up of one or more programs.
  // 

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  public class DebuggeeProgram : IDebugProgram3, IDebugProgramNode2
  {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Enumerator : DebugEnumerator<IDebugProgram2, IEnumDebugPrograms2>, IEnumDebugPrograms2
    {
      public Enumerator (List<IDebugProgram2> programs)
        : base (programs)
      {
      }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebuggeeProgram (DebuggeeProcess process)
    {
      Guid = Guid.NewGuid ();

      DebugProcess = process;

      AttachedEngine = null;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public Guid Guid { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebuggeeProcess DebugProcess { get; protected set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public DebugEngine AttachedEngine { get; set; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region IDebugProgram3 Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Attach (IDebugEventCallback2 pCallback)
    {
      // 
      // Attaches to this program.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.Attach (pCallback));

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
      // Determines if a debug engine (DE) can detach from the program.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.CanDetach ());

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
      // Requests that this program stop execution the next time one of its threads runs code.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.CauseBreak ());

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
      // Continues running this program from a stopped state. Any previous execution state is preserved.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.Continue (pThread));

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

    public int Detach ()
    {
      // 
      // Detaches the debugger from this program.
      // 

      LoggingUtils.PrintFunction ();

      uint exitCode = 0;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.Detach ());

        AttachedEngine.Broadcast (new DebugEngineEvent.ProgramDestroy (exitCode), this, null);

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

    public int EnumCodeContexts (IDebugDocumentPosition2 pDocPos, out IEnumDebugCodeContexts2 ppEnum)
    {
      // 
      // Enumerates the code contexts for a given position in a source file.
      // 

      LoggingUtils.PrintFunction ();

      ppEnum = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.EnumCodeContexts (pDocPos, out ppEnum));

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

    public int EnumCodePaths (string pszHint, IDebugCodeContext2 pStart, IDebugStackFrame2 pFrame, int fSource, out IEnumCodePaths2 ppEnum, out IDebugCodeContext2 ppSafety)
    {
      // 
      // Enumerates the code paths of this program.
      // 

      LoggingUtils.PrintFunction ();

      ppEnum = null;

      ppSafety = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.EnumCodePaths (pszHint, pStart, pFrame, fSource, out ppEnum, out ppSafety));

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

    public int EnumModules (out IEnumDebugModules2 ppEnum)
    {
      // 
      // Enumerates the modules that this program has loaded and is executing.
      // 

      LoggingUtils.PrintFunction ();

      ppEnum = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.EnumModules (out ppEnum));

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

    public int EnumThreads (out IEnumDebugThreads2 ppEnum)
    {
      // 
      // Enumerates the threads that are running in this program.
      // 

      LoggingUtils.PrintFunction ();

      ppEnum = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.EnumThreads (out ppEnum));

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

    public int Execute ()
    {
      // 
      // Continues running this program from a stopped state. Any previous execution state is cleared.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.Execute ());

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

    public int GetDebugProperty (out IDebugProperty2 ppProperty)
    {
      // 
      // Gets program properties.
      // 

      LoggingUtils.PrintFunction ();

      ppProperty = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.GetDebugProperty (out ppProperty));

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

    public int GetDisassemblyStream (enum_DISASSEMBLY_STREAM_SCOPE dwScope, IDebugCodeContext2 pCodeContext, out IDebugDisassemblyStream2 ppDisassemblyStream)
    {
      // 
      // Gets the disassembly stream for this program or part of this program.
      // 

      LoggingUtils.PrintFunction ();

      ppDisassemblyStream = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.GetDisassemblyStream (dwScope, pCodeContext, out ppDisassemblyStream));

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

    public int GetENCUpdate (out object ppUpdate)
    {
      // 
      // Gets the Edit and Continue (ENC) update for this program.
      // A custom debug engine does not implement this method (it should always return E_NOTIMPL).
      // 

      LoggingUtils.PrintFunction ();

      ppUpdate = null;

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetEngineInfo (out string pbstrEngine, out Guid pguidEngine)
    {
      // 
      // Gets the name and identifier of the debug engine (DE) running a program.
      // 

      LoggingUtils.PrintFunction ();

      pguidEngine = DebugEngineGuids.guidDebugEngineID;

      pbstrEngine = DebugEngineGuids.GetEngineNameFromId (pguidEngine);

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetMemoryBytes (out IDebugMemoryBytes2 ppMemoryBytes)
    {
      // 
      // Gets the memory bytes for this program.
      // 

      LoggingUtils.PrintFunction ();

      ppMemoryBytes = null;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        return AttachedEngine.NativeDebugger.NativeProgram.GetMemoryBytes (out ppMemoryBytes);
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

    public int GetName (out string pbstrName)
    {
      // 
      // Gets the name of the program.
      // 

      LoggingUtils.PrintFunction ();

      pbstrName = DebugProcess.NativeProcess.Name;

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.GetName (out pbstrName));

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        // Use the default process name.
      }

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetProcess (out IDebugProcess2 ppProcess)
    {
      // 
      // Gets the process that this program is running in.
      // 

      LoggingUtils.PrintFunction ();

      ppProcess = DebugProcess;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int GetProgramId (out Guid pguidProgramId)
    {
      // 
      // Gets a globally unique identifier for this program.
      // 

      LoggingUtils.PrintFunction ();

      pguidProgramId = Guid;

      return DebugEngineConstants.S_OK;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int Step (IDebugThread2 pThread, enum_STEPKIND sk, enum_STEPUNIT Step)
    {
      // 
      // Performs a step.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.Step (pThread, sk, Step));

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

    public int Terminate ()
    {
      // 
      // Terminates this program.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.Terminate ());

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

    public int WriteDump (enum_DUMPTYPE DUMPTYPE, string pszDumpUrl)
    {
      // 
      // Writes a dump to a file.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.WriteDump (DUMPTYPE, pszDumpUrl));

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

    #region IDebugProgram3 Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public int ExecuteOnThread (IDebugThread2 pThread)
    {
      // 
      // ExecuteOnThread is called when the Session Debug Manager (SDM) wants execution to continue and have stepping state cleared.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (AttachedEngine == null)
        {
          throw new InvalidOperationException ();
        }

        if (AttachedEngine.NativeDebugger == null)
        {
          throw new InvalidOperationException ();
        }

        LoggingUtils.RequireOk (AttachedEngine.NativeDebugger.NativeProgram.ExecuteOnThread (pThread));

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

    #region IDebugProgramNode2 Members

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Obsolete ("These methods are not called by the Visual Studio debugger.")]
    int IDebugProgramNode2.Attach_V7 (IDebugProgram2 pMDMProgram, IDebugEventCallback2 pCallback, uint dwReason)
    {
      LoggingUtils.PrintFunction ();

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Obsolete ("These methods are not called by the Visual Studio debugger.")]
    int IDebugProgramNode2.DetachDebugger_V7 ()
    {
      LoggingUtils.PrintFunction ();

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    [Obsolete ("These methods are not called by the Visual Studio debugger.")]
    int IDebugProgramNode2.GetHostMachineName_V7 (out string pbstrHostMachineName)
    {
      LoggingUtils.PrintFunction ();

      pbstrHostMachineName = string.Empty;

      return DebugEngineConstants.E_NOTIMPL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int IDebugProgramNode2.GetHostName (enum_GETHOSTNAME_TYPE dwHostNameType, out string pbstrHostName)
    {
      // 
      // Gets the name of the process hosting the program.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        if (dwHostNameType == enum_GETHOSTNAME_TYPE.GHN_FRIENDLY_NAME)
        {
          LoggingUtils.RequireOk (DebugProcess.GetName (enum_GETNAME_TYPE.GN_NAME, out pbstrHostName));

          return DebugEngineConstants.S_OK;
        }
        else if (dwHostNameType == enum_GETHOSTNAME_TYPE.GHN_FILE_NAME)
        {
          LoggingUtils.RequireOk (DebugProcess.GetName (enum_GETNAME_TYPE.GN_FILENAME, out pbstrHostName));

          return DebugEngineConstants.S_OK;
        }
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);
      }

      pbstrHostName = string.Empty;

      return DebugEngineConstants.E_FAIL;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    int IDebugProgramNode2.GetHostPid (AD_PROCESS_ID [] pHostProcessId)
    {
      //
      // Gets the system process identifier for the process hosting the program.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebugProcess.GetPhysicalProcessId (pHostProcessId));

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

    int IDebugProgramNode2.GetProgramName (out string pbstrProgramName)
    {
      // 
      // Gets the name of the program.
      // 

      LoggingUtils.PrintFunction ();

      try
      {
        LoggingUtils.RequireOk (DebugProcess.GetName (enum_GETNAME_TYPE.GN_NAME, out pbstrProgramName));

        return DebugEngineConstants.S_OK;
      }
      catch (Exception e)
      {
        LoggingUtils.HandleException (e);

        pbstrProgramName = string.Empty;

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

  }

  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////