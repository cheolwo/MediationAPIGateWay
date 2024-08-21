namespace Common.Actor.Builder
{
    public interface IDtoTypeCommandConfiguration<TDto> where TDto : class
    {
        public void Configure(DtoTypeCommandBuilder<TDto> builder); 
    }
}