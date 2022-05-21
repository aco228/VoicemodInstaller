﻿using Newtonsoft.Json;
using VoicemodPowertools.Domain.Gitlab.Models;

namespace VoicemodPowertools.Domain.Gitlab.Jobs;

public record GitlabJobsRequest : GitlabPagination
{
    [JsonProperty("id")]
    public long ProjectId { get; set; }
}