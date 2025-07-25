export type Experience = {
  id: number;
  title: string;
  description: string;
  startDate: Date;
  endDate?: Date | null;
  companyName: string;
  toPresent: boolean;
  userId: number;
};
