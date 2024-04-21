namespace BHL_url_server.HelpClasses;

public class ThreatEntry
{
    public string url { get; set; }
}

public class ThreatInfo
{
    public List<string> threatTypes { get; set; }
    public List<string> platformTypes { get; set; }
    public List<string> threatEntryTypes { get; set; }
    public List<ThreatEntry> threatEntries { get; set; }
}

public class Client
{
    public string clientId { get; set; }
    public string clientVersion { get; set; }
}

public class Request
{
    public Client client { get; set; }
    public ThreatInfo threatInfo { get; set; }
}