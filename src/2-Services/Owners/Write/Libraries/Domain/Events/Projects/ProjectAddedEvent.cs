﻿using TaskoMask.BuildingBlocks.Domain.Events;
using TaskoMask.Services.Owners.Write.Domain.Entities;

namespace TaskoMask.Services.Owners.Write.Domain.Events.Projects
{
    public class ProjectAddedEvent : DomainEvent
    {
        public ProjectAddedEvent(string id, string name, string description, string organizationId, string ownerId) : base(entityId: id, entityType: nameof(Project))
        {
            Id = id;
            Name = name;
            Description = description;
            OrganizationId = organizationId;
            OwnerId = ownerId;
        }

        public string Id { get; }
        public string Name { get; }
        public string Description { get;  }
        public string OrganizationId { get;  }
        public string OwnerId { get; }

    }
}