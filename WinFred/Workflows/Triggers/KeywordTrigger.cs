﻿using System.Runtime.Serialization;

namespace James.Workflows.Triggers
{
    [DataContract]
    public class KeywordTrigger : BasicTrigger
    {
        public KeywordTrigger()
        {
        }

        public KeywordTrigger(Workflow parent) : base(parent)
        {
        }

        [DataMember]
        [ComponentField("Listens for this keyword to trigger")]
        public string Keyword { get; set; } = "";

        public override string GetSummary() => $"Triggers for \"{Keyword}\"";

        public override void Run(string arguments = "") => TriggerRunables(arguments);

        public override bool IsAllowed(WorkflowComponent source) => false;
    }
}