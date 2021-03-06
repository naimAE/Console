﻿using System;
using System.Management.Automation;
using Sitecore.Data;
using Sitecore.Data.Items;
using Spe.Commands.Interactive.Messages;

namespace Spe.Commands.Interactive
{
    [Cmdlet(VerbsCommunications.Receive, "File")]
    [OutputType(typeof (String),
        ParameterSetName = new[] {"Receive Media Item", "Receive File", "Receive Media Item Advanced"})]
    public class ReceiveFileCommand : BaseFormCommand
    {
        [Parameter(ParameterSetName = "Receive Media Item")]
        [Parameter(ParameterSetName = "Receive File")]
        public string Description { get; set; }

        [Parameter(ValueFromPipeline = true,
            Mandatory = true, Position = 0, ParameterSetName = "Receive Media Item")]
        [Parameter(ValueFromPipeline = true,
            Mandatory = true, Position = 0, ParameterSetName = "Receive Media Item Advanced")]
        public Item ParentItem { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true,
            Mandatory = true, Position = 0, ParameterSetName = "Receive File")]
        public string Path { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        [Parameter(ParameterSetName = "Receive File")]
        public override string Title { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        [Parameter(ParameterSetName = "Receive File")]
        public override string Icon { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        [Parameter(ParameterSetName = "Receive File")]
        public string CancelButtonName { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        [Parameter(ParameterSetName = "Receive File")]
        public string OkButtonName { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        public SwitchParameter Versioned { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        public string Language { get; set; }

        [Parameter(ParameterSetName = "Receive File")]
        [Parameter(ParameterSetName = "Receive Media Item")]
        public SwitchParameter Overwrite { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item")]
        [Parameter(ParameterSetName = "Receive File")]
        public SwitchParameter Unpack { get; set; }

        [Parameter(ParameterSetName = "Receive Media Item Advanced", Mandatory = true)]
        public SwitchParameter AdvancedDialog { get; set; }

        protected override void ProcessRecord()
        {
            LogErrors(() =>
            {
                if (!CheckSessionCanDoInteractiveAction()) return;

                AssertDefaultSize(500, AdvancedDialog ? 650 : 300);

                var message = new ShowUploadFileMessage(WidthString, HeightString, Title, Description, Icon,
                    OkButtonName ?? Sitecore.Texts.OK, CancelButtonName ?? Sitecore.Texts.CANCEL,
                    ParentItem?.ID.ToString() ?? Path,
                    Versioned, Language, Overwrite, Unpack, AdvancedDialog);

                PutMessage(message);
                var result = (string) message.GetResult();

                if (ID.TryParse(result, out var itemId))
                {
                    var item = ParentItem?.Database.GetItem(itemId);
                    WriteItem(item);
                }
                else
                {
                    WriteObject(string.IsNullOrEmpty(result) ? "cancel" : result);
                }
            });
        }
    }
}