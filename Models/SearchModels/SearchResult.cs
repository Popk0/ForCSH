using System.Collections.Generic;

namespace Models.SearchModels
{
    public class SearchResult<TEntity> where TEntity : new()
    {
        public List<TEntity> Entities { get; set; }        
    }
}
