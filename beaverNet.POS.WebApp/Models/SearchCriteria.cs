using beaverNet.POS.WebApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharpPS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models
{
    public class SearchColumn
    {
        public string data { get; set; }
        public int position { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; } = true;
        public bool orderable { get; set; } = true;
        public string displayname { get; set; }
    }

    public class SearchOrder
    {
        public string column { get; set; }
        public string dir { get; set; }
    }

    public class SearchSearch
    {
        public string value { get; set; }
    }

    public class SearchFilter
    {
        public string column { get; set; }
        public string value { get; set; }
        public string condition { get; set; }
    }

    public class SearchCriteria
    {
        public List<SearchColumn> columns { get; set; }
        public List<SearchFilter> filters { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public List<SearchOrder> order { get; set; }
        public SearchSearch search { get; set; }

        public object Apply<T>(ApplicationDbContext context, params string[] additionalfields)
            where T : class
        {
            return Apply<T>(context, null, additionalfields);
        }

        public object Apply<T>(ApplicationDbContext context, Expression<Func<T, object>>[] Includes = null, params string[] additionalfields)
            where T : class
        {
            IQueryable<T> queryable = context.Set<T>();
            if (Includes != null)
            {
                foreach (var item in Includes)
                {
                    queryable = queryable.Include(item);
                }
            }
            var recordsTotal = queryable.Count();
            var recordsFiltered = recordsTotal;
            queryable = ApplyFilters(queryable);
            if (this.search != null || this.filters != null)
                recordsFiltered = queryable.Count();
            queryable = ApplySorting(queryable);
            queryable = ApplyPaging(queryable);
            return new
            {
                this.draw,
                recordsTotal,
                recordsFiltered,
                data = queryable.ToList().Select(x => GetValueOnly(x, additionalfields))
            };
        }

        public IQueryable<T> ApplyPaging<T>(IQueryable<T> queryable) where T : class
        {
            int page = this.start == 0 ? 1 : (int)Math.Ceiling(this.start * 1.0 / this.length);
            queryable = queryable.PagedBy(page, this.length);
            return queryable;
        }

        public IQueryable<T> ApplyFilters<T>(IQueryable<T> quearyable) where T : class
        {
            Expression<Func<T, bool>> predicate = null;
            if (this.search != null && !string.IsNullOrEmpty(this.search.value))
            {
                var filterExpression = ExpressionBuilder.False<T>();
                string term = string.Concat("%", this.search.value, "%");
                foreach (var column in this.columns.Where(x => !string.IsNullOrEmpty(x.name)))
                {
                    if (column.searchable)
                    {
                        var searchExpresion = Like<T>(column.name, term);
                        if (searchExpresion != null)
                            predicate = filterExpression = ExpressionBuilder.Or(filterExpression, searchExpresion);
                    }
                }
            }

            if (this.filters != null)
            {
                var filterExpression = ExpressionBuilder.True<T>();
                foreach (var filter in this.filters.Where(x => !string.IsNullOrEmpty(x.column)))
                {
                    if (string.IsNullOrEmpty(filter.value))
                        continue;

                    var searchExpresion = MethodExpression<T>(filter.condition, filter.column, filter.value);
                    if (searchExpresion != null)
                        filterExpression = ExpressionBuilder.And(filterExpression, searchExpresion);                    
                }
                if (predicate != null)
                    predicate = predicate.And(filterExpression);
                else
                    predicate = filterExpression;
            }

            if (predicate != null)
                quearyable = quearyable.Where(predicate);
            return quearyable;
        }


        public IQueryable<T> ApplySorting<T>(IQueryable<T> source) where T : class
        {
            if (this.order != null)
            {
                IOrderedQueryable<T> orderedSource = new EntityOrderedQuerable<T>(source);
                bool isOrdered = false;
                foreach (var item in order)
                {
                    if (string.IsNullOrEmpty(item.column))
                        continue;
                    MemberExpressionBuilder builder = new MemberExpressionBuilder();
                    Expression<Func<T, object>> keySelector = null;
                    if (!builder.TryCreateSelector(item.column, out keySelector))
                        continue;

                    Type fieldType = null;
                    switch (builder.MemberExpression.Member.MemberType)
                    {
                        case MemberTypes.Field:
                            fieldType = (builder.MemberExpression.Member as FieldInfo).FieldType;
                            break;
                        default:
                            fieldType = (builder.MemberExpression.Member as PropertyInfo).PropertyType;
                            break;
                    }

                    Expression valueExpression = builder.MemberExpression;
                    if (fieldType == typeof(DateTimeOffset) || fieldType == typeof(DateTimeOffset?))
                        valueExpression = DateExpression(builder.MemberExpression);

                    keySelector = Expression.Lambda<Func<T, object>>(Expression.Convert(valueExpression, typeof(object)), builder.Parameter);

                    if (!isOrdered)
                    {
                        if (item.dir == "asc")
                            orderedSource = source.OrderBy(keySelector);
                        else
                            orderedSource = source.OrderByDescending(keySelector);
                        isOrdered = true;
                    }
                    else
                    {
                        if (item.dir == "asc")
                            orderedSource = orderedSource.ThenBy(keySelector);
                        else
                            orderedSource = orderedSource.ThenByDescending(keySelector);
                    }
                }
                return orderedSource;
            }

            return source;
        }

        public IEnumerable<object> GetValueOnly<T>(T instance, params string[] additionalfields)
        {
            foreach (var item in columns.Where(x => !string.IsNullOrEmpty(x.name)))
            {
                var names = item.name.Split('.');
                yield return GetValue(instance, names);
            }

            foreach (var item in instance.GetType().GetProperties())
            {
                if (!additionalfields.Any(x => x == item.Name))
                    continue;
                yield return item.GetValue(instance);
            }
        }

        object GetValue(object instance, IEnumerable<string> names)
        {
            foreach (var item in instance.GetType().GetProperties())
            {
                if (names.FirstOrDefault() != item.Name)
                    continue;
                if (names.Count() > 1)
                    return GetValue(item.GetValue(instance), names.Skip(1));
                var value = item.GetValue(instance);
                return value;
            }
            return null;
        }

        Expression<Func<T, bool>> MethodExpression<T>(string methodname, string propertyOrfield, string value)
        {
            MemberExpressionBuilder builder = new MemberExpressionBuilder();
            Expression<Func<T, object>> memberExpression = null;
            if (!builder.TryCreateSelector(propertyOrfield, out memberExpression))
                return null;
            Type fieldType = null;
            switch (builder.MemberExpression.Member.MemberType)
            {
                case MemberTypes.Field:
                    fieldType = (builder.MemberExpression.Member as FieldInfo).FieldType;
                    break;
                default:
                    fieldType = (builder.MemberExpression.Member as PropertyInfo).PropertyType;
                    break;
            }
            var converter = TypeDescriptor.GetConverter(fieldType);
            Expression valueExpression = Expression.Constant(converter.ConvertFromString(value), fieldType);

            if(fieldType == typeof(DateTimeOffset) || fieldType == typeof(DateTimeOffset?))
                valueExpression = DateExpression(valueExpression);

            var methodinfo = typeof(Expression).GetMethods()
                .Where(x => x.Name == methodname && x.GetParameters().Count() == 2)
                .FirstOrDefault();

            if (methodinfo == null)
                throw new Exception(string.Format("Expression '{0}' not supported or method not found.", methodinfo));

            Expression member = builder.MemberExpression;
            if (fieldType == typeof(DateTimeOffset) || fieldType == typeof(DateTimeOffset?))
                member = DateExpression(member);

            var expression = methodinfo.Invoke(null, new Expression[] { member, valueExpression }) as System.Linq.Expressions.Expression;
            var lambda = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(expression, builder.Parameter);
            return lambda;
        }

        Expression DateExpression(Expression valueExpression)
        {
            var dateMethodInfo = typeof(ApplicationDbContext).GetMethod(nameof(ApplicationDbContext.Date));
            var fn = Expression.Constant(new ApplicationDbContext(new DbContextOptions<ApplicationDbContext>()), typeof(ApplicationDbContext));
            return Expression.Call(fn, dateMethodInfo, valueExpression) as System.Linq.Expressions.Expression;
        }

        Expression<Func<T, bool>> Like<T>(string name, string value)
        {
            MemberExpressionBuilder builder = new MemberExpressionBuilder();
            Expression<Func<T, object>> memberExpression = null;
            if (!builder.TryCreateSelector(name, out memberExpression))
                return null;
            MethodInfo method = method = typeof(DbFunctionsExtensions).GetMethods().FirstOrDefault();
            var realValue = Expression.Constant(value, typeof(string));
            var fn = Expression.Constant(EF.Functions, typeof(DbFunctions));
            var containsMethodExp = Expression.Call(method, fn, builder.MemberExpression, realValue);
            return Expression.Lambda<Func<T, bool>>(containsMethodExp, builder.Parameter);
        }
    }
}
