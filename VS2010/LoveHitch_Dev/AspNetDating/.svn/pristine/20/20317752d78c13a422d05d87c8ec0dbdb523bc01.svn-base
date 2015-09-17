using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace AspNetDating.Classes
{
    public class LoveHitchAsyncTask
    {
    }

    #region asyn task example

    public class GetRssAsyncTask : AsyncTask<XmlNodeList>
    {
        public string RssUrl { get; set; }

        public GetRssAsyncTask(string rssUrl, TaskFinished taskFinishedCall)
        {
            if ( String.IsNullOrEmpty(rssUrl) )
            {
                throw new ArgumentNullException(rssUrl);
            }

            RssUrl = rssUrl;
        }

        public override void Execute()
        {
            XmlDocument rssFeed = new XmlDocument();
            rssFeed.Load(RssUrl);

            Result = rssFeed.DocumentElement.SelectNodes("channel/item");
        }
    }
    public class RssViewerWebPart : System.Web.UI.WebControls.WebParts.WebPart
    {
        [WebBrowsable(true), WebDescription("Url of the RSS feed"), WebDisplayName("RSS url"), Personalizable(PersonalizationScope.Shared)]
        public string RssUrl { get; set; }

        GetRssAsyncTask getRssAsyncTask;
        Literal feedsLiteral;

        protected override void CreateChildControls()
        {
            if ( !String.IsNullOrEmpty(RssUrl) )
            {
                getRssAsyncTask = new GetRssAsyncTask(RssUrl, ShowRssFeeds);
                PageAsyncTask getRssPageAsyncTask =
                    new PageAsyncTask(getRssAsyncTask.OnBegin,
                                      getRssAsyncTask.OnEnd,
                                      getRssAsyncTask.OnTimeout,
                                      "GetRssAsyncTask", true);
                Page.RegisterAsyncTask(getRssPageAsyncTask);

                feedsLiteral = new Literal();
                Controls.Add(feedsLiteral);
            }
        }

        void ShowRssFeeds()
        {
            if ( getRssAsyncTask.Result != null && getRssAsyncTask.Result.Count > 0 )
            {
                StringBuilder feeds = new StringBuilder();

                foreach ( XmlNode post in getRssAsyncTask.Result )
                {
                    feeds.AppendFormat("<a href=\"{0}\">{1}</a><br />{2}", post.SelectSingleNode("link").InnerText, post.SelectSingleNode("title").InnerText, Environment.NewLine);
                }

                feedsLiteral.Text = feeds.ToString();
            }
        }
    }

    #endregion asyn task example

    public abstract class AsyncTask<TResult>
    {
        public TResult Result { get; protected set; }
        delegate void AsyncTaskDelegate();
        private AsyncTaskDelegate task;
        public delegate void TaskFinished();
        private TaskFinished taskFinished;

        public AsyncTask()
        {

        }

        public AsyncTask(TaskFinished taskFinishedHandler)
        {
            taskFinished = taskFinishedHandler;
        }

        public virtual IAsyncResult OnBegin(object sender, EventArgs e, AsyncCallback callback, object data)
        {
            task = new AsyncTaskDelegate(Execute);
            return task.BeginInvoke(callback, data);
        }

        public virtual void OnEnd(IAsyncResult result)
        {
            if ( taskFinished != null )
            {
                taskFinished.Invoke();
            }

            task.EndInvoke(result);
        }

        public virtual void OnTimeout(IAsyncResult result)
        {
            Result = default(TResult);
        }

        public abstract void Execute();
    }
}