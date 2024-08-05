using Refit;

namespace BFF.Models;
public class Episodes {
    public Info Info { get; set; }
    public IEnumerable<Episode> Results { get; set; }
}

