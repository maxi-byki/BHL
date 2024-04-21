namespace BHL_url_server.HelpClasses;


public class Elements
{
    public string type { get; set; }
    public string user_id { get; set; }
}
    
public class RichTextSection
{
    public string type { get; set; }
    public List<Elements> elements { get; set; }
}
    
public class Block
{
    public string type { get; set; }
    public string block_id { get; set; }
    public List<RichTextSection> elements { get; set; }
}
    
public class MyEvent
{
    public string user { get; set; }
    public string type { get; set; }
    public string ts { get; set; }
    public string client_msg_id { get; set; }
    public string text { get; set; }
    public string team { get; set; }
    public List<Block> blocks { get; set; }
    public string channel { get; set; }
    public string event_ts { get; set; }
}
    
public class Authorizations
{
    public object enterprise_id { get; set; }
    public string team_id { get; set; }
    public string user_id { get; set; }
    public bool is_bot { get; set; }
    public bool is_enterprise_install { get; set; }
}
    
public class Root
{
    public string token { get; set; }
    public string team_id { get; set; }
    public string api_app_id { get; set; }
    public MyEvent @event { get; set; }
    public string type { get; set; }
    public string event_id { get; set; }
    public int event_time { get; set; }
    public List<Authorizations> authorizations { get; set; }
    public bool is_ext_shared_channel { get; set; }
    public string event_context { get; set; }
}