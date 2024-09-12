namespace ContactInformationAPI.Modal
{
    public class APIResponse
    {
        public bool Status { get; set; }
        public string? StatusCode { get; set; }

        public string? Message { get; set; }

        public Object? Result { get; set; }

        public int? Id { get; set; }
    }
}
