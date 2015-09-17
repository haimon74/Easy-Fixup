using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ezFixUp.Classes.LoveHitchParallel
{
    public class SyncContextClass
    {
        public HttpContext Context = null;

        public HttpContext GetCurrent()
        {
            if (HttpContext.Current == null)
                HttpContext.Current = Context;
            return HttpContext.Current;
        }
    }
    public interface IMyParameterizedThreadExecuter
    {
        void Start(object parameter);
        void Join();
    }

    public class MyParameterizedThreadExecuter : IMyParameterizedThreadExecuter
    {
        private Thread _thread;
        private HttpContext _context;

        public MyParameterizedThreadExecuter(ParameterizedThreadStart doWorkFunc, bool isBackground, ThreadPriority priority, SyncContextClass _syncContext)
        {
            _thread = new Thread(doWorkFunc) { IsBackground = isBackground, Priority = priority };
            _context = _syncContext.GetCurrent();
        }
        public void Start(object parameter)
        {
            _thread.Start(parameter);
        }
        public void Join()
        {
            _thread.Join();
        }
    }
    public interface IWorkThreadClass
    {
        void DoWorkThread(object parameter);
    }
    public class WorkThreadClass : IWorkThreadClass
    {
        virtual public void DoWorkThread(object parameter)
        {
        }
    }
    public class ParameterizedThreadsList<T> //where T : WorkThreadClass
    {
        private List<T> _inList = null;
        private List<T> _outList = null;
        public List<T> OutList { get { return _outList; } set { _outList = value; } }
        private int _maxThreadsInPool = Config.DebugSettings.MaxParallelThreads;
        private ParameterizedThreadStart _doWorkFunc;
        private List<MyParameterizedThreadExecuter> _threadsList = null;
        public ThreadPriority RunAtPriority = ThreadPriority.AboveNormal;
        public bool RunAsBackgroundThreads = false;
        private SyncContextClass _syncContext;

        public ParameterizedThreadsList(List<T> inList, ParameterizedThreadStart doWorkFunc, HttpContext context)
        {
            _doWorkFunc = doWorkFunc;
            _inList = inList;
            _syncContext = new SyncContextClass { Context = context };
        }
        public ParameterizedThreadsList(List<T> inList, List<T> outList, ParameterizedThreadStart doWorkFunc, HttpContext context)
        {
            _doWorkFunc = doWorkFunc;
            _inList = inList;
            _outList = outList;
            _syncContext = new SyncContextClass { Context = context };
        }
        public void ExecuteParallelWork(int maxThreadsInPool)
        {
            if (_inList.Count() <= 0) return;

            _maxThreadsInPool = (maxThreadsInPool > 0) ? maxThreadsInPool : _maxThreadsInPool;
            _threadsList = new List<MyParameterizedThreadExecuter>();
            int maxItr = _inList.Count / _maxThreadsInPool;
            for (int idx = 0; idx < _inList.Count; idx++)
            {

                //ExecutionContext.Capture();
                if (_doWorkFunc == null && _inList[idx] is IWorkThreadClass)
                    _doWorkFunc = ((IWorkThreadClass)_inList[idx]).DoWorkThread;
                _threadsList.Add(new MyParameterizedThreadExecuter(_doWorkFunc, false, ThreadPriority.AboveNormal, _syncContext));
            }
            for (int itr = 0; itr <= maxItr; itr++)
            {
                for (int idx = 0; idx < _maxThreadsInPool; idx++)
                {
                    var arrIdx = (int)(itr * _maxThreadsInPool + idx);
                    if (arrIdx >= _threadsList.Count) continue;
                    _threadsList[arrIdx].Start(_inList[arrIdx]);
                }
                for (int idx = 0; idx < _maxThreadsInPool; idx++)
                {
                    var arrIdx = (int)(itr * _maxThreadsInPool + idx);
                    if (arrIdx >= _inList.Count) continue;
                    _threadsList[arrIdx].Join();
                }
            }
        }
    }
    public class ParameterizedThreadsList<TKey,TValue> //where T : WorkThreadClass
    {
        private List<TKey> _inList = null;
        private List<TValue> _outList = null;
        public List<TValue> OutList { get { return _outList; } set { _outList = value; } }
        private int _maxThreadsInPool = 5;
        private ParameterizedThreadStart _doWorkFunc;
        private List<MyParameterizedThreadExecuter> _threadsList = null;
        public ThreadPriority RunAtPriority = ThreadPriority.AboveNormal;
        public bool RunAsBackgroundThreads = false;
        private SyncContextClass _syncContext;

        public ParameterizedThreadsList(List<TKey> inList, List<TValue> outList, ParameterizedThreadStart doWorkFunc, HttpContext context)
        {
            _doWorkFunc = doWorkFunc;
            _inList = inList;
            _outList = outList;
            _syncContext = new SyncContextClass { Context = context };
        }
        public void ExecuteParallelWork(int maxThreadsInPool)
        {
            if (_inList.Count() <= 0) return;

            _maxThreadsInPool = (maxThreadsInPool > 0) ? maxThreadsInPool : _maxThreadsInPool;
            _threadsList = new List<MyParameterizedThreadExecuter>();
            int maxItr = _inList.Count / _maxThreadsInPool;
            for (int idx = 0; idx < _inList.Count; idx++)
            {

                //ExecutionContext.Capture();
                if (_doWorkFunc == null && _inList[idx] is IWorkThreadClass)
                    _doWorkFunc = ((IWorkThreadClass)_inList[idx]).DoWorkThread;
                _threadsList.Add(new MyParameterizedThreadExecuter(_doWorkFunc, false, ThreadPriority.AboveNormal, _syncContext));
            }
            for (int itr = 0; itr <= maxItr; itr++)
            {
                for (int idx = 0; idx < _maxThreadsInPool; idx++)
                {
                    var arrIdx = (int)(itr * _maxThreadsInPool + idx);
                    if (arrIdx >= _threadsList.Count) continue;
                    _threadsList[arrIdx].Start(_inList[arrIdx]);
                }
                for (int idx = 0; idx < _maxThreadsInPool; idx++)
                {
                    var arrIdx = (int)(itr * _maxThreadsInPool + idx);
                    if (arrIdx >= _inList.Count) continue;
                    _threadsList[arrIdx].Join();
                }
            }
        }
    }
    public class LoadUserResultThreadClass : IWorkThreadClass
    {
        public List<string> inKeys = new List<string>();
        public List<User> outValues = new List<User>();

        public LoadUserResultThreadClass(List<string> keys, List<User> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            string key = parameter as string;
            var user = User.Load(key);
            if (user != null)
                lock (outValues)
                {
                    outValues.Add(user);
                }
        }

        #endregion
    }
        
    public class LoadVideosResultThreadClass : IWorkThreadClass
    {
        public List<int> inKeys = new List<int>();
        public List<VideoUpload> outValues = new List<VideoUpload>();

        public LoadVideosResultThreadClass(List<int> keys, List<VideoUpload> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            var key = (int)parameter;
            var video = VideoUpload.Load(key);
            if (video != null)
                lock (outValues)
                {
                    outValues.Add(video);
                }
        }

        #endregion
    }
        
    public class LoadPhotosContestResultThreadClass : IWorkThreadClass
    {
        public List<int> inKeys = new List<int>();
        public List<PhotoContest> outValues = new List<PhotoContest>();

        public LoadPhotosContestResultThreadClass(List<int> keys, List<PhotoContest> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            var key = (int)parameter;
            var result = PhotoContest.Load(key);
            if (result != null)
                lock (outValues)
                {
                    outValues.Add(result);
                }
        }

        #endregion
    }
        
    public class LoadAbuseReportsResultThreadClass : IWorkThreadClass
    {
        public List<int> inKeys = new List<int>();
        public List<AbuseReport> outValues = new List<AbuseReport>();

        public LoadAbuseReportsResultThreadClass(List<int> keys, List<AbuseReport> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            var key = (int)parameter;
            var result = AbuseReport.Fetch(key);
            if (result != null)
                lock (outValues)
                {
                    outValues.Add(result);
                }
        }

        #endregion
    }
        
    public class LoadContactUsResultThreadClass : IWorkThreadClass
    {
        public List<int> inKeys = new List<int>();
        public List<ContactUs> outValues = new List<ContactUs>();

        public LoadContactUsResultThreadClass(List<int> keys, List<ContactUs> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            var key = (int)parameter;
            var result = ContactUs.Fetch(key);
            if (result != null)
                lock (outValues)
                {
                    outValues.Add(result);
                }
        }

        #endregion
    }
        
    public class LoadPhotosResultThreadClass : IWorkThreadClass
    {
        public List<int> inKeys = new List<int>();
        public List<Photo> outValues = new List<Photo>();

        public LoadPhotosResultThreadClass(List<int> keys, List<Photo> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            var key = (int)parameter;
            var result = Photo.Fetch(key);
            if (result != null)
                lock (outValues)
                {
                    outValues.Add(result);
                }
        }

        #endregion
    }
    public class LoadMessagesResultThreadClass : IWorkThreadClass
    {
        public List<int> inKeys = new List<int>();
        public List<Message> outValues = new List<Message>();

        public LoadMessagesResultThreadClass(List<int> keys, List<Message> values)
        {
            inKeys = keys;
            outValues = values;
        }
        #region IWorkThreadClass Members

        public virtual void DoWorkThread(object parameter)
        {
            var key = (int)parameter;
            var result = Message.Fetch(key);
            if (result != null)
                lock (outValues)
                {
                    outValues.Add(result);
                }
        }
        #endregion
    }
        
    public class AsyncWorkItem<T> where T : IWorkThreadClass
    {
        private ParameterizedThreadsList<T> _threadsList;
        public List<T> WorkThreadObjectsListResult;
        public HttpContext Context;

        public AsyncWorkItem(List<T> workThreadObjectsList, HttpContext context)
        {
            Context = context ?? HttpContext.Current;
            _threadsList =
                    new ParameterizedThreadsList<T>(workThreadObjectsList, null, Context);
        }

        public void ExecuteAsyncWork(int maxThreadsInPool)
        {
            if (_threadsList != null)
                _threadsList.ExecuteParallelWork(maxThreadsInPool);
        }
    }





    public class LoveHitchAsyncResult : IAsyncResult
        {

            #region IAsyncResult Imp.
            
            private AsyncCallback _cb;
            private object _state;
            private ManualResetEvent _event;
            private bool _completed = false;
            private object _lock = new object();

            public LoveHitchAsyncResult(AsyncCallback cb, object state)
            {
                _cb = cb;
                _state = state;
                ItsDelegate = new AsyncTaskDelegate(ExecuteAsyncTask);
            }
            public Object AsyncState { get { return _state; } }
            public bool CompletedSynchronously { get { return false; } }
            public bool IsCompleted { get { return _completed; } }

            public WaitHandle AsyncWaitHandle
            {
                get
                {
                    lock (_lock)
                    {
                        if (_event == null)
                            _event = new ManualResetEvent(IsCompleted);
                        return _event;
                    }
                }
            }
            public void CompleteCall()
            {
                lock (_lock)
                {
                    _completed = true;
                    if (_event != null) _event.Set();
                }

                if (_cb != null) _cb(this);
            }
            #endregion

            public AsyncTaskDelegate ItsDelegate;

            // Create delegate.
            public delegate void AsyncTaskDelegate();
            protected virtual void ExecuteAsyncTask()
            {
            }
        }

}