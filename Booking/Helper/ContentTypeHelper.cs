namespace Booking.Helper
{
    public static class ContentTypeHelper
    {
        public static readonly Dictionary<string, string> MimeTypes = new(StringComparer.OrdinalIgnoreCase)
{
    // Images
    { ".jpg", "image/jpeg" },
    { ".jpeg", "image/jpeg" },
    { ".png", "image/png" },
    { ".gif", "image/gif" },
    { ".bmp", "image/bmp" },
    { ".webp", "image/webp" },
    { ".svg", "image/svg+xml" },
    { ".ico", "image/x-icon" },

    // Documents
    { ".pdf", "application/pdf" },
    { ".doc", "application/msword" },
    { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
    { ".xls", "application/vnd.ms-excel" },
    { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
    { ".ppt", "application/vnd.ms-powerpoint" },
    { ".pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
    { ".txt", "text/plain" },
    { ".rtf", "application/rtf" },
    { ".csv", "text/csv" },
    { ".json", "application/json" },
    { ".xml", "application/xml" },
    { ".html", "text/html" },
    { ".htm", "text/html" },

    // Audio
    { ".mp3", "audio/mpeg" },
    { ".wav", "audio/wav" },
    { ".ogg", "audio/ogg" },
    { ".m4a", "audio/mp4" },
    { ".flac", "audio/flac" },

    // Video
    { ".mp4", "video/mp4" },
    { ".avi", "video/x-msvideo" },
    { ".mov", "video/quicktime" },
    { ".wmv", "video/x-ms-wmv" },
    { ".webm", "video/webm" },
    { ".mkv", "video/x-matroska" },

    // Archives
    { ".zip", "application/zip" },
    { ".rar", "application/vnd.rar" },
    { ".7z", "application/x-7z-compressed" },
    { ".tar", "application/x-tar" },
    { ".gz", "application/gzip" },

    // Code
    { ".js", "application/javascript" },
    { ".css", "text/css" },
    { ".cs", "text/plain" },
    { ".java", "text/plain" },
    { ".py", "text/plain" },
    { ".cpp", "text/plain" },
    { ".c", "text/plain" },
    { ".ts", "application/typescript" },

    // Fonts
    { ".ttf", "font/ttf" },
    { ".otf", "font/otf" },
    { ".woff", "font/woff" },
    { ".woff2", "font/woff2" },

    // Others
    { ".exe", "application/octet-stream" },
    { ".dll", "application/octet-stream" },
    { ".apk", "application/vnd.android.package-archive" },
    { ".msi", "application/x-msdownload" }
};
    }
}
