﻿using System.Diagnostics;
using System.IO.Abstractions;
using Giantnodes.Infrastructure.Faults;
using Giantnodes.Service.Encoder.Application.Contracts.Encoding.Commands;
using MassTransit;
using Serilog;

namespace Giantnodes.Service.Encoder.Application.Components.Encoding.Jobs;

public class TransferFileConsumer : IJobConsumer<TransferFile.Job>
{
    private const FileOptions FileOption = FileOptions.Asynchronous | FileOptions.SequentialScan;
    private const int BufferSize = 4096;

    private readonly IFileSystem _fs;

    public TransferFileConsumer(IFileSystem fs)
    {
        _fs = fs;
    }

    public async Task Run(JobContext<TransferFile.Job> context)
    {
        var input = _fs.FileInfo.New(context.Job.InputFilePath);
        if (!input.Exists)
        {
            await context.RejectAsync(FaultKind.NotFound, nameof(context.Job.InputFilePath));
            return;
        }

        var stopwatch = new Stopwatch();

        stopwatch.Start();
        await using var source = _fs.FileStream.New(input.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize, FileOption);
        await using var destination = _fs.FileStream.New(context.Job.OutputFilePath, FileMode.Create, FileAccess.Write, FileShare.None, BufferSize, FileOption);
        await source.CopyToAsync(destination, context.CancellationToken);
        stopwatch.Start();

        Log.Information("Successfully transferred {0} to {1} with job id {2} in {3:000ms}.", input.FullName, context.Job.OutputFilePath, context.JobId, stopwatch.ElapsedMilliseconds);
    }
}