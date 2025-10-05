export type PaginatedResult<T> = {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  nextPage: boolean;
  previousPage: boolean;
};
