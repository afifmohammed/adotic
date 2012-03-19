using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Adotic
{
    internal static class Extensions
    {
        public static TInstance Do<TInstance>(this TInstance instance, Action<TInstance> actionToInvoke)
        {
            actionToInvoke(instance);
            return instance;
        }

        /// <summary>
        /// converts to date time using the <see cref="DateTimeOffset.LocalDateTime"/> call
        /// </summary>
        public static DateTime ToDateTime(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.LocalDateTime;
        }

        /// <summary>
        /// extension method to invoke <see cref="string.Format(string,object[])"/>
        /// </summary>
        public static string For(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        public static string Name<T>(this Expression<Func<T, object>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }

        public static string Name<TInterface, TReturn>(this Expression<Func<TInterface, TReturn>> expression)
        {
            Expression body = expression.Body;
            return GetMemberName(body);
        }

        static string GetMemberName(Expression expression)
        {
            const string memberAppender = "";

            if (expression is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression;

                if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
                {
                    return GetMemberName(memberExpression.Expression) + memberAppender + memberExpression.Member.Name;
                }

                return memberExpression.Member.Name;
            }

            if (expression is UnaryExpression)
            {
                var unaryExpression = (UnaryExpression)expression;

                if (unaryExpression.NodeType != ExpressionType.Convert)
                    throw new Exception("Cannot interpret member from {0}".For(expression));

                return GetMemberName(unaryExpression.Operand);
            }

            if (expression is MethodCallExpression)
            {
                var methodCallExpression = (MethodCallExpression)expression;

                return methodCallExpression.Method.Name;
            }

            throw new Exception("Could not determine member from {0}".For(expression));
        }

        /// <summary>
        /// enumerates each item on the <paramref name="items"/> collection and will apply the <paramref name="action"/> on it.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static bool IsOpen(this IDbConnection connection)
        {
            return connection.State == ConnectionState.Open;
        }
    }
}