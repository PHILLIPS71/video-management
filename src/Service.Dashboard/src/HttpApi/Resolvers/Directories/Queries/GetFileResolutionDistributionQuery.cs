// using Giantnodes.Infrastructure;
// using Giantnodes.Infrastructure.Faults.Exceptions;
// using Giantnodes.Infrastructure.Faults.Types;
// using Giantnodes.Infrastructure.Validation.Exceptions;
// using Giantnodes.Service.Dashboard.Application.Contracts.Directories.Queries;
// using Giantnodes.Service.Dashboard.Domain.Enumerations;
// using MassTransit;
//
// namespace Giantnodes.Service.Dashboard.HttpApi.Resolvers.Directories.Queries;
//
// public readonly record struct FileResolutionDistribution(VideoResolution? Resolution, int Count);
//
// [QueryType]
// public class GetFileResolutionDistributionQuery
// {
//     [Error<DomainException>]
//     [Error<ValidationException>]
//     [UseSorting]
//     public async Task<IQueryable<FileResolutionDistribution>> FileResolutionDistribution(
//         [Service] IRequestClient<GetFileResolutionDistribution.Query> request,
//         [ID] Guid directoryId,
//         CancellationToken cancellation = default)
//     {
//         var query = new GetFileResolutionDistribution.Query
//         {
//             DirectoryId = directoryId
//         };
//
//         Response response = await request.GetResponse<GetFileResolutionDistribution.Result, DomainFault, ValidationFault>(query, cancellation);
//         return response switch
//         {
//             (_, GetFileResolutionDistribution.Result result) =>
//                 result
//                     .Distribution
//                     .Select(x => new FileResolutionDistribution
//                     {
//                         Resolution = Enumeration.TryParse<VideoResolution>(x.Key ?? 0),
//                         Count = x.Value
//                     })
//                     .AsQueryable(),
//             (_, DomainFault fault) => throw new DomainException(fault),
//             (_, ValidationFault fault) => throw new ValidationException(fault),
//             _ => throw new InvalidOperationException()
//         };
//     }
// }
//
// // [ObjectType<FileResolutionDistribution>]
// // public static partial class FileResolutionDistributionType
// // {
// //     static partial void Configure(IObjectTypeDescriptor<FileResolutionDistribution> descriptor)
// //     {
// //         descriptor
// //             .Field(p => p.Resolution);
// //
// //         descriptor
// //             .Field(p => p.Count);
// //     }
// // }