export class BaseQuery
{
  SortBy: string
  SortDesc: boolean
  IsPaging: boolean
  PageIndex: number
  PageSize: number


  sortBy(sortBy: string, sortDesc: boolean = false)
  {
    this.SortBy = sortBy; this.SortDesc = sortDesc;
  }

  cancelSorting()
  {
    this.sortBy("", false);
  }
}
