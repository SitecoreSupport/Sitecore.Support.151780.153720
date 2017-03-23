using Sitecore.ContentSearch;
using Sitecore.Data.Items;
using Sitecore.Reflection;
using System.Collections.Generic;

namespace Sitecore.Support.ContentSearch
{
  public class SitecoreItemCrawler : Sitecore.ContentSearch.SitecoreItemCrawler
  {
    protected override void UpdateItemVersion(IProviderUpdateContext context, Item version, IndexEntryOperationContext operationContext)
    {
      SitecoreIndexableItem indexable = ReflectionUtil.CallMethod(this, "PrepareIndexableVersion", new object[] { version, context }) as SitecoreIndexableItem;
      base.Operations.Update(indexable, context, context.Index.Configuration);
      this.UpdateClones(context, indexable);
      this.UpdateLanguageFallbackDependentItems(context, indexable, operationContext);
    }
    private void UpdateClones(IProviderUpdateContext context, SitecoreIndexableItem versionIndexable)
    {
      IEnumerable<Item> clones;
      using (new WriteCachesDisabler())
      {
        clones = versionIndexable.Item.GetClones(false);
      }
      foreach (Item item in clones)
      {
        if (item.RuntimeSettings.TemporaryVersion || item.Language!= versionIndexable.Item.Language)
        {         
          continue;
        }
        SitecoreIndexableItem indexable = ReflectionUtil.CallMethod(this, "PrepareIndexableVersion", new object[] { item, context }) as SitecoreIndexableItem;
        if (!this.IsExcludedFromIndex(item, false))
        {
          base.Operations.Update(indexable, context, context.Index.Configuration);
        }
      }
    }
  }
}