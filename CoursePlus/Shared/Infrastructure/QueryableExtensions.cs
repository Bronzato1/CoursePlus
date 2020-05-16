using CoursePlus.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace CoursePlus.Shared.Infrastructure
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string orderByMember, string direction)
        {
            if (string.IsNullOrEmpty(orderByMember) || string.IsNullOrEmpty(direction))
                return query;

            var propertyNames = orderByMember.Split(".");
            var param = Expression.Parameter(typeof(T), string.Empty);
            var property = (Expression)param;

            foreach (var prop in propertyNames)
            {
                property = Expression.PropertyOrField(property, prop);
            }

            var lambda = Expression.Lambda(property, param);

            //var queryElementTypeParam = Expression.Parameter(typeof(T));
            //var memberAccess = Expression.PropertyOrField(queryElementTypeParam, orderByMember);
            //var keySelector = Expression.Lambda(memberAccess, queryElementTypeParam);

            var orderBy = Expression.Call(
                typeof(Queryable),
                direction.ToUpper() == "ASC" ? "OrderBy" : "OrderByDescending",
                new Type[] { typeof(T), property.Type },
                query.Expression,
                Expression.Quote(lambda));

            return query.Provider.CreateQuery<T>(orderBy);
        }
        
        public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> query, string filterMember, string filterValue)
        {
            if (string.IsNullOrEmpty(filterMember) || string.IsNullOrEmpty(filterValue))
                return query;

            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, filterMember);
            var value = GetValueExpression(filterMember, filterValue, param);
            var body = Expression.Equal(prop, value);
            var lambda = Expression.Lambda<Func<T, bool>>(body, param);

            return System.Linq.Queryable.Where(query, lambda);
        }

        private static UnaryExpression GetValueExpression(string propertyName, string val, ParameterExpression param)
        {
            var member = Expression.Property(param, propertyName);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            if (!converter.CanConvertFrom(typeof(string)))
                throw new NotSupportedException();
            var propertyValue = converter.ConvertFromInvariantString(val);
            var constant = Expression.Constant(propertyValue);
            return Expression.Convert(constant, propertyType);
        }
    }
}
