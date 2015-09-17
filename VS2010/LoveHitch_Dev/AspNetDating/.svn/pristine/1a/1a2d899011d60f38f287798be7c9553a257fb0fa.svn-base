using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using AspNetDating.Plugins.Interfaces;

namespace AspNetDating.Classes
{
    public static class Plugins
    {
        public static List<Type> Types = new List<Type>();
        public static List<IPlugin> Instances = new List<IPlugin>();
        public static PluginContext Context = new PluginContext();
        public static PageEvents Events = new PageEvents();

        public static void InitializePlugins()
        {
            Context.Application = HttpContext.Current.ApplicationInstance;
            Context.PageEvents = Events;
            Context.ConnectionString = Config.DB.ConnectionString;

            try
            {
                foreach (string dir in Directory.GetDirectories(HttpContext.Current.Server.MapPath("~/Plugins")))
                {
                    foreach (string dllFile in Directory.GetFiles(dir, "*.dll"))
                    {
                        try
                        {
                            TryLoadingPlugin(dllFile);
                        }
                        catch (Exception err)
                        {
                            Global.Logger.LogWarning(err);
                            continue;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogWarning(err);
            }
        }

        private static void TryLoadingPlugin(string path)
        {
            AssemblyName an;
            try
            {
                an = AssemblyName.GetAssemblyName(path);
            }
            catch (BadImageFormatException)
            {
                return;
            }
            Assembly asm = AppDomain.CurrentDomain.Load(an);
            foreach (Type t in asm.GetTypes())
            {
                foreach (Type iface in t.GetInterfaces())
                {
                    if (iface.Equals(typeof (IPlugin)))
                    {
                        Types.Add(t);
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(t);
                        plugin.Initialize(Context);
                        Global.Logger.LogStatus("InitializePlugins", "Plugin " + plugin.Name + " initialized.");
                        Instances.Add(plugin);
                        break;
                    }
                }
            }
        }
    }

    public class PageEvents : IPageEvents
    {
        public event EventHandler OnPreInit;
        public void OnPreInitInvoke(object page, EventArgs e)
        {
            if (OnPreInit != null)
                OnPreInit(page, e);
        }

        public event EventHandler OnInit;
        public void OnInitInvoke(object page, EventArgs e)
        {
            if (OnInit != null)
                OnInit(page, e);
        }

        public event EventHandler OnInitComplete;
        public void OnInitCompleteInvoke(object page, EventArgs e)
        {
            if (OnInitComplete != null)
                OnInitComplete(page, e);
        }

        public event EventHandler OnPreLoad;
        public void OnPreLoadInvoke(object page, EventArgs e)
        {
            if (OnPreLoad != null)
                OnPreLoad(page, e);
        }

        public event EventHandler OnLoad;
        public void OnLoadInvoke(object page, EventArgs e)
        {
            if (OnLoad != null)
                OnLoad(page, e);
        }

        public event EventHandler OnLoadComplete;
        public void OnLoadCompleteInvoke(object page, EventArgs e)
        {
            if (OnLoadComplete != null)
                OnLoadComplete(page, e);
        }

        public event EventHandler OnPreRender;
        public void OnPreRenderInvoke(object page, EventArgs e)
        {
            if (OnPreRender != null)
                OnPreRender(page, e);
        }

        public event EventHandler OnPreRenderComplete;
        public void OnPreRenderCompleteInvoke(object page, EventArgs e)
        {
            if (OnPreRenderComplete != null)
                OnPreRenderComplete(page, e);
        }
    }

    public static class FaceFinderPlugin
    {
        public static bool IsInstalled
        {
            get
            {
                foreach (IPlugin plugin in Plugins.Instances)
                {
                    if (plugin.Name == "FaceFinder")
                        return true;
                }
                return false;
            }
        }

        public class FaceRegion
        {
            private int x;
            private int y;
            private int width;
            private int height;

            public int X
            {
                get { return x; }
                set { x = value; }
            }

            public int Y
            {
                get { return y; }
                set { y = value; }
            }

            public int Width
            {
                get { return width; }
                set { width = value; }
            }

            public int Height
            {
                get { return height; }
                set { height = value; }
            }
        }

        public static FaceRegion[] FindFaces(byte[] image)
        {
            IPlugin facefinder = null;
            foreach (IPlugin plugin in Plugins.Instances)
            {
                if (plugin.Name == "FaceFinder")
                {
                    facefinder = plugin;
                    break;
                }
            }
            if (facefinder == null) return null;

            Type faceFinderType = facefinder.GetType();
            object results = faceFinderType.InvokeMember("FindFaces", BindingFlags.Default | BindingFlags.InvokeMethod, 
                null, facefinder, new object[] {image});

            if (results == null)
            {
                Global.Logger.LogStatus("Plugins::FindFaces", "Application Pool is restarting...");
                ApplicationPoolRecycle.RecycleCurrentApplicationPool();
                return new FaceRegion[0];
            }

            List<FaceRegion> regions = new List<FaceRegion>();
            foreach (object result in (IEnumerable) results)
            {
                Type faceRegionType = result.GetType();
                FaceRegion region = new FaceRegion();
                region.X = (int) faceRegionType.GetField("X").GetValue(result);
                region.Y = (int) faceRegionType.GetField("Y").GetValue(result);
                region.Width = (int) faceRegionType.GetField("Width").GetValue(result);
                region.Height = (int) faceRegionType.GetField("Height").GetValue(result);
                regions.Add(region);
            }
            return regions.ToArray();
        }
    }

    public static class VideoConverterPlugin
    {
        public static bool IsInstalled
        {
            get
            {
                foreach (IPlugin plugin in Plugins.Instances)
                {
                    if (plugin.Name == "Video Converter")
                        return true;
                }
                return false;
            }
        }

        public static void ConvertVideo(string sourceFile, string targetFile)
        {
            IPlugin videoConverter = null;
            foreach (IPlugin plugin in Plugins.Instances)
            {
                if (plugin.Name == "Video Converter")
                {
                    videoConverter = plugin;
                    break;
                }
            }
            if (videoConverter == null) return;

            Type faceFinderType = videoConverter.GetType();
            string ffmpegPath = Config.Directories.Home + @"\Plugins\VideoConverter\ffmpeg.exe";
            try
            {
                DateTime dtConvertStart = DateTime.Now;
                var size = "320x240";
                var thumbSize = "130x98";
                try
                {
                    var originalSize = faceFinderType.InvokeMember("GetMovieSize",
                                                                   BindingFlags.Default | BindingFlags.InvokeMethod,
                                                                   null, videoConverter, new object[]
                                                                                         {
                                                                                             sourceFile, ffmpegPath
                                                                                         }) as int[];

                    if (originalSize != null && originalSize.Length > 0 && originalSize[0] > 0)
                    {
                        // Calculate new dimentions
                        var newHeight = (int)(320 / (originalSize[0] / (decimal)originalSize[1]));
                        if (newHeight % 8 != 0)
                        {
                            if ((newHeight & 4) == 4)
                                newHeight = ((newHeight >> 3) + 1) << 3;
                            else
                                newHeight = ((newHeight >> 3)) << 3;
                        }
                        size = "320x" + newHeight;

                        thumbSize = "130x" + ((int) (130/(originalSize[0]/(decimal) originalSize[1])));
                    }
                }
                catch (Exception err)
                {
                    Global.Logger.LogError("Unable to get movie size!", err);
                }
                faceFinderType.InvokeMember("ConvertToFLV", BindingFlags.Default | BindingFlags.InvokeMethod,
                                            null, videoConverter, new object[]
                                                                      {
                                                                          sourceFile, targetFile, ffmpegPath,
                                                                          size, 360, 25, 32, 22050, true
                                                                      });
                faceFinderType.InvokeMember("CaptureFrame", BindingFlags.Default | BindingFlags.InvokeMethod,
                                            null, videoConverter, new object[]
                                                                      {
                                                                          sourceFile, targetFile.Replace(".flv", ".png"), 
                                                                          ffmpegPath, thumbSize
                                                                      });
                Global.Logger.LogStatus("ConvertVideo", "Video file " + sourceFile + " converted finished ("
                    + DateTime.Now.Subtract(dtConvertStart) + ")");
            }
            catch (Exception err)
            {
                Global.Logger.LogError("ConvertVideo", err);
            }
        }
    }

    public static class VideoStreamerPlugin
    {
        public static bool IsInstalled
        {
            get
            {
                foreach (IPlugin plugin in Plugins.Instances)
                {
                    if (plugin.Name == "Video Streamer" || plugin.Name == "Video Broadcast")
                        return true;
                }
                return false;
            }
        }
    }
}