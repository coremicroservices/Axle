namespace Booking.Helper
{
    public class FileDetail
    {
        public string FileId { get; set; }
        public string OriginalFileName { get; set; }
        public string StoredFileName { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeKB { get; set; }
        public string ContentType { get; set; }
    }
}
