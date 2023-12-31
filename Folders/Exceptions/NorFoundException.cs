﻿namespace Folders.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string name, long id)
            : base($"Entity {name} with id {id} not found.") { }

        public NotFoundException(string name)
            : base($"Entity {name} not found.") { }
    }
}
