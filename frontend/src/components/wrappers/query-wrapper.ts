import type { UseQueryResult } from '@tanstack/react-query';
import React from 'react';
import { MainErrorFallback } from '@/components/errors/main';
import { Spinner } from '@/components/ui';

interface QueryWrapperProps<T> {
  query: UseQueryResult<T>;
  children: (data: T) => React.ReactNode;
  loadingFallback?: React.ReactNode;
  errorFallback?: (error: Error) => React.ReactNode;
}

export function QueryWrapper<T>({
  query,
  children,
  loadingFallback,
  errorFallback,
}: QueryWrapperProps<T>) {
  //   if (isLoading)
  //   if (error || !user)
  //     return errorFallback || React.createElement(MainErrorFallback);

  //   return React.createElement(React.Fragment, null, children(user)); // equivalent to <> without props <>
  // }
  if (query.isLoading) {
    return loadingFallback || React.createElement(Spinner); // equivalent to <Spinner/>
  }

  if (query.error) {
    if (errorFallback) {
      return errorFallback(query.error as Error);
    }
    return React.createElement(MainErrorFallback);
  }

  if (!query.data) {
    return React.createElement('div', null, 'No data available');
  }

  return React.createElement(React.Fragment, null, children(query.data)); // equivalent to <> without props <>
}
// Usage example:
// function UserProfile({ userId }: { userId: string }) {
//   const userQuery = useUser({ userId });
//
//   return (
//     <QueryWrapper query={userQuery}>
//       {(user) => (
//         <div>
//           <h1>{user.name}</h1>
//           <p>{user.email}</p>
//         </div>
//       )}
//     </QueryWrapper>
//   );
// }
