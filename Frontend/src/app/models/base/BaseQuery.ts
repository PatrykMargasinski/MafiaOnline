export class BaseQuery
{
  SortBy: string
  SortDesc: boolean

  sortBy(sortBy: string, sortDesc: boolean = false)
  {
    this.SortBy = sortBy; this.SortDesc = sortDesc;
  }

  cancelSorting()
  {
    this.sortBy("", false);
  }
}
