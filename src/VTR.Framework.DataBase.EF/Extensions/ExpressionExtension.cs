namespace VTR.Framework.DataBase.EF;

public static class ExpressionExtension
{
    public static TFunc Call<TFunc>(this Expression<TFunc> expression)
    {
        throw new InvalidOperationException("This method should never be called. It is a marker for replacing.");
    }

    public static Expression<TFunc> SubstituteMarker<TFunc>(this Expression<TFunc> expression)
    {
        var visitor = new SubstituteExpressionCallVisitor();
        return (Expression<TFunc>)visitor.Visit(expression);
    }
}

#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
public class SubstituteExpressionCallVisitor : ExpressionVisitor
{
    private readonly MethodInfo _markerDesctiprion;

    public SubstituteExpressionCallVisitor()
    {

        _markerDesctiprion =
            typeof(ExpressionExtension).GetMethod(nameof(ExpressionExtension.Call)).GetGenericMethodDefinition();
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (IsMarker(node))
        {
            return Visit(ExtractExpression(node));
        }
        return base.VisitMethodCall(node);
    }

    private LambdaExpression ExtractExpression(MethodCallExpression node)
    {
        var target = node.Arguments[0];


        return (LambdaExpression)Expression.Lambda(target).Compile().DynamicInvoke();


    }

    private bool IsMarker(MethodCallExpression node)
    {
        return node.Method.IsGenericMethod && node.Method.GetGenericMethodDefinition() == _markerDesctiprion;
    }
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8603 // Possible null reference return.