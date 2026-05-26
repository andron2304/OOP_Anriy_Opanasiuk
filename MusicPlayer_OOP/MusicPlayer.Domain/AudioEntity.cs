using System;

namespace MusicPlayer.Domain
{
    public abstract class AudioEntity
    {
        public Guid Id { get; }
        public string Name { get; protected set; }

        protected AudioEntity(string name)
        {
            Id = Guid.NewGuid();
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name cannot be empty.") : name;
        }
    }
}