export type Session = {
  id: number;
  mentorId: number;
  mentorFirstName?: string | null;
  mentorLastName?: string | null;
  mentorEmail?: string | null;
  mentorProfilePicture?: string | null;
  mentorProfilePictureBlurry?: string | null;

  menteeId: number;
  menteeFirstName?: string | null;
  menteeLastName?: string | null;
  menteeEmail?: string | null;
  menteeProfilePicture?: string | null;
  menteeProfilePictureBlurry?: string | null;

  price: number;
  status: SessionStatus;

  googleMeetLink?: string | null;
  scheduledAt: string;
  durationInMinutes: number;
  updatedAt: string;
  completedAt?: string | null;
  cancelledAt?: string | null;
  createdAt: string;
  iamMentor: boolean;
};

// @ts-expect-error : eslint issue skip
export enum SessionStatus {
  Booked = 1,
  WaitingForPayment = 2,
  Confirmed = 3, // means paid
  Completed = 4,
  Cancelled = 5,
  NoShow = 6,
}
