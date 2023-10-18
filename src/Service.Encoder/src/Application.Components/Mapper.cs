using Giantnodes.Service.Encoder.Application.Contracts.Common;
using Riok.Mapperly.Abstractions;
using Xabe.FFmpeg;

namespace Giantnodes.Service.Encoder.Application.Components;

[Mapper]
public partial class Mapper
{
    [MapProperty(nameof(IVideoStream.Ratio), nameof(FileVideoStream.AspectRatio))]
    public partial FileVideoStream Map(IVideoStream dto);

    public partial FileAudioStream Map(IAudioStream dto);

    public partial FileSubtitleStream Map(ISubtitleStream dto);
}