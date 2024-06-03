using Giantnodes.Infrastructure.Messages;
using Giantnodes.Infrastructure.Uow.Execution;

namespace Giantnodes.Infrastructure.Uow;

public abstract class UnitOfWork : IUnitOfWork
{
    private readonly IUnitOfWorkExecutor _executor;

    private bool _started;
    private bool _committed;
    private Exception? _exception;

    /// <summary>
    /// Gets the list of domain events that occurred during the execution of the unit of work.
    /// </summary>
    protected List<Event> DomainEvents { get; }

    /// <summary>
    /// Gets the unique correlation identifier for this unit of work.
    /// </summary>
    public Guid CorrelationId { get; }

    /// <summary>
    /// Gets the user identifier associated with this unit of work, or null if not set.
    /// </summary>
    public Guid? UserId { get; private set; }

    /// <summary>
    /// Gets the tenant identifier associated with this unit of work, or null if not set.
    /// </summary>
    public Guid? TenantId { get; private set; }

    /// <summary>
    /// Gets the value indicating whether this unit of work has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Occurs when the unit of work has been successfully committed.
    /// </summary>
    public event EventHandler? Completed;

    /// <summary>
    /// Occurs when an exception is thrown during the execution of the unit of work.
    /// </summary>
    public event EventHandler? Failed;

    /// <summary>
    /// Occurs when the unit of work is disposed.
    /// </summary>
    public event EventHandler? Disposed;

    /// <summary>
    /// Gets the options used to configure the behavior of this unit of work.
    /// </summary>
    public UnitOfWorkOptions? Options { get; private set; }

    /// <summary>
    /// Gets a read-only collection of domain events that occurred during the execution of the unit of work.
    /// </summary>
    public IReadOnlyCollection<object> Events => DomainEvents.AsReadOnly();

    protected UnitOfWork(IUnitOfWorkExecutor executor)
    {
        _executor = executor;

        CorrelationId = Guid.NewGuid();
        DomainEvents = new List<Event>();
    }

    /// <summary>
    /// Begins the unit of work with the specified options.
    /// </summary>
    /// <param name="options">The options used to configure the behavior of the unit of work.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>The unit of work instance.</returns>
    public async Task<IUnitOfWork> BeginAsync(UnitOfWorkOptions options, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (_started)
            throw new InvalidOperationException("The Uow has already started.");

        Options = options;

        try
        {
            await OnBeginAsync(options, cancellation);
            _started = true;
            return this;
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }
    }

    /// <summary>
    /// Commits the unit of work.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    public async Task CommitAsync(CancellationToken cancellation = default)
    {
        if (_committed)
            throw new InvalidOperationException("The Uow has already been committed.");

        try
        {
            await OnCommitAsync(cancellation);
            await _executor.OnAfterCommitAsync(this, cancellation);
            _committed = true;
            Completed?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _exception = ex;
            throw;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc cref="Dispose" />
    protected virtual void Dispose(bool dispose)
    {
        if (!dispose || !_started || IsDisposed)
            return;

        IsDisposed = true;

        if (_exception != null)
            Failed?.Invoke(this, new UnitOfWorkFailedEventArgs(_exception));

        Disposed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Performs any necessary initialization or setup tasks before the unit of work begins.
    /// </summary>
    /// <param name="options">The options used to configure the behavior of the unit of work.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    protected abstract Task OnBeginAsync(UnitOfWorkOptions options, CancellationToken cancellation = default);

    /// <summary>
    /// Performs any necessary commit operations after the unit of work has completed successfully.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    protected abstract Task OnCommitAsync(CancellationToken cancellation = default);
}