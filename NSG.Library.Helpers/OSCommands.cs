using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
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
            string _return = "";
            if (cmdStr != "")
            {
                System.Diagnostics.Process _cmd = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo _startInfo = new System.Diagnostics.ProcessStartInfo();
                _startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                _startInfo.FileName = "cmd.exe";
                _startInfo.WorkingDirectory = workingDirectory;
                _startInfo.RedirectStandardInput = true;
                _startInfo.RedirectStandardOutput = true;
                _startInfo.UseShellExecute = false;
                _startInfo.Arguments = "/c " + cmdStr;
                _cmd.StartInfo = _startInfo;
                _cmd.Start();
                _cmd.StandardInput.Flush();
                _cmd.StandardInput.Close();
                _cmd.WaitForExit(timeOut); // # of milliseconds
                _return = _cmd.StandardOutput.ReadToEnd();
            }
            return _return;
        }
    }
}
//
