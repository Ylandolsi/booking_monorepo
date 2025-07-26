import { useQuery, queryOptions } from '@tanstack/react-query';
import { getCurrentUser } from '@/features/auth';

export const getUserQueryOptions = () => {
  return queryOptions({
    queryKey: ['user'],
    queryFn: getCurrentUser,
    staleTime: 5 * 60 * 1000, // 5 minutes
  });
};

export const useUser = () => useQuery(getUserQueryOptions());

// interface UserDataWrapperProps {
//   children: (user: User) => React.ReactNode;
//   loadingFallback?: React.ReactNode;
//   errorFallback?: React.ReactNode;
// }

// export function UserDataWrapper({
//   children,
//   loadingFallback,
//   errorFallback,
// }: UserDataWrapperProps) {
//   const { data: user, error, isLoading } = useUser();

//   if (isLoading) return loadingFallback || React.createElement(Spinner); // equivalent to <Spinner/>
//   if (error || !user)
//     return errorFallback || React.createElement(MainErrorFallback);

//   return React.createElement(React.Fragment, null, children(user)); // equivalent to <> without props <>
// }

// export function ExperienceForm() {
//   return (
//     <UserDataWrapper>
//       {user => (
//         <>
//         </>
//       )}
//     </UserDataWrapper>
//   );
// }

// --------------------------------
// interface UserDataWrapperProps {
//   Component: React.ComponentType<{ user: User }>;
//   loadingFallback?: React.ReactNode;
//   errorFallback?: React.ReactNode;
// }

// export function UserDataWrapper({
//   Component,
//   loadingFallback,
//   errorFallback,
// }: UserDataWrapperProps) {
//   const { data: user, error, isLoading } = useUser();

//   if (isLoading) return loadingFallback || React.createElement(Spinner);
//   if (error || !user)
//     return errorFallback || React.createElement(MainErrorFallback);

//   return React.createElement(Component, { user });
// }
// export function ExperienceForm() {
//   return (
//     <UserDataWrapper Component={Experience} />
//
//   );
// }
