using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Workflow.Core
{
    public class WorkflowContext
    {
        private readonly ConcurrentDictionary<string, object> data = new ConcurrentDictionary<string, object>();

        public CancellationToken CancellationToken { get; private set; } = CancellationToken.None;

        public WorkflowContext(CancellationToken cancellationToken)
        {
            this.CancellationToken = cancellationToken;
        }

        public T Get<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return data.TryGetValue(key, out var value) ? (T)value : default;
        }

        public void Set<T>(string key, T value)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            data[key] = value;
        }

        public void Remove(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            data.TryRemove(key, out _);
        }

        public bool ContainsKey(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return data.ContainsKey(key);
        }
    }
}