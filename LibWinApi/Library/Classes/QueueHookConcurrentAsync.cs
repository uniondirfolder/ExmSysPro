using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace LibWinApi.Library.Classes
{
    internal class QueueHookConcurrentAsync<T>
    {
        ConcurrentQueue<T> _concurrentQueue = new ConcurrentQueue<T>();
        private TaskCompletionSource<bool> _dequeueTasks;
        private SemaphoreSlim _dequeueTaskLock=new SemaphoreSlim(1);
        private CancellationToken _taskCancellationToken;
        public QueueHookConcurrentAsync(CancellationToken taskCancellationToken)
        {
            _taskCancellationToken = taskCancellationToken;
        }
        internal void Enqueue(T value)
        {
            _concurrentQueue.Enqueue(value);
            _dequeueTaskLock.Wait();
            _dequeueTasks?.TrySetResult(true);
            _dequeueTaskLock.Release();
        }
        internal async Task<T> DequeueAsync()
        {
            T result;
            _concurrentQueue.TryDequeue(out result);

            if (result != null)
            {
                return result;
            }

            await _dequeueTaskLock.WaitAsync();
            _dequeueTasks = new TaskCompletionSource<bool>();
            _dequeueTaskLock.Release();

            _taskCancellationToken.Register(() => _dequeueTasks.TrySetCanceled());
            await _dequeueTasks.Task;

            _concurrentQueue.TryDequeue(out result);
            return result;
        }
    }
}