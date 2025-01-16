namespace EventSourcingExample.Application.Common.Models
{
    public class VersionableDataModel
    {
        public int Version { get; set; }
        public string Identifier { get; }

        public VersionableDataModel(int version, string identifier)
        {
            Version = version;
            Identifier = identifier;
        }
    }
}
