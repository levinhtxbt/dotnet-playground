using System;
using System.Collections.Generic;
using System.Text;

namespace M1GetListBuilder.ListBuilder
{
    public class ListBuilderGroup
    {
        public string Name { get; set; }

        public List<ListBuilderItem> Items { get; set; }
            = new List<ListBuilderItem>();
    }

    public class ListBuilderItem
    {
        public string Name { get; set; }
    }
}
