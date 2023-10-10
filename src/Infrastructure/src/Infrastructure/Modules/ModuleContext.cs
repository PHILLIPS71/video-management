using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Giantnodes.Infrastructure.Modules;

internal class ModuleContext : IModuleContext
{
    private readonly IServiceCollection _collection;

    internal ModuleContext(IServiceCollection collection)
    {
        ArgumentNullException.ThrowIfNull(collection);

        _collection = collection;
    }

    public int Count => _collection.Count;

    public bool IsReadOnly => _collection.IsReadOnly;

    public IEnumerator<ServiceDescriptor> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_collection).GetEnumerator();
    }

    public void Add(ServiceDescriptor item)
    {
        _collection.Add(item);
    }

    public void Clear()
    {
        _collection.Clear();
    }

    public bool Contains(ServiceDescriptor item)
    {
        return _collection.Contains(item);
    }

    public void CopyTo(ServiceDescriptor[] array, int index)
    {
        _collection.CopyTo(array, index);
    }

    public bool Remove(ServiceDescriptor item)
    {
        return _collection.Remove(item);
    }

    public int IndexOf(ServiceDescriptor item)
    {
        return _collection.IndexOf(item);
    }

    public void Insert(int index, ServiceDescriptor item)
    {
        _collection.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        _collection.RemoveAt(index);
    }

    public ServiceDescriptor this[int index]
    {
        get => _collection[index];
        set => _collection[index] = value;
    }
}