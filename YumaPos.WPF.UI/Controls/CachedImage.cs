/*
 The MIT License (MIT)

Copyright (c) 2014 Haridas Pachuveetil

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

From https://github.com/floydpink/CachedImage

 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace YumaPos.WPF.UI.Controls
{
    /// <summary>
    ///     Represents a control that is a wrapper on System.Windows.Controls.Image for enabling filesystem-based caching
    /// </summary>
    public class CachedImage : System.Windows.Controls.Image
    {
        static CachedImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CachedImage),
                new FrameworkPropertyMetadata(typeof(CachedImage)));
        }

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        public BitmapCreateOptions CreateOptions
        {
            get { return ((BitmapCreateOptions)(base.GetValue(CachedImage.CreateOptionsProperty))); }
            set { base.SetValue(CachedImage.CreateOptionsProperty, value); }
        }

        private static async void ImageUrlPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var url = e.NewValue as string;

            if (string.IsNullOrEmpty(url))
                return;

            var cachedImage = (CachedImage)obj;
            var bitmapImage = new BitmapImage();

            switch (FileCache.AppCacheMode)
            {
                case FileCache.CacheMode.WinINet:
                    bitmapImage.BeginInit();
                    bitmapImage.CreateOptions = cachedImage.CreateOptions;
                    bitmapImage.UriSource = new Uri(url);
                    // Enable IE-like cache policy.
                    bitmapImage.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.Default);
                    bitmapImage.EndInit();
                    cachedImage.Source = bitmapImage;
                    break;

                case FileCache.CacheMode.Dedicated:
                    try
                    {
                        var memoryStream = await FileCache.HitAsync(url);
                        if (memoryStream == null)
                            return;

                        bitmapImage.BeginInit();
                        bitmapImage.CreateOptions = cachedImage.CreateOptions;
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();
                        cachedImage.Source = bitmapImage;
                    }
                    catch (Exception)
                    {
                        // ignored, in case the downloaded file is a broken or not an image.
                    }
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl",
            typeof(string), typeof(CachedImage), new PropertyMetadata("", ImageUrlPropertyChanged));

        public static readonly DependencyProperty CreateOptionsProperty = DependencyProperty.Register("CreateOptions",
            typeof(BitmapCreateOptions), typeof(CachedImage));
    }

    public static class FileCache
    {
        public enum CacheMode
        {
            WinINet,
            Dedicated
        }

        // Record whether a file is being written.
        private static readonly Dictionary<string, bool> IsWritingFile = new Dictionary<string, bool>();

        static FileCache()
        {
            // default cache directory - can be changed if needed from App.xaml
            AppCacheDirectory = string.Format("{0}\\{1}\\Cache\\",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Process.GetCurrentProcess().ProcessName);
            AppCacheMode = CacheMode.WinINet;
        }

        /// <summary>
        ///     Gets or sets the path to the folder that stores the cache file. Only works when AppCacheMode is
        ///     CacheMode.Dedicated.
        /// </summary>
        public static string AppCacheDirectory { get; set; }

        /// <summary>
        ///     Gets or sets the cache mode. WinINet is recommended, it's provided by .Net Framework and uses the Temporary Files
        ///     of IE and the same cache policy of IE.
        /// </summary>
        public static CacheMode AppCacheMode { get; set; }

        public static async Task<MemoryStream> HitAsync(string url)
        {
            if (!Directory.Exists(AppCacheDirectory))
            {
                Directory.CreateDirectory(AppCacheDirectory);
            }
            var uri = new Uri(url);
            var fileNameBuilder = new StringBuilder();
            using (var sha1 = new SHA1Managed())
            {
                var canonicalUrl = uri.ToString();
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(canonicalUrl));
                fileNameBuilder.Append(BitConverter.ToString(hash).Replace("-", "").ToLower());
                if (Path.HasExtension(canonicalUrl))
                    fileNameBuilder.Append(Path.GetExtension(canonicalUrl));
            }

            var fileName = fileNameBuilder.ToString();
            var localFile = string.Format("{0}\\{1}", AppCacheDirectory, fileName);
            var memoryStream = new MemoryStream();

            FileStream fileStream = null;
            if (!IsWritingFile.ContainsKey(fileName) && File.Exists(localFile))
            {
                using (fileStream = new FileStream(localFile, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }

            var request = WebRequest.Create(uri);
            request.Timeout = 30;
            try
            {
                var response = await request.GetResponseAsync();
                var responseStream = response.GetResponseStream();
                if (responseStream == null)
                    return null;
                if (!IsWritingFile.ContainsKey(fileName))
                {
                    IsWritingFile[fileName] = true;
                    fileStream = new FileStream(localFile, FileMode.Create, FileAccess.Write);
                }

                using (responseStream)
                {
                    var bytebuffer = new byte[100];
                    int bytesRead;
                    do
                    {
                        bytesRead = await responseStream.ReadAsync(bytebuffer, 0, 100);
                        if (fileStream != null)
                            await fileStream.WriteAsync(bytebuffer, 0, bytesRead);
                        await memoryStream.WriteAsync(bytebuffer, 0, bytesRead);
                    } while (bytesRead > 0);
                    if (fileStream != null)
                    {
                        await fileStream.FlushAsync();
                        fileStream.Dispose();
                        IsWritingFile.Remove(fileName);
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
            catch (WebException)
            {
                return null;
            }
        }
    }
}
