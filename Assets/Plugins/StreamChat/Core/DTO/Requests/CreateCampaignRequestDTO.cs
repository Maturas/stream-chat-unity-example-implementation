//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------


using StreamChat.Core.DTO.Responses;
using StreamChat.Core.DTO.Events;
using StreamChat.Core.DTO.Models;

namespace StreamChat.Core.DTO.Requests
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v9.0.0.0))")]
    internal partial class CreateCampaignRequestDTO
    {
        [Newtonsoft.Json.JsonProperty("campaign", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public CampaignDataRequestDTO Campaign { get; set; } = new CampaignDataRequestDTO();

        private System.Collections.Generic.Dictionary<string, object> _additionalProperties = new System.Collections.Generic.Dictionary<string, object>();

        [Newtonsoft.Json.JsonExtensionData]
        public System.Collections.Generic.Dictionary<string, object> AdditionalProperties
        {
            get { return _additionalProperties; }
            set { _additionalProperties = value; }
        }

    }

}

