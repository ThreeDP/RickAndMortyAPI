using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Refit;

namespace BFF.DTOs;

public class CharacterFilterDTO : PageFilterDTO
{
    [AliasAs("name")]
    public string?  Name { get; set; }
    [AliasAs("status")]
    public string?  Status { get; set; }
    [AliasAs("species")]
    public string?  Species { get; set; }
    [AliasAs("type")]
    public string?  Type {  get; set; }
    [AliasAs("gender")]
    public string?  Gender {  get; set; }
}
