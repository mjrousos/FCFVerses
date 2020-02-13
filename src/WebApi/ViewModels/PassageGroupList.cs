using System;
using System.Collections.Generic;

namespace WebApi.ViewModels
{
    public class PassageGroupList
    {
        public string? CopyrightNotice { get; }

        public IEnumerable<PassageGroup> PassageGroups { get; }

        public PassageGroupList(string? copyrightNotice, IEnumerable<PassageGroup> passageGroups)
        {
            PassageGroups = passageGroups ?? throw new ArgumentNullException(nameof(passageGroups));
            CopyrightNotice = copyrightNotice;
        }
    }
}
