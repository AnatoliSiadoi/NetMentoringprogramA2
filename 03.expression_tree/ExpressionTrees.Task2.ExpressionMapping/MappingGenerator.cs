using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var constructor = GetDefaultConstructor(typeof(TDestination));
            var sourceParam = Expression.Parameter(typeof(TSource));
            var sourceInstance = Expression.Variable(typeof(TSource), "inInstance");
            var outInstance = Expression.Variable(typeof(TDestination), "outInstance");

            var expressions = new List<Expression>
            {
                Expression.Assign(sourceInstance, sourceParam),
                Expression.Assign(outInstance, Expression.New(constructor))
            };


            var sourceProperties = GetProperties(typeof(TSource), p => p.CanRead);
            var destProperties = GetProperties(typeof(TDestination), p => p.CanWrite);

            var assignedExpressions = from sourceProperty in sourceProperties
                                      let destProperty = destProperties.FirstOrDefault(p =>
                                         p.Name.Equals(sourceProperty.Name) && p.PropertyType == sourceProperty.PropertyType)
                                      where destProperty != null
                                      let sourceValue = Expression.Property(sourceInstance, sourceProperty)
                                      let outValue = Expression.Property(outInstance, destProperty)
                                      select Expression.Assign(outValue, sourceValue);

            expressions.AddRange(assignedExpressions);
            expressions.Add(outInstance);

            var body = Expression.Block(new[] { sourceInstance, outInstance }, expressions);
            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(body, sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type, Func<PropertyInfo, bool> predicate)
        {
            return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(predicate);
        }

        private ConstructorInfo GetDefaultConstructor(Type destType)
        {
            return destType.GetTypeInfo().DeclaredConstructors
                .FirstOrDefault(c => !c.IsStatic && c.GetParameters().Length == 0)
                ?? throw new ArgumentException("TDestination is not provide default constructor");
        }

    }
}
