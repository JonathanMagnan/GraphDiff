﻿using System.Data.Entity;
using System.Reflection;

namespace RefactorThis.GraphDiff.Internal.Members.Entities
{
    internal class OwnedEntity : AEntityMember
    {
        internal OwnedEntity(AMember parent, PropertyInfo accessor)
                : base(parent, accessor)
        {
        }

        protected override void UpdateInternal<T>(DbContext context, T existing, object dbValue, object newValue)
        {
            // Check if the same key, if so then update values on the entity
            if (IsKeyIdentical(context, newValue, dbValue))
                UpdateValuesWithConcurrencyCheck(context, newValue, dbValue);
            else
                SetValue(existing, newValue);

            AttachCyclicNavigationProperty(context, existing, newValue);

            foreach (var childMember in Members)
                childMember.Update(context, dbValue, newValue);
        }
    }
}
