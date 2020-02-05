using System;
using System.Collections.Generic;
using System.Linq;

namespace Organizr.Domain.AggregateModel.ListAggregate
{
    public class MainListBase<TSubList, TItem>: ListBase<TItem> 
        where TSubList: ListBase<TItem> 
        where TItem : OrderedListItemBase
    {
        protected List<TSubList> _subLists;
        public IReadOnlyCollection<TSubList> SubLists => _subLists.AsReadOnly();

        protected MainListBase()
        {
            _subLists = new List<TSubList>();
        }

        protected TItem GetItemById(int itemId)
        {
            var todo = Items.SingleOrDefault(item => item.Id == itemId) ?? SubLists.SelectMany(subList => subList.Items)
                       .SingleOrDefault(item => item.Id == itemId);

            if (todo == null)
                // TODO: Add custom exception
                throw new Exception();

            return todo;
        }

        protected void RemoveItemById(int itemId)
        {

        }
    }
}
