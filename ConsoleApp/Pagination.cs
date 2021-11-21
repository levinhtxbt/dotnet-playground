using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ConsoleApp;

public delegate ItemPosition FindItemPositionHandler(int itemZeroIndex);

public class PagingDescriptor
{
    public int ActualPageSize { get; private set; }
    public int NumberOfPages { get; private set; }
    public PageBoundary[] PagesBoundaries { get; private set; }
    public FindItemPositionHandler Find { get; private set; }

    public PagingDescriptor(
    int actualPageSize,
    int numberOfPages,
    PageBoundary[] pagesBoundaries,
    FindItemPositionHandler find)
    {
        ActualPageSize = actualPageSize;
        NumberOfPages = numberOfPages;
        PagesBoundaries = pagesBoundaries;
        Find = find;
    }

    // This code is added for demonstration purposes only.
    // public override string ToString()
    // {
    //     var builder = new StringBuilder();
    //     // builder.AppendLine($"ActualPageZeroIndex: {ActualPageZeroIndex}");
    //     // builder.AppendLine("");
    //     // builder.AppendLine($"PagingDescriptor:");
    //     // builder.AppendLine($"----------------");
    //     // builder.AppendLine(PagingDescriptor.ToString());
    //     // builder.AppendLine($"Results:");
    //     // builder.AppendLine($"----------------");

    //     // foreach (var entity in Results)
    //     // {
    //     //     builder.Append(entity.ToString());
    //     // }

    //     return builder.ToString();
    // }
}

public class PageBoundary
{
    public int FirstItemIndex { get; private set; }
    public int LastItemIndex { get; private set; }

    public PageBoundary(int firstItemIndex, int lastItemIndex)
    {
        FirstItemIndex = firstItemIndex;
        LastItemIndex = lastItemIndex;
    }
}

public class ItemPosition
{
    public int PageIndex { get; private set; }
    public int ItemIndexInsidePage { get; private set; }

    public ItemPosition(int pageIndex, int itemIndexInsidePage)
    {
        PageIndex = pageIndex;
        ItemIndexInsidePage = itemIndexInsidePage;
    }
}

public static class ListExtensionMethods
{
    public static PagingDescriptor Page(this IList list, int pageSize)
    {
        var actualPageSize = pageSize;

        if (actualPageSize <= 0)
        {
            actualPageSize = list.Count;
        }

        var maxNumberOfPages = (int)Math.Round(Math.Max(1, Math.Ceiling(((float)list.Count) / ((float)actualPageSize))));

        return new PagingDescriptor(
            actualPageSize,
            maxNumberOfPages,
            Enumerable
                .Range(0, maxNumberOfPages)
                .Select(pageIndex => new PageBoundary(
                    pageIndex * actualPageSize,
                    Math.Min((pageIndex * actualPageSize) + (actualPageSize - 1), list.Count - 1)
                )).ToArray(),
            (itemIndex) => new ItemPosition(
                itemIndex / actualPageSize,
                itemIndex % actualPageSize
            )
        );
    }
}
