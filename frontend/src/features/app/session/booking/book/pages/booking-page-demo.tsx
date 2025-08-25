// import { useState, useEffect } from 'react';
// import { useParams } from '@tanstack/react-router';
// import { Calendar, Clock, User, CheckCircle } from 'lucide-react';
// import {
//   Button,
//   Card,
//   CardContent,
//   CardHeader,
//   CardTitle,
//   Alert,
//   AlertDescription,
//   Badge,
//   Avatar,
//   AvatarImage,
//   AvatarFallback,
//   Spinner,
//   Calendar as BookingCalendar,
// } from '@/components/ui';
// import { TimeSlots } from '@/features/booking/shared/components/time-slots.tsx';
// import { BookingSummaryCard } from '@/features/booking/shared/components/booking-summary.tsx';
// import { useBookSession } from '@/features/booking/book-session/api/book-session-api.ts';
// import {
//   useMonthlyAvailability,
//   type DayAvailabilityType,
//   useBooking,
// } from '@/features/booking/book-session';
// import { type SessionSlot } from '@/features/booking/book-session';
// import { useMentorDetails } from '@/features/mentor/become/api';
// import type { BookingSummary } from '@/features/booking';
// import { useQueryState } from '@/hooks/use-query-state.ts';
// import { useIsMobile } from '@/hooks/use-mobile.ts';

// // {
// //   "year": 2025,
// //   "month": 8,
// //   "days": [
// //     {
// //       "date": "2025-08-01T00:00:00Z",
// //       "isAvailable": false,
// //       "timeSlots": [],
// //       "summary": {
// //         "totalSlots": 0,
// //         "availableSlots": 0,
// //         "bookedSlots": 0,
// //         "availabilityPercentage": 0
// //       }
// //     },

// //     {
// //       "date": "2025-08-21T00:00:00Z",
// //       "isAvailable": true,
// //       "timeSlots": [
// //         {
// //           "startTime": "08:00:00",
// //           "endTime": "08:30:00",
// //           "isBooked": false,
// //           "isAvailable": true
// //         },
// //         {
// //           "startTime": "08:45:00",
// //           "endTime": "09:15:00",
// //           "isBooked": false,
// //           "isAvailable": true
// //         },
// //         {
// //           "startTime": "09:30:00",
// //           "endTime": "10:00:00",
// //           "isBooked": false,
// //           "isAvailable": true
// //         },

// //       ],
// //       "summary": {
// //         "totalSlots": 8,
// //         "availableSlots": 8,
// //         "bookedSlots": 0,
// //         "availabilityPercentage": 100
// //       }
// //     },

// //   ]
// // }

// function BookingSummaryCo(props) {
//   return (
//     <div className="space-y-6">
//       {props.bookingSummary ? (
//         <>
//           <BookingSummaryCard booking={props.bookingSummary} />

//           {/* Booking Actions */}
//           <Card>
//             <CardContent className="pt-6">
//               <Button
//                 className="w-full"
//                 size="lg"
//                 onClick={props.onClick}
//                 disabled={
//                   !props.selectedDate ||
//                   !props.selectedSlot ||
//                   props.bookSessionMutation.isPending
//                 }
//               >
//                 {props.bookSessionMutation.isPending ? (
//                   <>
//                     <Clock className="w-4 h-4 mr-2 animate-spin" />
//                     Booking Session...
//                   </>
//                 ) : (
//                   <>
//                     <CheckCircle className="w-4 h-4 mr-2" />
//                     Book Session
//                   </>
//                 )}
//               </Button>

//               {props.bookSessionMutation.isError && (
//                 <Alert className="mt-4" variant="destructive">
//                   <AlertDescription>
//                     Failed to book session. Please try again.
//                   </AlertDescription>
//                 </Alert>
//               )}
//             </CardContent>
//           </Card>
//         </>
//       ) : (
//         <Card>
//           <CardHeader>
//             <CardTitle>Your Booking Summary</CardTitle>
//           </CardHeader>
//           <CardContent>
//             <div className="text-center py-8 text-muted-foreground">
//               <Calendar className="w-12 h-12 mx-auto mb-4 text-gray-400" />
//               <p className="text-lg font-medium mb-2">Select a date and time</p>
//               <p className="text-sm">
//                 Choose your preferred date and time slot to see the booking
//                 summary
//               </p>
//             </div>
//           </CardContent>
//         </Card>
//       )}

//       {/* Session Details Info */}
//       <Card>
//         <CardHeader>
//           <CardTitle className="text-base">Session Details</CardTitle>
//         </CardHeader>
//         <CardContent className="space-y-3">
//           <div className="flex justify-between">
//             <span className="text-sm text-gray-600">Duration:</span>
//             <span className="text-sm font-medium">30 minutes</span>
//           </div>
//           <div className="flex justify-between">
//             <span className="text-sm text-gray-600">Price:</span>
//             <span className="text-sm font-medium">
//               $50 (or mentor-specific pricing)
//             </span>
//           </div>
//           <div className="flex justify-between">
//             <span className="text-sm text-gray-600">Format:</span>
//             <span className="text-sm font-medium">Online video call</span>
//           </div>
//           <div className="pt-2 border-t">
//             <p className="text-xs text-gray-500">
//               * Duration is fixed at 30 minutes for now but will be customizable
//               in the future
//             </p>
//           </div>
//         </CardContent>
//       </Card>
//     </div>
//   );
// }

// export function BookingPageDemo() {
//   const {
//     mentorInfoQuery,
//     setSelectedDate,
//     selectedDate,
//     setBookingStep,
//     bookSessionMutation,
//     mentorDetailsQuery,
//     monthlyAvailabilityQuery,
//     bookingStep,
//   } = useBooking();
//   const onMobile = useIsMobile();
//   // Loading states
//   if (mentorDetailsQuery.isLoading || monthlyAvailabilityQuery.isLoading) {
//     return (
//       <div className="container mx-auto py-10 px-4 max-w-4xl">
//         <div className="flex items-center justify-center min-h-[400px]">
//           <div className="text-center space-y-4">
//             <Spinner className="w-8 h-8 mx-auto" />
//             <p className="text-lg text-gray-600">Loading booking page...</p>
//           </div>
//         </div>
//       </div>
//     );
//   }

//   // Error states
//   if (mentorDetailsQuery.isError) {
//     return (
//       <div className="container mx-auto py-10 px-4 max-w-4xl">
//         <Alert variant="destructive">
//           <AlertDescription>
//             Failed to load mentor details. Please try again later.
//           </AlertDescription>
//         </Alert>
//       </div>
//     );
//   }

//   // Success page
//   /*if (bookingStep === 'success') {
//     return (
//       <div className="container mx-auto py-10 px-4 max-w-4xl">
//         <div className="text-center space-y-6">
//           <div className="w-20 h-20 bg-green-100 rounded-full flex items-center justify-center mx-auto">
//             <CheckCircle className="w-10 h-10 text-green-600" />
//           </div>
//           <div>
//             <h1 className="text-3xl font-bold text-gray-900 mb-2">
//               Session Booked Successfully!
//             </h1>
//             <p className="text-lg text-gray-600">
//               Your mentoring session has been confirmed. You'll receive an email
//               with the meeting details.
//             </p>
//           </div>

//           {bookingSummary && (
//             <div className="max-w-md mx-auto">
//               <BookingSummaryCard booking={bookingSummary} />
//             </div>
//           )}

//           <div className="flex flex-col sm:flex-row gap-4 justify-center">
//             <Button onClick={() => (window.location.href = '/app/bookings')}>
//               View My Bookings
//             </Button>
//             <Button
//               variant="outline"
//               onClick={() => {
//                 setBookingStep('select');
//                 setSelectedDate(null);
//                 setSelectedSlot(null);
//               }}
//             >
//               Book Another Session
//             </Button>
//           </div>
//         </div>
//       </div>
//     );
//   }*/
//   const daySlots =
//     monthlyAvailabilityQuery.data?.days[selectedDate?.getDate() - 1];
//   console.log(monthlyAvailabilityQuery.data);
//   console.log(daySlots);
//   return (
//     <div className="container mx-auto py-10 px-4 max-w-7xl">
//       {/* Header */}
//       <div className="text-center space-y-6 mb-8">
//         <div>
//           <h1 className="text-4xl font-bold text-gray-900 mb-4">
//             Book a Session with {mentorInfoQuery.data?.firstName}{' '}
//             {mentorInfoQuery.data?.lastName}
//           </h1>
//           <p className="text-lg text-gray-600 max-w-3xl mx-auto">
//             Connect with experienced mentors across various fields and schedule
//             personalized sessions to accelerate your growth and achieve your
//             goals.
//           </p>
//         </div>

//         {/* How it works */}
//       </div>

//       {/* Mentor Profile */}
//       {mentorDetailsQuery.data && (
//         <Card className="mb-8">
//           <CardHeader>
//             <CardTitle className="flex items-center gap-2">
//               <User className="w-5 h-5" />
//               Mentor Profile
//             </CardTitle>
//           </CardHeader>
//           <CardContent>
//             <div className="flex items-startTime gap-4">
//               <Avatar className="w-16 h-16">
//                 <AvatarImage src={undefined} alt="Mentor Avatar" />
//                 <AvatarFallback>M</AvatarFallback>
//               </Avatar>
//               <div className="flex-1">
//                 <h3 className="text-xl font-semibold mb-1">
//                   Professional Mentor
//                 </h3>
//                 <p className="text-gray-600 mb-2">Experienced Mentor</p>
//                 <div className="flex flex-wrap gap-2 mb-3">
//                   {['General Mentoring'].map((skill, index) => (
//                     <Badge key={index} variant="secondary">
//                       {skill}
//                     </Badge>
//                   ))}
//                 </div>
//                 <div className="text-sm text-gray-600">
//                   <span className="font-medium">Hourly Rate: </span>
//                   <span className="text-lg font-semibold text-green-600">
//                     ${mentorDetailsQuery.data.hourlyRate || 50}/hour
//                   </span>
//                 </div>
//               </div>
//             </div>
//           </CardContent>
//         </Card>
//       )}

//       <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
//         {/* Calendar and Time Slots */}
//         <div className="lg:col-span-2 space-y-6">
//           {/* Calendar */}
//           <BookingCalendar
//             mode="single"
//             className="rounded-xl border shadow-sm "
//             captionLayout="label"
//             defaultMonth={selectedDate}
//             onSelect={setSelectedDate}
//             selected={selectedDate}
//             numberOfMonths={onMobile ? 1 : 1}
//             disabled={(date) => {
//               const pastDaysOfCurrentMonth =
//                 date.getDate() < new Date().getDate() &&
//                 date.getMonth() == new Date().getMonth();
//               const pastMonthsOfCurrentAndPreviousYears =
//                 date.getMonth() < new Date().getMonth() &&
//                 date.getFullYear() <= new Date().getFullYear();
//               return (
//                 pastDaysOfCurrentMonth || pastMonthsOfCurrentAndPreviousYears
//               );
//             }}
//           />
//           {/* Time Slots */}
//           {selectedDate &&
//             daySlots &&
//             daySlots.timeSlots.map((slot: SessionSlot, index: number) => {
//               return <div> {slot.startTime} </div>;
//             })}
//           ;
//           {/*{selectedDate && (
//             <div>
//               {dailyAvailabilityQuery.isLoading ? (
//                 <Card>
//                   <CardHeader>
//                     <CardTitle className="flex items-center gap-2">
//                       <Clock className="w-5 h-5" />
//                       Available Times
//                     </CardTitle>
//                   </CardHeader>
//                   <CardContent>
//                     <div className="space-y-2">
//                       {[...Array(4)].map((_, i) => (
//                         <div
//                           key={i}
//                           className="h-10 bg-gray-100 animate-pulse rounded"
//                         />
//                       ))}
//                     </div>
//                   </CardContent>
//                 </Card>
//               ) : (
//                 <TimeSlots
//                   selectedDate={selectedDate}
//                   timeSlots={[]}
//                   // timeSlots={dailyAvailabilityQuery.data?.slots|| []}
//                   selectedSlot={selectedSlot}
//                   onSlotSelect={setSelectedSlot}
//                 />
//               )}
//             </div>
//           )}*/}
//         </div>

//         {/* Booking Summary */}
//         {/*        <BookingSummaryCo bookingSummary={bookingSummary} onClick={handleBookSession} selectedDate={selectedDate}
//                           selectedSlot={selectedSlot} bookSessionMutation={bookSessionMutation} />*/}
//       </div>
//     </div>
//   );
// }
