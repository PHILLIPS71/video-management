using Giantnodes.Service.Dashboard.Domain.Entities.Files.Streams;
using Giantnodes.Service.Encoder.Application.Contracts.Common;
using Riok.Mapperly.Abstractions;

namespace Giantnodes.Service.Dashboard.Application.Components;

[Mapper]
public partial class Mapper
{
    [MapperIgnoreSource(nameof(FileVideoStream.Default))]
    [MapperIgnoreSource(nameof(FileVideoStream.Forced))]
    public partial FileSystemFileVideoStream Map(FileVideoStream dto);

    [MapperIgnoreSource(nameof(FileAudioStream.Default))]
    [MapperIgnoreSource(nameof(FileAudioStream.Forced))]
    public partial FileSystemFileAudioStream Map(FileAudioStream dto);

    [MapperIgnoreSource(nameof(FileSubtitleStream.Default))]
    [MapperIgnoreSource(nameof(FileSubtitleStream.Forced))]
    public partial FileSystemFileSubtitleStream Map(FileSubtitleStream dto);
}