namespace FluentHateoas.Interfaces
{
    public interface IHateoasExpressionBuilderBase<TModel>
    {
        IHateoasExpression<TModel> GetExpression();
    }
}