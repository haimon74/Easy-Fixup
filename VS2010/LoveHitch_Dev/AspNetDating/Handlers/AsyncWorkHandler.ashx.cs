﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using AspNetDating.Classes;
using AspNetDating.Classes.LoveHitchParallel;

namespace AspNetDating.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class AsyncWorkHandler : LoveHitchBaseAsyncHandler
    {

        public override IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            ItsAsynchOperation = new AsynchOperation(cb, context, extraData);
            return base.BeginProcessRequest(context, cb, extraData);
        }
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            _context = ItsAsynchOperation.Context;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public class AsyncParallelWork : AsynchOperation
        {
            public AsyncParallelWork(AsyncCallback callback, HttpContext context, Object state) : 
                base(callback, context, state)
            {
            }
            override public void StartAsyncTask(Object workItemState)
            {
                var asyncWorkList = (List<AsyncWorkItem<IWorkThreadClass>>)Context.Session["AsyncWorkList"];
                foreach (AsyncWorkItem<IWorkThreadClass> workItem in asyncWorkList)
                {
                    workItem.ExecuteAsyncWork(5);
                }
                base.StartAsyncTask(workItemState);
            }
        }
    }
}
