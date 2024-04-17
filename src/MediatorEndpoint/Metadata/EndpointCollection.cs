using System.Collections.Generic;
using System.Linq;

namespace MediatorEndpoint.Metadata;
internal class EndpointCollection(IEnumerable<Endpoint> endpoints) : List<Endpoint>(endpoints), IEndpointCollection
{
    private readonly Dictionary<string, Endpoint> _nameDictionary = endpoints.ToDictionary(x => x.Name.ToString());
    public Endpoint this[string name] => _nameDictionary[name];
    public bool Exist(string name) => !string.IsNullOrEmpty(name) && _nameDictionary.ContainsKey(name);
}
