using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ezFixUpBaseAsyncHandler : IHttpAsyncHandler, IRequiresSessionState
    {
        protected string sessionId = null;

        public virtual bool IsReusable
        {
            get{return false;}
        }

        protected AsynchOperation ItsAsynchOperation;

        virtual public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            // set in derived handler
            //ItsAsynchOperation = new AsynchOperation(cb, context, extraData);
            sessionId = context.Session.SessionID;
            ItsAsynchOperation.StartAsyncWork();
            return ItsAsynchOperation;
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new InvalidOperationException();
        }

        virtual public void EndProcessRequest(IAsyncResult result)
        {
            // Cache this handler response for 1 second.
            //HttpCachePolicy c = ((AsynchOperation)result).Context.Response.Cache;
            //c.SetCacheability(HttpCacheability.Public);
            //c.SetMaxAge(new TimeSpan(0, 0, 1));
        }
    }

    public class AsynchOperation : IAsyncResult
    {
        private bool _completed;
        private Object _state;
        private AsyncCallback _callback;
        public HttpContext Context;

        bool IAsyncResult.IsCompleted { get { return _completed; } }
        WaitHandle IAsyncResult.AsyncWaitHandle { get { return null; } }
        Object IAsyncResult.AsyncState { get { return _state; } }
        bool IAsyncResult.CompletedSynchronously { get { return false; } }

        public AsynchOperation(AsyncCallback callback, HttpContext context, Object state)
        {
            _callback = callback;
            Context = context;
            _state = state;
            _completed = false;
        }

        public void StartAsyncWork()
        {
            ThreadPool.QueueUserWorkItem(StartAsyncTask, null);
        }

        virtual public void StartAsyncTask(Object workItemState)
        {
            _completed = true;
            _callback(this);
        }
    }
}
