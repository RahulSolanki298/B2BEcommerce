namespace Core.IRepository
{
    public interface IProductImageRepository
    {
        Task<bool> ImageUploadByZipFile();

    }
}
