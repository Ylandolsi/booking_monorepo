
export type PaginatedResult<T> = {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  nextPage: boolean;
  previousPage: boolean;
};

// export const paginatedResultSchema = <T extends z.ZodTypeAny>(itemSchema: T) =>
//   z.object({
//     items: z.array(itemSchema),
//     page: z.number(),
//     pageSize: z.number(),
//     totalCount: z.number(),
//     totalPages: z.number(),
//   });

//   const paginatedAdminNotificationsSchema = paginatedResultSchema(adminNotificationDtoSchema);
  
// how to use it for parsing : paginatedAdminNotificationsSchema.parse(data)