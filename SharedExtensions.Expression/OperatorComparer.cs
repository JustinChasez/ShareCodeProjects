using System.Collections.Generic;

namespace System.Linq.Expressions;

internal enum OperatorComparer
{
    Contains,
    StartsWith,
    EndsWith,
    Equals             = ExpressionType.Equal,
    GreaterThan        = ExpressionType.GreaterThan,
    GreaterThanOrEqual = ExpressionType.GreaterThanOrEqual,
    LessThan           = ExpressionType.LessThan,
    LessThanOrEqual    = ExpressionType.LessThanOrEqual,
    NotEqual           = ExpressionType.NotEqual
}

internal static class OperatorComparerMapping
{
    private static readonly Dictionary<string, OperatorComparer> Mapping = 
        new()
        {
            {"eq", OperatorComparer.Equals},
            {"=", OperatorComparer.Equals},
            {"ne", OperatorComparer.NotEqual},
            {"!=", OperatorComparer.NotEqual},
            {"lt", OperatorComparer.LessThan},
            {"<", OperatorComparer.LessThan},
            {"le", OperatorComparer.LessThanOrEqual},
            {"<=", OperatorComparer.LessThanOrEqual},
            {"gt", OperatorComparer.GreaterThan},
            {">", OperatorComparer.GreaterThan},
            {"ge", OperatorComparer.GreaterThanOrEqual},
            {">=", OperatorComparer.GreaterThanOrEqual},
            {"sw", OperatorComparer.StartsWith},
            {"startsWith", OperatorComparer.StartsWith},
            {"ew", OperatorComparer.EndsWith},
            {"endsWith", OperatorComparer.EndsWith},
            {"like", OperatorComparer.Contains},
            {"contains", OperatorComparer.Contains},
        };

    public static OperatorComparer? ToCompareOperator(this string operatorString, bool shouldThrow = true)
    {
        if (Mapping.TryGetValue(operatorString, out var operatorComparer))
        {
            return operatorComparer;
        }

        if (!shouldThrow)
            return null;

        throw new InvalidOperationException($"Unsupported operator '{operatorString}'");
    }
}