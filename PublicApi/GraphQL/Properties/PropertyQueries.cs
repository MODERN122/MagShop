using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using HotChocolate.Types;

namespace PublicApi.GraphQL.Properties
{
    [ExtendObjectType(typeof(Query))]
    public class PropertyQueries
    {
        private readonly IAsyncRepository<Property> _propertiesRepository;

        public PropertyQueries(
            IAsyncRepository<Property> propertiesRepository)
        {
            _propertiesRepository = propertiesRepository;
        }


        public async Task<IEnumerable<Property>> GetProperties()
        {
            var spec = new PropertySpecification();
            return await _propertiesRepository.ListAsync(spec);
        }
    }
}
