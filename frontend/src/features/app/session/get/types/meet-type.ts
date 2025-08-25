export type Session = {
  id: number;
  mentorId: number;
  mentorFirstName?: string | null;
  mentorLastName?: string | null;
  mentorEmail?: string | null;
  mentorProfilePicture?: string | null;
  mentorProfilePictureBlurry?: string | null;
  price: number;
  status: string;
  googleMeetLink?: string | null;
  scheduledAt: string;
  durationInMinutes: number;
  updatedAt: string;
  completedAt?: string | null;
  cancelledAt?: string | null;
  createdAt: string;
  iamMentor: boolean;
};
