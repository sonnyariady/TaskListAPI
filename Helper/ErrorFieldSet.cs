namespace TasklistAPI.Helper
{
    public class ErrorFieldSet
    {
        public List<ErrorFieldItem> ListErrors { get; set; } = new List<ErrorFieldItem>();

        public void AddError(string fieldcode,string message)
        {
            this.ListErrors.Add(new ErrorFieldItem { ErrorMessage = message, FieldCode = fieldcode });
        }

        public bool IsValid 
        { 
            get
            {
                return this.ListErrors.Count == 0;
            }
                
        }
    }
}
