using System.ComponentModel.DataAnnotations;

namespace EntityLayer.Concrete;

public class Logs
{
    public string? ComputerName { get; set; }

    public string? Exception { get; set; }

    [Key] public int Id { get; set; }

    public string? IpAddress { get; set; }
    public string? Level { get; set; }
    public string? Message { get; set; }
    public string? MessageTemplate { get; set; }
    public string? Properties { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? UserDomainNamePC { get; set; }
    public string? Username { get; set; }

    public string? UserNamePC { get; set; }
}