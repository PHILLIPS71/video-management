namespace Giantnodes.Infrastructure.Uow;

public abstract class UnitOfWork : IUnitOfWork
{
    private bool _started;
    private bool _committed;
    private Exception? _exception;

    public Guid CorrelationId { get; }

    public Guid? UserId { get; private set; }

    public Guid? TenantId { get; private set; }

    public bool IsDisposed { get; private set; }

    public event EventHandler? Completed;

    public event EventHandler? Failed;

    public event EventHandler? Disposed;

    public UnitOfWorkOptions? Options { get; private set; }

    protected UnitOfWork()
    {
        CorrelationId = Guid.NewGuid();
    }

    public IUnitOfWork Begin(UnitOfWorkOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (_started)
            throw new InvalidOperationException("The Uow has already started.");

        Options = options;

        _started = true;
        return this;
    }

    public async Task CommitAsync(CancellationToken cancellation = default)
    {
        if (_committed)
            throw new InvalidOperationException("The Uow has already been committed.");

        try
        {
            await SaveChangesAsync(cancellation);
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

    protected abstract Task SaveChangesAsync(CancellationToken cancellation = default);
}