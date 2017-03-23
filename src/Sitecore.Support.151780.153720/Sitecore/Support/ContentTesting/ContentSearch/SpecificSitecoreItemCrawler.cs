using Sitecore.ContentSearch;

namespace Sitecore.Support.ContentTesting.ContentSearch
{
  public class SpecificSitecoreItemCrawler : Sitecore.Support.ContentSearch.SitecoreItemCrawler
  {
    public override int GetContextIndexRanking(IIndexable indexable)
    {
      return 0x7fffffff;
    }
  }
}