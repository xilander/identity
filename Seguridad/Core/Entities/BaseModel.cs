namespace Seguridad.Core.Entities
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime ModifiedAt { get; set; }
    }
}
