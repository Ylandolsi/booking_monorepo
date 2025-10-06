import InteractiveCalendar from '@/pages/store/private/products/get-all-sessions/components/interactive-calendar';

export const SessionBookedCalendar = () => {
  return (
    <main className="flex w-full flex-col items-center justify-start px-4 py-10 md:justify-center">
      <InteractiveCalendar />
    </main>
  );
};
