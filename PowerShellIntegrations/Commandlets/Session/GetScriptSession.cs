﻿using System;
using System.Linq;
using System.Management.Automation;
using Cognifide.PowerShell.PowerShellIntegrations.Host;

namespace Cognifide.PowerShell.PowerShellIntegrations.Commandlets.Session
{
    [Cmdlet("Get", "ScriptSession")]
    public class GetScriptSession : BaseCommand
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public SwitchParameter Current { get; set; }

        protected override void ProcessRecord()
        {
            if (Current.IsPresent)
            {
                var scriptingHostPrivateData = Host.PrivateData.BaseObject() as ScriptingHostPrivateData;
                if (scriptingHostPrivateData == null) return;

                var id = scriptingHostPrivateData.SessionId;
                if (!string.IsNullOrEmpty(id))
                {
                    WriteObject(
                        ScriptSessionManager.GetAll().Where(s => id.Equals(s.ID, StringComparison.OrdinalIgnoreCase)),
                        true);
                }
            }
            else if (!string.IsNullOrEmpty(Id))
            {
                WriteObject(ScriptSessionManager.GetSession(Id));
            }
            else
            {
                WriteObject(ScriptSessionManager.GetAll());
            }
        }
    }
}