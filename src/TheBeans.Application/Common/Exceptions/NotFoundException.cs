namespace TheBeans.Application.Common.Exceptions
{
    /// <summary>
    /// Exception thrown when an entity is not found in the system.
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specified entity name and key.
        /// </summary>
        /// <param name="name">The name of the entity that was not found.</param>
        /// <param name="key">The key associated with the missing entity.</param>
        public NotFoundException(string name, object key) 
            : base($"{name} ( {key} ) is not found")
        {
        }
    }

}