import { createFileRoute } from '@tanstack/react-router';
import { ROUTE_PATHS } from '@/config/routes';
import { HomePage } from '@/components';

export const Route = createFileRoute(ROUTE_PATHS.APP.INDEX)({
  component: HomePage,
});




// old one 
// function HomePage() {
//   const location = useLocation();
//   const error = new URLSearchParams(location.search).get('error') ?? undefined;
//   const { currentUser } = useAuth();
//   const { isMentor } = useMentor();
//   const nav = useAppNavigation();

//   useEffect(() => {
//     if (error) {
//       toast.error(error);
//     }
//   }, [error]);

//   const currentHour = new Date().getHours();
//   const getGreeting = () => {
//     if (currentHour < 12) return 'Good morning';
//     if (currentHour < 18) return 'Good afternoon';
//     return 'Good evening';
//   };

//   return (
//     <div className="space-y-8">
//       {/* Welcome Header */}
//       <div className="">
//         <h1 className="text-3xl font-bold tracking-tight text-foreground">
//           {getGreeting()}, {currentUser?.firstName}! 👋
//         </h1>
//         <p className="mt-2 text-muted-foreground">
//           {isMentor
//             ? 'Welcome back to your mentoring dashboard'
//             : 'Ready to learn something new today?'}
//         </p>
//       </div>

//       {/* Quick Stats */}
//       <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-4">
//         <Card>
//           <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
//             <CardTitle className="text-sm font-medium">Upcoming Sessions</CardTitle>
//             <Calendar className="h-4 w-4 text-muted-foreground" />
//           </CardHeader>
//           <CardContent>
//             <div className="text-2xl font-bold">3</div>
//             <p className="text-xs text-muted-foreground">
//               Next session in 2 hours
//             </p>
//           </CardContent>
//         </Card>

//         <Card>
//           <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
//             <CardTitle className="text-sm font-medium">
//               {isMentor ? 'Students Helped' : 'Sessions Completed'}
//             </CardTitle>
//             <Users className="h-4 w-4 text-muted-foreground" />
//           </CardHeader>
//           <CardContent>
//             <div className="text-2xl font-bold">{isMentor ? '24' : '12'}</div>
//             <p className="text-xs text-muted-foreground">
//               +3 from last week
//             </p>
//           </CardContent>
//         </Card>

//         <Card>
//           <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
//             <CardTitle className="text-sm font-medium">Learning Hours</CardTitle>
//             <Clock className="h-4 w-4 text-muted-foreground" />
//           </CardHeader>
//           <CardContent>
//             <div className="text-2xl font-bold">48</div>
//             <p className="text-xs text-muted-foreground">
//               This month
//             </p>
//           </CardContent>
//         </Card>

//         <Card>
//           <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
//             <CardTitle className="text-sm font-medium">
//               {isMentor ? 'Avg Rating' : 'Skills Learned'}
//             </CardTitle>
//             {isMentor ? (
//               <Star className="h-4 w-4 text-muted-foreground" />
//             ) : (
//               <GraduationCap className="h-4 w-4 text-muted-foreground" />
//             )}
//           </CardHeader>
//           <CardContent>
//             <div className="text-2xl font-bold">
//               {isMentor ? '4.9' : '7'}
//             </div>
//             <p className="text-xs text-muted-foreground">
//               {isMentor ? 'Based on 45 reviews' : 'New skills this month'}
//             </p>
//           </CardContent>
//         </Card>
//       </div>

//       {/* Main Content Grid */}
//       <div className="grid gap-6 lg:grid-cols-3">
//         {/* Quick Actions */}
//         <div className="lg:col-span-2 space-y-6">
//           {/* Actions Card */}
//           <Card>
//             <CardHeader>
//               <CardTitle className="flex items-center gap-2">
//                 <TrendingUp className="h-5 w-5" />
//                 Quick Actions
//               </CardTitle>
//             </CardHeader>
//             <CardContent className="grid gap-4 sm:grid-cols-2">
//               {isMentor ? (
//                 <>
//                   <Button
//                     variant="outline"
//                     className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
//                     onClick={() => nav.goToMentorSetSchedule()}
//                   >
//                     <div className="flex items-center gap-2 w-full">
//                       <Calendar className="h-5 w-5 text-primary" />
//                       <span className="font-medium">Set Availability</span>
//                     </div>
//                     <span className="text-sm text-muted-foreground">
//                       Update your available time slots
//                     </span>
//                   </Button>
//                   <Button
//                     variant="outline"
//                     className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
//                     onClick={() => nav.goToMeets()}
//                   >
//                     <div className="flex items-center gap-2 w-full">
//                       <Video className="h-5 w-5 text-primary" />
//                       <span className="font-medium">View Sessions</span>
//                     </div>
//                     <span className="text-sm text-muted-foreground">
//                       Manage your upcoming meetings
//                     </span>
//                   </Button>
//                 </>
//               ) : (
//                 <>
//                   <Button
//                     variant="outline"
//                     className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
//                     onClick={() => nav.goToMentorBecome()}
//                   >
//                     <div className="flex items-center gap-2 w-full">
//                       <GiTeacher className="h-5 w-5 text-primary" />
//                       <span className="font-medium">Become a Mentor</span>
//                     </div>
//                     <span className="text-sm text-muted-foreground">
//                       Share your expertise with others
//                     </span>
//                   </Button>
//                   <Button
//                     variant="outline"
//                     className="h-auto p-4 text-left justify-start flex-col items-start space-y-2"
//                     onClick={() => nav.goToMeets()}
//                   >
//                     <div className="flex items-center gap-2 w-full">
//                       <BookOpen className="h-5 w-5 text-primary" />
//                       <span className="font-medium">Find Mentors</span>
//                     </div>
//                     <span className="text-sm text-muted-foreground">
//                       Discover experts in your field
//                     </span>
//                   </Button>
//                 </>
//               )}
//             </CardContent>
//           </Card>

//           {/* Recent Activity */}
//           <Card>
//             <CardHeader>
//               <CardTitle className="flex items-center gap-2">
//                 <Clock className="h-5 w-5" />
//                 Recent Activity
//               </CardTitle>
//             </CardHeader>
//             <CardContent className="space-y-4">
//               <div className="flex items-start gap-4">
//                 <div className="flex h-9 w-9 items-center justify-center rounded-full bg-primary/10">
//                   <Video className="h-4 w-4 text-primary" />
//                 </div>
//                 <div className="flex-1 min-w-0">
//                   <p className="text-sm font-medium">Session completed with John Doe</p>
//                   <p className="text-sm text-muted-foreground">2 hours ago</p>
//                 </div>
//               </div>
//               <div className="flex items-start gap-4">
//                 <div className="flex h-9 w-9 items-center justify-center rounded-full bg-chart-2/10">
//                   <MessageCircle className="h-4 w-4 text-chart-2" />
//                 </div>
//                 <div className="flex-1 min-w-0">
//                   <p className="text-sm font-medium">New message received</p>
//                   <p className="text-sm text-muted-foreground">1 day ago</p>
//                 </div>
//               </div>
//               <div className="flex items-start gap-4">
//                 <div className="flex h-9 w-9 items-center justify-center rounded-full bg-chart-3/10">
//                   <Calendar className="h-4 w-4 text-chart-3" />
//                 </div>
//                 <div className="flex-1 min-w-0">
//                   <p className="text-sm font-medium">Session booked for tomorrow</p>
//                   <p className="text-sm text-muted-foreground">2 days ago</p>
//                 </div>
//               </div>
//             </CardContent>
//           </Card>
//         </div>

//         {/* Sidebar Content */}
//         <div className="space-y-6">
//           {/* Profile Card */}
//           <Card>
//             <CardHeader>
//               <CardTitle className="flex items-center gap-2">
//                 <User className="h-5 w-5" />
//                 Your Profile
//               </CardTitle>
//             </CardHeader>
//             <CardContent className="space-y-4">
//               <div className="flex items-center gap-3">
//                 <div className="h-12 w-12 rounded-full bg-primary/10 flex items-center justify-center">
//                   <User className="h-6 w-6 text-primary" />
//                 </div>
//                 <div className="flex-1 min-w-0">
//                   <p className="font-medium">
//                     {currentUser?.firstName} {currentUser?.lastName}
//                   </p>
//                   <p className="text-sm text-muted-foreground truncate">
//                     {currentUser?.email}
//                   </p>
//                 </div>
//               </div>
//               <div className="flex items-center gap-2">
//                 {isMentor ? (
//                   <Badge variant="secondary" className="gap-1">
//                     <GiTeacher className="h-3 w-3" />
//                     Mentor
//                   </Badge>
//                 ) : (
//                   <Badge variant="outline">Student</Badge>
//                 )}
//               </div>
//               <Button
//                 variant="outline"
//                 size="sm"
//                 className="w-full"
//                 onClick={() => nav.goToProfile(currentUser?.slug || '')}
//               >
//                 <Settings className="h-4 w-4 mr-2" />
//                 Edit Profile
//               </Button>
//             </CardContent>
//           </Card>

//           {/* Upcoming Sessions */}
//           <Card>
//             <CardHeader>
//               <CardTitle className="flex items-center gap-2">
//                 <Calendar className="h-5 w-5" />
//                 Upcoming Sessions
//               </CardTitle>
//             </CardHeader>
//             <CardContent className="space-y-3">
//               <div className="rounded-lg border p-3">
//                 <div className="flex items-center justify-between">
//                   <div>
//                     <p className="font-medium text-sm">React Development</p>
//                     <p className="text-xs text-muted-foreground">with Sarah Johnson</p>
//                   </div>
//                   <Badge variant="outline" className="text-xs">
//                     Today 2:00 PM
//                   </Badge>
//                 </div>
//               </div>
//               <div className="rounded-lg border p-3">
//                 <div className="flex items-center justify-between">
//                   <div>
//                     <p className="font-medium text-sm">System Design</p>
//                     <p className="text-xs text-muted-foreground">with Mike Chen</p>
//                   </div>
//                   <Badge variant="outline" className="text-xs">
//                     Tomorrow 10:00 AM
//                   </Badge>
//                 </div>
//               </div>
//               <Button
//                 variant="ghost"
//                 size="sm"
//                 className="w-full text-sm"
//                 onClick={() => nav.goToMeets()}
//               >
//                 View All Sessions
//                 <ArrowRight className="h-3 w-3 ml-1" />
//               </Button>
//             </CardContent>
//           </Card>
//         </div>
//       </div>
//     </div>
//   );
// }
