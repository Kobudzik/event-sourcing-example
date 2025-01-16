namespace EventSourcingExample.Application.Common.Models
{
    public class VersionableCreatedResult
    {
        public int Version { get; set; }
        public string Identifier { get; }

        public VersionableCreatedResult(int version, string identifier)
        {
            Version = version;
            Identifier = identifier;
        }
    }
}
