namespace Hospital_Management.Helper
{
    public class JWT
    {
        public string SecretKey { get; set; }
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public double DurationInDays { get; set; }
    }
}
