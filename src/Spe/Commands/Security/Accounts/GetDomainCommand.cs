﻿using System;
using System.Data;
using System.Management.Automation;
using Sitecore.Security.Domains;
using Sitecore.SecurityModel;
using Spe.Core.Validation;

namespace Spe.Commands.Security.Accounts
{
    [Cmdlet(VerbsCommon.Get, "Domain", DefaultParameterSetName = "Name")]
    [OutputType(typeof (Domain))]
    public class GetDomainCommand : BaseSecurityCommand
    {
        [Parameter(ParameterSetName = "Name")]
        [AutocompleteSet(nameof(DomainNames))]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            if (!String.IsNullOrEmpty(Name))
            {
                if (DomainManager.DomainExists(Name))
                {
                    WriteObject(DomainManager.GetDomain(Name));
                }
                else
                {
                    WriteError(typeof(ObjectNotFoundException), $"Cannot find a domain with name '{Name}'.", 
                        ErrorIds.DomainNotFound, ErrorCategory.ObjectNotFound, Name);
                }
            }
            else
            {
                WildcardWrite(String.Empty, DomainManager.GetDomains(), d => d.Name);
            }
        }
    }
}