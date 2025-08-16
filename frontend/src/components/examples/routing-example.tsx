// /**
//  * Example component demonstrating the new centralized routing system
//  * This file shows how to use the routing utilities in a real component
//  */

// import React from 'react';
// import { Link } from '@tanstack/react-router';
// import { useAppNavigation } from '@/hooks/use-navigation';
// import { routes, ROUTE_PATHS } from '@/config/routes';
// import { routeUtils } from '@/config/route-registry';
// import { Card, Button } from '@/components/ui';

// export function RoutingExampleComponent() {
//   const nav = useAppNavigation();

//   // Example: Dynamic mentor slug
//   const mentorSlug = 'john-doe-mentor';
//   const userSlug = 'user-123';

//   // Example: Check current route type
//   const currentPath = window.location.pathname;
//   const isAuthRoute = routes.isAuthRoute(currentPath);
//   const isProtectedRoute = routeUtils.isProtectedRoute(currentPath);
//   const routeGroup = routeUtils.getRouteGroup(currentPath);

//   const handleNavigationExamples = {
//     // Auth navigation
//     login: () => nav.goToLogin({ redirectTo: '/app' }),
//     register: () => nav.goToRegister(),
//     forgotPassword: () => nav.goToForgotPassword(),

//     // App navigation
//     dashboard: () => nav.goToDashboard(),

//     // Booking navigation
//     bookSession: () => nav.goToBookingSession(mentorSlug),
//     bookingDemo: () => nav.goToBookingDemo(mentorSlug),
//     bookingEnhanced: () => nav.goToBookingEnhanced(mentorSlug),

//     // Profile navigation
//     viewProfile: () => nav.goToProfile(userSlug),

//     // Mentor navigation
//     becomeMentor: () => nav.goToMentorBecome(),
//     setSchedule: () => nav.goToMentorSetSchedule(),
//   };

//   return (
//     <div className="p-6 space-y-6">
//       <h1 className="text-2xl font-bold">Centralized Routing Examples</h1>

//       {/* Route Information */}
//       <Card className="p-4">
//         <h2 className="text-lg font-semibold mb-2">
//           Current Route Information
//         </h2>
//         <div className="space-y-1 text-sm">
//           <p>
//             <strong>Current Path:</strong> {currentPath}
//           </p>
//           <p>
//             <strong>Is Auth Route:</strong> {isAuthRoute ? 'Yes' : 'No'}
//           </p>
//           <p>
//             <strong>Is Protected Route:</strong>{' '}
//             {isProtectedRoute ? 'Yes' : 'No'}
//           </p>
//           <p>
//             <strong>Route Group:</strong> {routeGroup || 'Unknown'}
//           </p>
//         </div>
//       </Card>

//       {/* Navigation Examples */}
//       <Card className="p-4">
//         <h2 className="text-lg font-semibold mb-4">Navigation Examples</h2>
//         <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
//           {/* Auth Navigation */}
//           <div className="space-y-2">
//             <h3 className="font-medium text-sm">Auth</h3>
//             <Button
//               onClick={handleNavigationExamples.login}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Login
//             </Button>
//             <Button
//               onClick={handleNavigationExamples.register}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Register
//             </Button>
//             <Button
//               onClick={handleNavigationExamples.forgotPassword}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Forgot Password
//             </Button>
//           </div>

//           {/* App Navigation */}
//           <div className="space-y-2">
//             <h3 className="font-medium text-sm">App</h3>
//             <Button
//               onClick={handleNavigationExamples.dashboard}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Dashboard
//             </Button>
//           </div>

//           {/* Booking Navigation */}
//           <div className="space-y-2">
//             <h3 className="font-medium text-sm">Booking</h3>
//             <Button
//               onClick={handleNavigationExamples.bookSession}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Book Session
//             </Button>
//             <Button
//               onClick={handleNavigationExamples.bookingDemo}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Booking Demo
//             </Button>
//             <Button
//               onClick={handleNavigationExamples.bookingEnhanced}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Enhanced Booking
//             </Button>
//           </div>

//           {/* Profile Navigation */}
//           <div className="space-y-2">
//             <h3 className="font-medium text-sm">Profile</h3>
//             <Button
//               onClick={handleNavigationExamples.viewProfile}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               View Profile
//             </Button>
//           </div>

//           {/* Mentor Navigation */}
//           <div className="space-y-2">
//             <h3 className="font-medium text-sm">Mentor</h3>
//             <Button
//               onClick={handleNavigationExamples.becomeMentor}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Become Mentor
//             </Button>
//             <Button
//               onClick={handleNavigationExamples.setSchedule}
//               variant="outline"
//               size="sm"
//               className="w-full"
//             >
//               Set Schedule
//             </Button>
//           </div>
//         </div>
//       </Card>

//       {/* Link Examples */}
//       <Card className="p-4">
//         <h2 className="text-lg font-semibold mb-4">
//           Link Examples (using route builders)
//         </h2>
//         <div className="space-y-2">
//           <div>
//             <Link
//               to={routes.to.auth.login({ redirectTo: '/app' })}
//               className="text-blue-600 hover:underline"
//             >
//               Login with redirect to app
//             </Link>
//           </div>

//           <div>
//             <Link
//               to={routes.to.booking.enhanced(mentorSlug)}
//               className="text-blue-600 hover:underline"
//             >
//               Enhanced booking for {mentorSlug}
//             </Link>
//           </div>

//           <div>
//             <Link
//               to={routes.to.profile.user(userSlug)}
//               className="text-blue-600 hover:underline"
//             >
//               User profile for {userSlug}
//             </Link>
//           </div>
//         </div>
//       </Card>

//       {/* Route Constants */}
//       <Card className="p-4">
//         <h2 className="text-lg font-semibold mb-4">Route Constants</h2>
//         <div className="grid grid-cols-2 gap-4 text-sm">
//           <div>
//             <h3 className="font-medium mb-2">Auth Routes</h3>
//             <ul className="space-y-1">
//               <li>
//                 <code>{ROUTE_PATHS.AUTH.LOGIN}</code>
//               </li>
//               <li>
//                 <code>{ROUTE_PATHS.AUTH.REGISTER}</code>
//               </li>
//               <li>
//                 <code>{ROUTE_PATHS.AUTH.FORGOT_PASSWORD}</code>
//               </li>
//               <li>
//                 <code>{ROUTE_PATHS.AUTH.RESET_PASSWORD}</code>
//               </li>
//             </ul>
//           </div>

//           <div>
//             <h3 className="font-medium mb-2">Booking Routes</h3>
//             <ul className="space-y-1">
//               <li>
//                 <code>{ROUTE_PATHS.BOOKING.SESSION}</code>
//               </li>
//               <li>
//                 <code>{ROUTE_PATHS.BOOKING.DEMO}</code>
//               </li>
//               <li>
//                 <code>{ROUTE_PATHS.BOOKING.ENHANCED}</code>
//               </li>
//               <li>
//                 <code>{ROUTE_PATHS.BOOKING.TEST}</code>
//               </li>
//             </ul>
//           </div>
//         </div>
//       </Card>

//       {/* Usage Tips */}
//       <Card className="p-4">
//         <h2 className="text-lg font-semibold mb-2">Usage Tips</h2>
//         <ul className="text-sm space-y-1">
//           <li>
//             • Use <code>useAppNavigation()</code> hook for programmatic
//             navigation
//           </li>
//           <li>
//             • Use <code>routes.to.*</code> for generating URLs with parameters
//           </li>
//           <li>
//             • Use <code>ROUTE_PATHS</code> for direct path constants
//           </li>
//           <li>
//             • Use <code>routeUtils</code> for route validation and checking
//           </li>
//           <li>• All route builders handle URL encoding automatically</li>
//           <li>
//             • Dynamic routes use <code>$paramName</code> syntax
//           </li>
//         </ul>
//       </Card>
//     </div>
//   );
// }

// export default RoutingExampleComponent;
