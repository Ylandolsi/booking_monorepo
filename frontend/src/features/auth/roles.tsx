// import * as React from 'react';
// import { useUser } from '@/features/auth/auth';
// import type { User } from '@/types/api';
// import { Spinner } from '@/components/ui/spinner';
// export const ROLES = {
//   ADMIN: 'ADMIN',
//   USER: 'USER',
// } as const;

// type RoleTypes = keyof typeof ROLES;

// export const POLICIES = {
//   'user:view': (user: User) => {
//     return user.role === 'ADMIN' || user.role === 'USER';
//   },
//   'user:edit': (user: User, targetUser?: User) => {
//     if (user.role === 'ADMIN') {
//       return true;
//     }
//     if (user.role === 'USER' && targetUser && user.userId === targetUser.userId) {
//       return true;
//     }
//     return false;
//   },
//   'user:delete': (user: User, targetUser?: User) => {
//     if (user.role === 'ADMIN') {
//       return true;
//     }
//     return false;
//   },
//   'booking:create': (user: User) => {
//     return user.isActive && (user.role === 'ADMIN' || user.role === 'USER');
//   },
//   'booking:manage': (user: User) => {
//     return user.role === 'ADMIN';
//   },
// };

// export const useAuthorization = () => {
//   const { data: user, isLoading, error } = useUser();

//   const checkAccess = React.useCallback(
//     ({ allowedRoles }: { allowedRoles: RoleTypes[] }) => {
//       if (!user || error) {
//         return false;
//       }

//       if (allowedRoles && allowedRoles.length > 0) {
//         return allowedRoles.includes(user.role as RoleTypes);
//       }

//       return true;
//     },
//     [user, error],
//   );

//   const checkPolicy = React.useCallback(
//     (policy: keyof typeof POLICIES, resource?: any) => {
//       if (!user || error) {
//         return false;
//       }

//       return POLICIES[policy](user, resource);
//     },
//     [user, error],
//   );

//   return { 
//     checkAccess, 
//     checkPolicy, 
//     role: user?.role, 
//     user, 
//     isLoading,
//     error 
//   };
// };

// type AuthorizationProps = {
//   forbiddenFallback?: React.ReactNode;
//   children: React.ReactNode;
// } & (
//   | {
//       allowedRoles: RoleTypes[];
//       policyCheck?: never;
//     }
//   | {
//       allowedRoles?: never;
//       policyCheck: keyof typeof POLICIES;
//       resource?: any;
//     }
// );

// export const Authorization = ({
//   policyCheck,
//   allowedRoles,
//   forbiddenFallback = <p className="text-red-500">You do not have permission to access this resource.</p>,
//   children,
//   resource,
// }: AuthorizationProps) => {
//   const { checkAccess, checkPolicy, isLoading } = useAuthorization();

//   if (isLoading) {
//     return <Spinner />;
//   }

//   let canAccess = false;

//   if (allowedRoles) {
//     canAccess = checkAccess({ allowedRoles });
//   }

//   // if (policyCheck) {
//   //   canAccess = checkPolicy(policyCheck, resource);
//   // }
 

//   return <>{canAccess ? children : forbiddenFallback}</>;
// };


// // import { useAuthorization, ROLES, POLICIES } from '@/lib/authorization'

// // export const usePermissions = () => {
// //   const { checkAccess, checkPolicy, user } = useAuthorization()

// //   return {
// //     canViewUsers: checkAccess({ allowedRoles: [ROLES.ADMIN] }),
// //     canEditUser: (targetUser?: any) => checkPolicy('user:edit', targetUser),
// //     canDeleteUser: (targetUser?: any) => checkPolicy('user:delete', targetUser),
// //     canCreateBooking: checkPolicy('booking:create'),
// //     canManageBookings: checkPolicy('booking:manage'),
// //     isAdmin: user?.role === ROLES.ADMIN,
// //     isUser: user?.role === ROLES.USER,
// //   }
// // }