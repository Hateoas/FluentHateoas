namespace FluentHateoas.Interfaces
{
    public interface IExpressionBuilderBase<TModel>
    {
        IHateoasExpression<TModel> GetExpression();
    }
}