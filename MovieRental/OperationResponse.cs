using System.Collections.Generic;

namespace MovieRental
{
    public class OperationResponse
    {
        public IDictionary<string, string> Errors { get; private set; }

        public OperationResponse()
        {
            Errors = new Dictionary<string, string>();
        }

        public bool Success
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        public OperationResponse AddError(string key, string message)
        {
            Errors.Add(key, message);

            return this;
        }
    }
}
