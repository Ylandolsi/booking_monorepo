// export type SessionStatusType =
//   | 'scheduled'
//   | 'in-progress'
//   | 'completed'
//   | 'cancelled'
//   | 'no-show';

// export type BookingSessionType = {
//   id: string;
//   mentorSlug: string;
//   studentId: string;
//   date: string; // YYYY-MM-DD format
//   startTime: string; // HH:mm format
//   endTime: string; // HH:mm format
//   duration: number; // in minutes
//   price: number;
//   currency: string;
//   status: SessionStatusType;
//   notes?: string;
//   meetingLink?: string;
//   createdAt: string;
//   updatedAt: string;
//   mentor: {
//     slug: string;
//     firstName: string;
//     lastName: string;
//     avatar?: string;
//     title?: string;
//     expertise: string[];
//   };
//   student: {
//     id: string;
//     firstName: string;
//     lastName: string;
//     avatar?: string;
//   };
// };

// export type SessionBookingRequestType = {
//   mentorSlug: string;
//   date: string; // YYYY-MM-DD format
//   startTime: string; // HH:mm format
//   duration: number; // in minutes (default 30)
//   notes?: string;
// };
