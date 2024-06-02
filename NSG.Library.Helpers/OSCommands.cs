﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
//
// https://stackoverflow.com/questions/1469764/run-command-prompt-commands
//
namespace NSG.Library.Helpers
{
    //
    /// <summary>
    /// Static helpers for handling Operating System interaction.
    /// </summary>
    public static partial class OS
    {
        //
        /// <summary>
        /// Execute the operating system command.
        /// </summary>
        /// <param name="cmdStr">The command string to execute in the operating system.</param>
        /// <param name="workingDirectory">The base directory to start the OS command.</param>
        /// <param name="timeOut">How many millisecond to wait. -1 is infinite timeout.</param>
        /// <returns>String of the output of the command.</returns>
        public static string CallOperatingSystemCmd(string cmdStr, string workingDirectory, int timeOut)
        {
            var _return = new StringBuilder();
            var _error = new StringBuilder();
            if ( !string.IsNullOrEmpty( cmdStr ) )
            {
                System.Diagnostics.ProcessStartInfo _startInfo = new System.Diagnostics.ProcessStartInfo();
                _startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                _startInfo.FileName = "cmd.exe";
                _startInfo.WorkingDirectory = workingDirectory;
                _startInfo.RedirectStandardInput = true;
                _startInfo.RedirectStandardError = true;
                _startInfo.RedirectStandardOutput = true;
                _startInfo.UseShellExecute = false;
                _startInfo.Arguments = "/c " + cmdStr;
                using (Process _cmd = new Process())
                {
                    _cmd.StartInfo = _startInfo;
                    _cmd.EnableRaisingEvents = true;
                    _cmd.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
                    {
                        _return.AppendLine(e.Data);
                    });
                    _cmd.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
                    {
                        _error.AppendLine(e.Data);
                    });
                    _cmd.Start();
                    _cmd.BeginOutputReadLine();
                    _cmd.BeginErrorReadLine();
                    if (!_cmd.WaitForExit(timeOut)) // # of milliseconds
                    {
                        Console.WriteLine($"Command: {cmdStr} timed out after {timeOut} milliseconds.");
                        _cmd.Kill();
                    }
                    _cmd.Close();
                }
            }
            if( !string.IsNullOrEmpty(_error.ToString().Replace($"\r", "").Replace($"\n", "")))
            {
                _return.AppendLine("-- error ---");
                _return.Append(_error.ToString());
            }
            return _return.ToString();
        }
    }
}
//
