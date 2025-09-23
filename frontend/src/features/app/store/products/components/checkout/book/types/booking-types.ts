export type BookSessionRequestType = {
  mentorSlug: string;
  date: string; // "YYYY-MM-DD"
  startTime: string; // HH:MM
  endTime: string;
  notes?: string;
  title: string;
};

export type BookingSummaryType = {
  mentor: {
    slug: string;
    name: string;
    avatar?: string;
    title?: string;
    expertise: string[];
    hourlyRate: number;
  };
  session: {
    date: string;
    time: string;
    duration: number;
    price: number;
    currency: string;
  };
  total: number;
};
