export type ExperienceType = {
  id?: number;
  title: string;
  description: string;
  startDate: Date;
  endDate?: Date | null;
  company: string;
  toPresent?: boolean;
  userId?: number;
};
