using System.Collections.Generic;
namespace CoreLibrary.Cores
{
    public class CountModel<T>
    {
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; }
        
    }
}
