using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TheBeans.Core.Common
{
	public abstract class BaseEntity
	{

		[Key]
		public Guid Id { get; private set; } = Guid.NewGuid();

		public DateTime CreatedAt { get; protected set; }
		public DateTime? LastModified { get; protected set; }

		protected BaseEntity()
		{
			CreatedAt = DateTime.UtcNow;
		}

		public void SetModified()
		{
			LastModified = DateTime.UtcNow;
		}
	}
}

