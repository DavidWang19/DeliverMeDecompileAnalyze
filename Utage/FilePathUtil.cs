namespace Utage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class FilePathUtil
    {
        public static string AddDoubleExtension(string path, string doubleExtension)
        {
            if (!CheckExtension(path, doubleExtension))
            {
                path = path + doubleExtension;
            }
            return path;
        }

        public static string AddFileProtocol(string path)
        {
            if (path.Contains("://"))
            {
                return path;
            }
            if (path[0] != '/')
            {
                path = '/' + path;
            }
            return ("file://" + path);
        }

        public static string ChangeExtension(string path, string extenstion)
        {
            return Path.ChangeExtension(path, extenstion);
        }

        public static bool CheckExtension(string path, string ext)
        {
            return (string.Compare(GetExtension(path), ext, true) == 0);
        }

        public static bool CheckExtensionWithOutDouble(string path, string ext, string doubleExtension)
        {
            return CheckExtension(GetExtensionWithOutDouble(path, doubleExtension), ext);
        }

        public static string Combine(params string[] args)
        {
            string str = string.Empty;
            foreach (string str2 in args)
            {
                if (!string.IsNullOrEmpty(str2))
                {
                    str = Path.Combine(str, str2);
                }
            }
            return str.Replace(@"\", "/");
        }

        public static string EncodeUrl(string url)
        {
            try
            {
                Uri uri = new Uri(url.Replace('\\', '/'));
                return uri.AbsoluteUri;
            }
            catch (Exception exception)
            {
                Debug.LogError(url + ":" + exception.Message);
                return url;
            }
        }

        public static string Format(string path)
        {
            path = path.Replace(@"\", "/");
            if (!path.Contains("://"))
            {
                path = path.Replace(":/", "://");
            }
            return path;
        }

        public static string GetDirectoryNameOnly(string path)
        {
            return Path.GetFileName(Path.GetDirectoryName(path));
        }

        public static string GetDirectoryPath(string path)
        {
            int length = Mathf.Max(path.LastIndexOf('/'), path.LastIndexOf('\\'));
            if (length >= 0)
            {
                return path.Substring(0, length);
            }
            if (path.IndexOf('.') >= 0)
            {
                return string.Empty;
            }
            return path;
        }

        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public static string GetExtensionWithOutDouble(string path, string doubleExtension)
        {
            string extension = Path.GetExtension(path);
            if (string.Compare(extension, doubleExtension, true) != 0)
            {
                return extension;
            }
            path = path.Substring(0, path.Length - doubleExtension.Length);
            return Path.GetExtension(path);
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public static string GetFileNameWithoutDoubleExtension(string path)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            if (fileNameWithoutExtension.Contains("."))
            {
                fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileNameWithoutExtension);
            }
            return fileNameWithoutExtension;
        }

        public static string GetFileNameWithoutExtension(string path)
        {
            try
            {
                return Path.GetFileNameWithoutExtension(path);
            }
            catch (Exception exception)
            {
                Debug.LogError(path + "  " + exception.Message);
                return string.Empty;
            }
        }

        public static string GetPathWithoutExtension(string path)
        {
            int length = path.LastIndexOf('.');
            if (length > 0)
            {
                path = path.Substring(0, length);
            }
            return path;
        }

        public static bool IsAbsoluteUri(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            if (path.Length <= 1)
            {
                return false;
            }
            try
            {
                Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
                return uri.IsAbsoluteUri;
            }
            catch (Exception exception)
            {
                Debug.LogError(path + ":" + exception.Message);
                return false;
            }
        }

        internal static bool IsUnderDirectory(string path, string directoryPath)
        {
            path = Format(path);
            directoryPath = Format(directoryPath);
            return path.StartsWith(directoryPath);
        }

        public static string RemoveDirectory(string path, string directoryPath)
        {
            string str;
            path = Format(path);
            directoryPath = Format(directoryPath);
            if (!TryRemoveDirectory(path, directoryPath, out str))
            {
                Debug.LogError("RemoveDirectoryPath Error [" + path + "]  [" + directoryPath + "] ");
            }
            return str;
        }

        public static string ToCacheClearUrl(string url)
        {
            return string.Format("{0}?datetime={1}", url, DateTime.Now.ToFileTime());
        }

        public static string ToRelativePath(string root, string path)
        {
            Uri uri = new Uri(root);
            Uri uri2 = new Uri(path);
            return uri.MakeRelativeUri(uri2).ToString();
        }

        public static string ToStreamingAssetsPath(string path)
        {
            string[] args = new string[] { Application.get_streamingAssetsPath(), path };
            return AddFileProtocol(Combine(args));
        }

        public static bool TryRemoveDirectory(string path, string directoryPath, out string newPath)
        {
            newPath = path;
            if (!path.StartsWith(directoryPath))
            {
                return false;
            }
            int length = directoryPath.Length;
            if (path.Length > length)
            {
                switch (path[length])
                {
                    case '/':
                    case '\\':
                        length++;
                        break;
                }
            }
            newPath = path.Remove(0, length);
            return true;
        }
    }
}

