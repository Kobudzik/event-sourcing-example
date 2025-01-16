using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace EventSourcingExample.Application.Common.Models
{
    public class Pager
    {
        #region Pager()
        public Pager() : this(1, 20, null, null)
        {
        }

        public Pager(int pageIndex, int pageSize = 20, string sort = null, string order = null)
        {
            TotalRows = Int32.MaxValue;
            Index = pageIndex;
            Size = pageSize;
            SortBy = sort;
            Order = order;
        }
        #endregion

        private int index;
        private int size;
        private string order;
        private int totalRows;

        #region Index
        /// <summary>
        /// Number of page
        /// </summary>
        public virtual int Index
        {
            get
            {
                if (index < TotalPages)
                    return index;
                else if (TotalPages > 0)
                    return TotalPages;
                else
                    return 1;
            }

            set => index = value > 0 ? value : 1;
        }
        #endregion

        #region Size
        /// <summary>
        /// Page size
        /// </summary>
        public int Size
        {
            get => size;

            set => size = value > 0 ? value : 1;
        }
        #endregion

        public string SortBy { get; set; }

        #region Order
        public string Order
        {
            get => order;
            set => order = value?.ToUpper() == "ASC" ? "ASC" : "DESC";
        }
        #endregion

        #region TotalRows
        public int TotalRows
        {
            get => totalRows;
            set => totalRows = value > 0 ? value : 0;
        }
        #endregion

        #region TotalPages
        public int TotalPages
        {
            get
            {
                if (Size > 0)
                    return Convert.ToInt32(Math.Ceiling((double)TotalRows / Size));
                else
                    return 0;
            }
        }
        #endregion

        #region Offset
        /// <summary>
        /// How many items to skip
        /// </summary>
        public int Offset
        {
            get => (Index - 1) * Size;
        }
        #endregion
    }

    public static class PagerExtensions
    {
        #region Paginate()
        /// <summary>
        /// Sets pager's total rows;
        /// sorts data;
        /// skips offset;
        /// takes size;
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="query"></param>
        /// <param name="pager"></param>
        /// <returns>IQueryable<TModel> </returns>
        public static IQueryable<TModel> Paginate<TModel>(this IQueryable<TModel> query, Pager pager) where TModel : class
        {
            // Count
            pager.TotalRows = query.Count();

            // Sort
            if (!string.IsNullOrEmpty(pager.SortBy))
            {
                var orderBy = $"{pager.SortBy} {pager.Order}";

                if (OrderingMethodFinder.OrderMethodExists(query.Expression))
                    query = (query as IOrderedQueryable<TModel>)?.ThenBy(orderBy);
                else
                    query = query.OrderBy(orderBy);
            }

            // Paginate
            return query
                .Skip(pager.Offset)
                .Take(pager.Size);
        }
        #endregion
    }

    internal class OrderingMethodFinder : ExpressionVisitor
    {
        private bool orderingMethodFound;

        #region VisitMethodCall()
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var name = node.Method.Name;

            var startsWithOrderByOrThenBy = name.StartsWith("OrderBy", StringComparison.Ordinal) ||
                name.StartsWith("ThenBy", StringComparison.Ordinal);

            if (node.Method.DeclaringType == typeof(Queryable) && startsWithOrderByOrThenBy)
                orderingMethodFound = true;

            return base.VisitMethodCall(node);
        }
        #endregion

        #region OrderMethodExists()
        public static bool OrderMethodExists(Expression expression)
        {
            var visitor = new OrderingMethodFinder();

            visitor.Visit(expression);

            return visitor.orderingMethodFound;
        }
        #endregion
    }
}
