namespace BlogApplication.Framework.ResultHelper
{
    public class ObjectResult<TEntity> : Result
    {
        public TEntity Data { get; set; }

        public void SetData(TEntity entity)
        {
            this.Data = entity;
            this.HasFailed = false;
        }
    }
}